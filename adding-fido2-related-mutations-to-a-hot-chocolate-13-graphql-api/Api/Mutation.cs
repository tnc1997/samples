using System.Text;
using System.Text.Json;
using Api.Data;
using Api.Exceptions;
using Api.Types;
using Fido2NetLib;
using Fido2NetLib.Objects;
using Fido2NetLib.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace Api;

public class Mutation
{
    [Error(typeof(Fido2Exception))]
    [Error(typeof(PublicKeyCredentialCreationOptionsNotFoundException))]
    [GraphQLType(typeof(NonNullType<Types.PublicKeyCredentialType>))]
    public async Task<PublicKeyCredential> AddPublicKeyCredentialAsync(
        IDistributedCache cache,
        ApplicationDbContext context,
        IFido2 fido2,
        [GraphQLType(typeof(NonNullType<IdType>))]
        byte[] id,
        [GraphQLType(typeof(NonNullType<StringType>))]
        byte[] rawId,
        [GraphQLType(typeof(NonNullType<StringType>))]
        Fido2NetLib.Objects.PublicKeyCredentialType type,
        [GraphQLType(typeof(NonNullType<JsonType>))]
        JsonElement clientExtensionResults,
        [GraphQLType(typeof(NonNullType<AuthenticatorAttestationResponseInputType>))]
        AuthenticatorAttestationRawResponse.AttestationResponse response,
        [GraphQLType(typeof(NonNullType<IdType>))]
        string userId,
        CancellationToken cancellationToken = default)
    {
        var json = await cache.GetStringAsync($"Users:{userId}:Fido2AttestationOptions", cancellationToken);

        if (string.IsNullOrEmpty(json))
        {
            throw new PublicKeyCredentialCreationOptionsNotFoundException();
        }

        var attestationResponse = new AuthenticatorAttestationRawResponse
        {
            Id = id,
            RawId = rawId,
            Type = type,
            Response = response,
            ClientExtensionResults = clientExtensionResults.Deserialize(FidoModelSerializerContext.Default.AuthenticationExtensionsClientOutputs)
        };

        var options = CredentialCreateOptions.FromJson(json);

        var result = await fido2.MakeNewCredentialAsync(
            attestationResponse,
            options,
            async (@params, cancellationToken) =>
                !await context.PublicKeyCredentials.AnyAsync(
                    credential => credential.Id == @params.CredentialId,
                    cancellationToken),
            cancellationToken);

        if (result.Result is null)
        {
            throw new Fido2Exception(result.ErrorMessage);
        }

        var credential = new PublicKeyCredential
        {
            Id = result.Result.Id,
            PublicKey = result.Result.PublicKey,
            SignatureCounter = result.Result.SignCount,
            IsBackupEligible = result.Result.IsBackupEligible,
            IsBackedUp = result.Result.IsBackedUp,
            AttestationObject = result.Result.AttestationObject,
            AttestationClientDataJson = result.Result.AttestationClientDataJson,
            AttestationFormat = result.Result.AttestationFormat,
            AaGuid = result.Result.AaGuid,
            UserId = userId
        };

        foreach (var authenticatorTransport in result.Result.Transports)
        {
            credential.AuthenticatorTransports.Add(new Data.AuthenticatorTransport
            {
                PublicKeyCredentialId = result.Result.Id,
                Value = authenticatorTransport
            });
        }

        if (result.Result.DevicePublicKey is not null)
        {
            credential.DevicePublicKeys.Add(new DevicePublicKey
            {
                PublicKeyCredentialId = result.Result.Id,
                Value = result.Result.DevicePublicKey
            });
        }

        await context.PublicKeyCredentials.AddAsync(credential, cancellationToken);

        await context.SaveChangesAsync(cancellationToken);

        return credential;
    }
    
    [Error(typeof(UserNotFoundException))]
    [GraphQLType(typeof(NonNullType<JsonType>))]
    public async Task<JsonElement> CreatePublicKeyCredentialCreationOptionsAsync(
        IDistributedCache cache,
        ApplicationDbContext context,
        IFido2 fido2,
        [GraphQLType(typeof(NonNullType<IdType>))]
        string userId,
        [GraphQLType(typeof(StringType))]
        AuthenticatorAttachment? authenticatorAttachment,
        [GraphQLType(typeof(NonNullType<StringType>))]
        ResidentKeyRequirement residentKey = ResidentKeyRequirement.Discouraged,
        [GraphQLType(typeof(NonNullType<StringType>))]
        UserVerificationRequirement userVerification = UserVerificationRequirement.Preferred,
        CancellationToken cancellationToken = default)
    {
        var user = await context.Users
            .Include(user => user.PublicKeyCredentials)
            .SingleOrDefaultAsync(user => user.Id == userId, cancellationToken);

        if (user is null)
        {
            throw new UserNotFoundException();
        }

        var fido2User = new Fido2User
        {
            Name = user.UserName,
            Id = Encoding.UTF8.GetBytes(user.Id),
            DisplayName = user.UserName
        };

        var excludeCredentials = user.PublicKeyCredentials
            .Select(credential => new PublicKeyCredentialDescriptor(credential.Id))
            .ToList();

        var authenticatorSelection = new AuthenticatorSelection
        {
            AuthenticatorAttachment = authenticatorAttachment,
            ResidentKey = residentKey,
            UserVerification = userVerification
        };

        var extensions = new AuthenticationExtensionsClientInputs
        {
            Extensions = true,
            UserVerificationMethod = true,
            DevicePubKey = new AuthenticationExtensionsDevicePublicKeyInputs(),
            CredProps = true
        };

        var options = fido2.RequestNewCredential(
            fido2User,
            excludeCredentials,
            authenticatorSelection,
            AttestationConveyancePreference.None,
            extensions);

        await cache.SetStringAsync(
            $"Users:{user.Id}:Fido2AttestationOptions",
            options.ToJson(),
            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5) },
            cancellationToken);

        return JsonSerializer.SerializeToElement(options, FidoModelSerializerContext.Default.CredentialCreateOptions);
    }
}
