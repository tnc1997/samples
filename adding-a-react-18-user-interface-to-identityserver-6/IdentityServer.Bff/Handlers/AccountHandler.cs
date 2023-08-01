using Duende.IdentityServer.Services;
using IdentityServer.Bff.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Bff.Handlers;

public class AccountHandler
{
    public static async Task<IResult> LoginAsync([FromServices] IIdentityServerInteractionService interactionService,
        [FromServices] IServerUrls serverUrls,
        [FromServices] SignInManager<ApplicationUser> signInManager,
        [FromBody] LoginInputModel input)
    {
        var returnUrl =
            input.ReturnUrl != null && await interactionService.GetAuthorizationContextAsync(input.ReturnUrl) != null
                ? input.ReturnUrl
                : serverUrls.BaseUrl;

        var result = await signInManager.PasswordSignInAsync(input.UserName, input.Password, input.IsPersistent, true);

        return result.Succeeded ? Results.Json(new { returnUrl }) : Results.BadRequest(result.ToString());
    }

    public record LoginInputModel(string UserName, string Password, string? ReturnUrl, bool IsPersistent = false);

    public static async Task<IResult> LogoutAsync([FromServices] IIdentityServerInteractionService interactionService,
        [FromServices] SignInManager<ApplicationUser> signInManager,
        [FromBody] LogoutInputModel input)
    {
        var request = await interactionService.GetLogoutContextAsync(input.LogoutId);

        await signInManager.SignOutAsync();

        return Results.Json(new
        {
            iFrameUrl = request.SignOutIFrameUrl, postLogoutRedirectUri = request.PostLogoutRedirectUri
        });
    }

    public record LogoutInputModel(string? LogoutId);
}
