// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApplication.Data;

namespace WebApplication.Areas.Identity.Pages.Account;

public class LoginModel(SignInManager<ApplicationUser> signInManager) : PageModel
{
        public string ReturnUrl { get; set; }

        public List<AuthenticationScheme> ExternalLogins { get; set; }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }
}
