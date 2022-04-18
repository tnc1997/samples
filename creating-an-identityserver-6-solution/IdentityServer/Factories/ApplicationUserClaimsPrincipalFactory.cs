using System.Security.Claims;
using IdentityModel;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace IdentityServer.Factories;

public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
{
    public ApplicationUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager,
        IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
    {
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var claimsIdentity = await base.GenerateClaimsAsync(user);

        if (user.GivenName != null)
        {
            claimsIdentity.AddClaim(new Claim(JwtClaimTypes.GivenName, user.GivenName));
        }

        if (user.FamilyName != null)
        {
            claimsIdentity.AddClaim(new Claim(JwtClaimTypes.FamilyName, user.FamilyName));
        }

        return claimsIdentity;
    }
}
