using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers;

[AllowAnonymous]
[Route("[controller]/[action]")]
public class AccountController : Controller
{
    [HttpGet("{scheme?}")]
    public IActionResult SignIn([FromRoute] string? scheme, [FromQuery] string? redirectUri)
    {
        scheme ??= OpenIdConnectDefaults.AuthenticationScheme;

        string redirect;

        if (!string.IsNullOrEmpty(redirectUri) && Url.IsLocalUrl(redirectUri))
        {
            redirect = redirectUri;
        }
        else
        {
            redirect = Url.Content("~/");
        }

        var properties = new AuthenticationProperties { RedirectUri = redirect };

        return Challenge(properties, scheme);
    }

    [HttpGet("{scheme?}")]
    public IActionResult SignOut([FromRoute] string? scheme)
    {
        scheme ??= OpenIdConnectDefaults.AuthenticationScheme;

        var callbackUrl = Url.Page("/Index", null, null, Request.Scheme);

        var properties = new AuthenticationProperties { RedirectUri = callbackUrl };

        return SignOut(properties, CookieAuthenticationDefaults.AuthenticationScheme, scheme);
    }
}
