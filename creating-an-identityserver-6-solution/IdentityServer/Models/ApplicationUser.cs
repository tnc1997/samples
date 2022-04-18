using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Models;

public class ApplicationUser : IdentityUser
{
    public string? GivenName { get; set; }

    public string? FamilyName { get; set; }
}
