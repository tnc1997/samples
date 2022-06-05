using Duende.IdentityServer.Services;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers;

public class AccountController : Controller
{
    public AccountController(IIdentityServerInteractionService interactionService, IServerUrls serverUrls,
        SignInManager<ApplicationUser> signInManager)
    {
        InteractionService = interactionService;
        ServerUrls = serverUrls;
        SignInManager = signInManager;
    }

    private IIdentityServerInteractionService InteractionService { get; }

    private IServerUrls ServerUrls { get; }

    private SignInManager<ApplicationUser> SignInManager { get; }

    [HttpPost("/api/login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var returnUrl = model.ReturnUrl != null ? Uri.UnescapeDataString(model.ReturnUrl) : null;

        if (returnUrl == null || await InteractionService.GetAuthorizationContextAsync(returnUrl) == null)
        {
            returnUrl = ServerUrls.BaseUrl;
        }

        var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.IsPersistent, false);

        return result.Succeeded ? Ok(new { returnUrl }) : BadRequest();
    }

    [HttpGet("/api/logout")]
    public async Task<IActionResult> Logout([FromQuery] string logoutId)
    {
        var logoutRequest = await InteractionService.GetLogoutContextAsync(logoutId);

        if (logoutRequest == null || (logoutRequest.ShowSignoutPrompt && User.Identity?.IsAuthenticated == true))
        {
            return Ok(new { prompt = User.Identity?.IsAuthenticated ?? false });
        }

        await SignInManager.SignOutAsync();

        return Ok(new
        {
            iFrameUrl = logoutRequest.SignOutIFrameUrl, postLogoutRedirectUri = logoutRequest.PostLogoutRedirectUri
        });
    }

    [HttpPost("/api/logout")]
    public async Task<IActionResult> PostLogout([FromQuery] string logoutId)
    {
        var logoutRequest = await InteractionService.GetLogoutContextAsync(logoutId);

        await SignInManager.SignOutAsync();

        return Ok(new
        {
            iFrameUrl = logoutRequest?.SignOutIFrameUrl,
            postLogoutRedirectUri = logoutRequest?.PostLogoutRedirectUri
        });
    }

    public record LoginRequestModel(string UserName, string Password, string? ReturnUrl, bool IsPersistent = false);
}
