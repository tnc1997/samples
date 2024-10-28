using Microsoft.AspNetCore.Identity;

namespace Api.Data;

public class ApplicationUser : IdentityUser
{
    public ICollection<PublicKeyCredential> PublicKeyCredentials { get; } = new List<PublicKeyCredential>();
}
