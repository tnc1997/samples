using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Bff.Models;

public class ApplicationUser : IdentityUser
{
    public string? GivenName { get; set; }
    
    public string? FamilyName { get; set; }
}
