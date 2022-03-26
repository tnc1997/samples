using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Duende.IdentityServer;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using IdentityModel;
using IdentityServer.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                
                using (var persistedGrantDbContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>())
                {
                    persistedGrantDbContext.Database.Migrate();
                }
                
                using (var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>())
                {
                    configurationDbContext.Database.Migrate();

                    if (!configurationDbContext.Clients.Any())
                    {
                        configurationDbContext.Clients.Add(new Client
                        {
                            ClientId = "web",
                            ClientSecrets = new List<Secret> {new("secret".Sha256())},
                            AllowedGrantTypes = GrantTypes.Code,
                            RedirectUris = new List<string> {$"{configuration["WebApplicationUrl"]}/signin-oidc"},
                            PostLogoutRedirectUris = new List<string> {$"{configuration["WebApplicationUrl"]}/signin-oidc"},
                            AllowedScopes = new List<string> {IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, "api"}
                        }.ToEntity());
                        configurationDbContext.SaveChanges();
                    }

                    if (!configurationDbContext.ApiScopes.Any())
                    {
                        configurationDbContext.ApiScopes.Add(new ApiScope("api").ToEntity());
                        configurationDbContext.SaveChanges();
                    }

                    if (!configurationDbContext.ApiResources.Any())
                    {
                        configurationDbContext.ApiResources.Add(new ApiResource("api")
                        {
                            Scopes = new List<string> {"api"}
                        }.ToEntity());
                        configurationDbContext.SaveChanges();
                    }

                    if (!configurationDbContext.IdentityResources.Any())
                    {
                        configurationDbContext.IdentityResources.Add(new IdentityResources.OpenId().ToEntity());
                        configurationDbContext.IdentityResources.Add(new IdentityResources.Profile().ToEntity());
                        configurationDbContext.SaveChanges();
                    }
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