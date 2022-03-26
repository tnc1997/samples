using System.Collections.Generic;
using System.Linq;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Hosting;
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

                    if (!configurationDbContext.Clients.Any())
                    {
                        configurationDbContext.Clients.Add(new Client
                        {
                            ClientId = "console",
                            ClientSecrets = new List<Secret> {new("secret".Sha256())},
                            AllowedGrantTypes = GrantTypes.ClientCredentials,
                            AllowedScopes = new List<string> {"api"}
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