using Fido2NetLib;
using Fido2NetLib.Objects;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using AuthenticatorTransport = WebApplication.Data.AuthenticatorTransport;

namespace WebApplication.Handlers;

public static class Fido2Handler
{
    public static async Task<Results<BadRequest, Ok<VerifyAssertionResult>>> CreateAssertion(
        ApplicationDbContext applicationDbContext,
        [FromBody] AuthenticatorAssertionRawResponse assertionResponse,
        IFido2 fido2,
        HttpContext httpContext,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        CancellationToken cancellationToken = default)
    {
        var json = httpContext.Session.GetString("Fido2AssertionOptions");

        if (string.IsNullOrEmpty(json))
        {
            return TypedResults.BadRequest();
        }

        var options = AssertionOptions.FromJson(json);

        var credential = await userManager.Users
            .SelectMany(user => user.PublicKeyCredentials)
            .Include(credential => credential.DevicePublicKeys)
            .SingleOrDefaultAsync(credential => credential.Id == assertionResponse.Id, cancellationToken);

        if (credential is null)
        {
            return TypedResults.BadRequest();
        }

        var user = await userManager.FindByIdAsync(credential.UserId);

        if (user is null)
        {
            return TypedResults.BadRequest();
        }

        var assertionResult = await fido2.MakeAssertionAsync(
            assertionResponse,
            options,
            credential.PublicKey,
            credential.DevicePublicKeys.Select(key => key.Value).ToList(),
            credential.SignatureCounter,
            async (@params, cancellationToken) =>
                await userManager.Users
                    .Where(user => user.Id == new Guid(@params.UserHandle).ToString())
                    .SelectMany(user => user.PublicKeyCredentials)
                    .AnyAsync(credential => credential.Id == @params.CredentialId, cancellationToken),
            cancellationToken);

        credential.SignatureCounter = assertionResult.SignCount;

        if (assertionResult.DevicePublicKey is not null)
        {
            credential.DevicePublicKeys.Add(new DevicePublicKey
            {
                PublicKeyCredentialId = assertionResult.CredentialId,
                Value = assertionResult.DevicePublicKey
            });
        }

        applicationDbContext.PublicKeyCredentials.Update(credential);

        await applicationDbContext.SaveChangesAsync(cancellationToken);

        await signInManager.SignInAsync(user, false);

        return TypedResults.Ok(assertionResult);
    }
    
    public static async Task<Ok<AssertionOptions>> CreateAssertionOptions(
        IFido2 fido2,
        HttpContext httpContext,
        [FromBody] CreateAssertionOptionsInputModel input,
        UserManager<ApplicationUser> userManager,
        CancellationToken cancellationToken = default)
    {
        var normalizedUserName = userManager.NormalizeName(input.UserName);

        var allowedCredentials = await userManager.Users
            .Where(user => user.NormalizedUserName == normalizedUserName)
            .SelectMany(user => user.PublicKeyCredentials)
            .Select(credential => new PublicKeyCredentialDescriptor(credential.Id))
            .ToListAsync(cancellationToken);

        var extensions = new AuthenticationExtensionsClientInputs
        {
            Extensions = true,
            UserVerificationMethod = true,
            DevicePubKey = new AuthenticationExtensionsDevicePublicKeyInputs()
        };

        var options = fido2.GetAssertionOptions(
            allowedCredentials,
            input.UserVerification,
            extensions);

        httpContext.Session.SetString("Fido2AssertionOptions", options.ToJson());

        return TypedResults.Ok(options);
    }
    
    public class CreateAssertionOptionsInputModel
    {
        public required string UserName { get; set; }

        public UserVerificationRequirement UserVerification { get; set; } = UserVerificationRequirement.Preferred;
    }
    
