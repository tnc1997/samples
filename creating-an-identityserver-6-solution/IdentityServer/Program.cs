using System.Reflection;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using IdentityServer.Data;
using IdentityServer.Factories;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, dbContextOptionsBuilder) =>
{
    dbContextOptionsBuilder.UseNpgsql(
        serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString("Identity"),
        NpgsqlOptionsAction);
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddClaimsPrincipalFactory<ApplicationUserClaimsPrincipalFactory>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentityServer()
    .AddAspNetIdentity<ApplicationUser>()
    .AddConfigurationStore(configurationStoreOptions =>
    {
        configurationStoreOptions.ResolveDbContextOptions = ResolveDbContextOptions;
    })
    .AddOperationalStore(operationalStoreOptions =>
    {
        operationalStoreOptions.ResolveDbContextOptions = ResolveDbContextOptions;
    });

builder.Services.AddRazorPages();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();

app.UseAuthorization();

app.MapRazorPages();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();
    await scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.MigrateAsync();
    await scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.MigrateAsync();

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    if (await userManager.FindByNameAsync("thomas.clark") == null)
    {
        await userManager.CreateAsync(
            new ApplicationUser
            {
                UserName = "thomas.clark",
                Email = "thomas.clark@example.com",
                GivenName = "Thomas",
                FamilyName = "Clark"
            }, "Pa55w0rd!");
    }

    var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

    if (!await configurationDbContext.ApiResources.AnyAsync())
    {
        await configurationDbContext.ApiResources.AddAsync(new ApiResource
        {
            Name = "9fc33c2e-dbc1-4d0a-b212-68b9e07b3ba0",
            DisplayName = "API",
            Scopes = new List<string> { "https://www.example.com/api" }
        }.ToEntity());


        await configurationDbContext.SaveChangesAsync();
    }

    if (!await configurationDbContext.ApiScopes.AnyAsync())
    {
        await configurationDbContext.ApiScopes.AddAsync(new ApiScope
        {
            Name = "https://www.example.com/api", DisplayName = "API"
        }.ToEntity());

        await configurationDbContext.SaveChangesAsync();
    }

    if (!await configurationDbContext.Clients.AnyAsync())
    {
        await configurationDbContext.Clients.AddRangeAsync(
            new Client
            {
                ClientId = "b4e758d2-f13d-4a1e-bf38-cc88f4e290e1",
                ClientSecrets = new List<Secret> { new("secret".Sha512()) },
                ClientName = "Console Application",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = new List<string> { "https://www.example.com/api" },
                AllowedCorsOrigins = new List<string> { "https://api:7001" }
            }.ToEntity(),
            new Client
            {
                ClientId = "4ecc4153-daf9-4eca-8b60-818a63637a81",
                ClientSecrets = new List<Secret> { new("secret".Sha512()) },
                ClientName = "Web Application",
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes = new List<string> { "openid", "profile", "email", "https://www.example.com/api" },
                RedirectUris = new List<string> { "https://webapplication:7002/signin-oidc" },
                PostLogoutRedirectUris = new List<string> { "https://webapplication:7002/signout-callback-oidc" }
            }.ToEntity(),
            new Client
            {
                ClientId = "7e98ad57-540a-4191-b477-03d88b8187e1",
                RequireClientSecret = false,
                ClientName = "Single Page Application",
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes = new List<string> { "openid", "profile", "email", "https://www.example.com/api" },
                AllowedCorsOrigins = new List<string> { "http://singlepageapplication:7003" },
                RedirectUris =
                    new List<string> { "http://singlepageapplication:7003/authentication/login-callback" },
                PostLogoutRedirectUris = new List<string>
                {
                    "http://singlepageapplication:7003/authentication/logout-callback"
                }
            }.ToEntity());

        await configurationDbContext.SaveChangesAsync();
    }

    if (!await configurationDbContext.IdentityResources.AnyAsync())
    {
        await configurationDbContext.IdentityResources.AddRangeAsync(
            new IdentityResources.OpenId().ToEntity(),
            new IdentityResources.Profile().ToEntity(),
            new IdentityResources.Email().ToEntity());

        await configurationDbContext.SaveChangesAsync();
    }
}

app.Run();

void NpgsqlOptionsAction(NpgsqlDbContextOptionsBuilder npgsqlDbContextOptionsBuilder)
{
    npgsqlDbContextOptionsBuilder.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name);
}

void ResolveDbContextOptions(IServiceProvider serviceProvider, DbContextOptionsBuilder dbContextOptionsBuilder)
{
    dbContextOptionsBuilder.UseNpgsql(
        serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString("IdentityServer"),
        NpgsqlOptionsAction);
}
