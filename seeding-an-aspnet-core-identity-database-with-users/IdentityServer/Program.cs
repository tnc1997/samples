using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Duende.IdentityServer.EntityFramework.DbContexts;
using IdentityModel;
using IdentityServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = CreateHostBuilder(args).Build();

            using (var scope = builder.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var persistedGrantDbContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>())
                {
                    persistedGrantDbContext.Database.Migrate();
                }
                
                using (var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>())
                {
                    configurationDbContext.Database.Migrate();
                }
                
                using (var applicationDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
                {
                    applicationDbContext.Database.Migrate();

                    if (!applicationDbContext.Users.Any())
                    {
                        using (var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>())
                        {
                            var user = new IdentityUser("test@example.com")
                            {
                                Email = "test@example.com"
                            };

                            userManager.CreateAsync(user, "Pass123$").Wait();
                            userManager.AddClaimsAsync(user, new List<Claim>
                            {
                                new(JwtClaimTypes.FamilyName, "Clark"),
                                new(JwtClaimTypes.GivenName, "Thomas")
                            }).Wait();
                        }
                    }
                }
            }

            builder.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}