    public static async Task<Results<BadRequest, Ok<RegisteredPublicKeyCredential>>> CreateAttestation(
        [FromBody] AuthenticatorAttestationRawResponse attestationResponse,
        IFido2 fido2,
        HttpContext httpContext,
        SignInManager<ApplicationUser> signInManager,
        UserManager<ApplicationUser> userManager,
        CancellationToken cancellationToken = default)
    {
        var json = httpContext.Session.GetString("Fido2AttestationOptions");

        if (string.IsNullOrEmpty(json))
        {
            return TypedResults.BadRequest();
        }

        var options = CredentialCreateOptions.FromJson(json);

        var credentialResult = await fido2.MakeNewCredentialAsync(
            attestationResponse,
            options,
            async (@params, cancellationToken) =>
                await userManager.Users
                    .SelectMany(user => user.PublicKeyCredentials)
                    .AllAsync(credential => credential.Id != @params.CredentialId, cancellationToken),
            cancellationToken);

        if (credentialResult.Result is null)
        {
            throw new Exception(credentialResult.ErrorMessage);
        }

        var credential = new PublicKeyCredential
        {
            Id = credentialResult.Result.Id,
            PublicKey = credentialResult.Result.PublicKey,
            SignatureCounter = credentialResult.Result.SignCount,
            IsBackupEligible = credentialResult.Result.IsBackupEligible,
            IsBackedUp = credentialResult.Result.IsBackedUp,
            AttestationObject = credentialResult.Result.AttestationObject,
            AttestationClientDataJson = credentialResult.Result.AttestationClientDataJson,
            AttestationFormat = credentialResult.Result.AttestationFormat,
            AaGuid = credentialResult.Result.AaGuid,
            UserId = new Guid(credentialResult.Result.User.Id).ToString()
        };

        foreach (var authenticatorTransport in credentialResult.Result.Transports)
        {
            credential.AuthenticatorTransports.Add(new AuthenticatorTransport
            {
                PublicKeyCredentialId = credentialResult.Result.Id,
                Value = authenticatorTransport
            });
        }

        if (credentialResult.Result.DevicePublicKey is not null)
        {
            credential.DevicePublicKeys.Add(new DevicePublicKey
            {
                PublicKeyCredentialId = credentialResult.Result.Id,
                Value = credentialResult.Result.DevicePublicKey
            });
        }

        var user = new ApplicationUser
        {
            Id = new Guid(credentialResult.Result.User.Id).ToString(),
            UserName = credentialResult.Result.User.Name,
            PublicKeyCredentials = { credential }
        };

        var identityResult = await userManager.CreateAsync(user);

        if (!identityResult.Succeeded)
        {
            throw new Exception(identityResult.ToString());
        }

        await signInManager.SignInAsync(user, false);

        return TypedResults.Ok(credentialResult.Result);
    }
    
    public static Ok<CredentialCreateOptions> CreateAttestationOptions(
        IFido2 fido2,
        HttpContext httpContext,
        [FromBody] CreateAttestationOptionsInputModel input)
    {
        var user = new Fido2User
        {
            Name = input.UserName,
            Id = Guid.NewGuid().ToByteArray(),
            DisplayName = input.UserName
        };

        var authenticatorSelection = new AuthenticatorSelection
        {
            AuthenticatorAttachment = input.AuthenticatorAttachment,
            ResidentKey = input.ResidentKey,
            UserVerification = input.UserVerification
        };

        var attestationPreference = input.AttestationType.ToEnum<AttestationConveyancePreference>();

        var extensions = new AuthenticationExtensionsClientInputs
        {
            Extensions = true,
            UserVerificationMethod = true,
            DevicePubKey = new AuthenticationExtensionsDevicePublicKeyInputs { Attestation = input.AttestationType },
            CredProps = true
        };

        var options = fido2.RequestNewCredential(
            user,
            new List<PublicKeyCredentialDescriptor>(),
            authenticatorSelection,
            attestationPreference,
            extensions);

        httpContext.Session.SetString("Fido2AttestationOptions", options.ToJson());

        return TypedResults.Ok(options);
    }
    
    public class CreateAttestationOptionsInputModel
    {
        public required string UserName { get; set; }

        public string AttestationType { get; set; } = "none";

        public AuthenticatorAttachment? AuthenticatorAttachment { get; set; }

        public ResidentKeyRequirement ResidentKey { get; set; } = ResidentKeyRequirement.Discouraged;

        public UserVerificationRequirement UserVerification { get; set; } = UserVerificationRequirement.Preferred;
    }
}
