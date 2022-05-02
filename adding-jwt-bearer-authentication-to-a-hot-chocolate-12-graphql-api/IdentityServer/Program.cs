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

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    if (await roleManager.FindByNameAsync("Administrator") == null)
    {
        await roleManager.CreateAsync(new IdentityRole("Administrator"));
    }

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    if (await userManager.FindByNameAsync("thomas.clark") == null)
    {
        var user = new ApplicationUser
        {
            UserName = "thomas.clark",
            Email = "thomas.clark@example.com",
            GivenName = "Thomas",
            FamilyName = "Clark"
        };

        await userManager.CreateAsync(user, "Pa55w0rd!");

        await userManager.AddToRoleAsync(user, "Administrator");
    }

    var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

    if (!await configurationDbContext.ApiResources.AnyAsync())
    {
        await configurationDbContext.ApiResources.AddAsync(new ApiResource
        {
            Name = "668ed607-34cd-469b-8339-ac3c7a3dc6a2",
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
        await configurationDbContext.Clients.AddAsync(
            new Client
            {
                ClientId = "2bebae3e-7539-4760-984d-d828c6bfd603",
                ClientSecrets = new List<Secret> { new("secret".Sha512()) },
                ClientName = "Banana Cake Pop",
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes = new List<string>
                {
                    "openid",
                    "profile",
                    "email",
                    "role",
                    "https://www.example.com/api"
                },
                AllowedCorsOrigins = new List<string> { "https://api:7001" },
                RedirectUris = new List<string> { "https://api:7001/graphql" }
            }.ToEntity());

        await configurationDbContext.SaveChangesAsync();
    }

    if (!await configurationDbContext.IdentityResources.AnyAsync())
    {
        await configurationDbContext.IdentityResources.AddRangeAsync(
            new IdentityResources.OpenId().ToEntity(),
            new IdentityResources.Profile().ToEntity(),
            new IdentityResources.Email().ToEntity(),
            new IdentityResource("role", new List<string> { "role" }).ToEntity());

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
