using Microsoft.AspNetCore.Identity;

namespace WebApplication.Data;

public class ApplicationUser : IdentityUser
{
    public ICollection<PublicKeyCredential> PublicKeyCredentials { get; } = new List<PublicKeyCredential>();
}
