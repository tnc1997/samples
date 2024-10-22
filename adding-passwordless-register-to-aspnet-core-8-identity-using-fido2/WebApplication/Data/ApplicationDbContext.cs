using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApplication.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<AuthenticatorTransport> AuthenticatorTransports { get; set; }
    
    public DbSet<DevicePublicKey> DevicePublicKeys { get; set; }
    
    public DbSet<PublicKeyCredential> PublicKeyCredentials { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<AuthenticatorTransport>()
            .ToTable("Fido2AuthenticatorTransports")
            .HasKey(transport => new { transport.PublicKeyCredentialId, transport.Value });

        builder.Entity<DevicePublicKey>()
            .ToTable("Fido2DevicePublicKeys")
            .HasKey(key => new { key.PublicKeyCredentialId, key.Value });

        builder.Entity<PublicKeyCredential>()
            .ToTable("Fido2PublicKeyCredentials")
            .HasOne<ApplicationUser>()
            .WithMany(user => user.PublicKeyCredentials)
            .HasForeignKey(credential => credential.UserId);
    }
}
