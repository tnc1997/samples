using Microsoft.EntityFrameworkCore;
using WebApplication.Data;
using WebApplication.Handlers;
using Microsoft.AspNetCore.Identity;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    options.UseNpgsql(connectionString);
});

builder.Services.AddDefaultIdentity<ApplicationUser>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddFido2(options =>
{
    options.Origins = builder.Configuration.GetSection("Fido2:Origins").Get<HashSet<string>>();
    options.ServerDomain = builder.Configuration["Fido2:ServerDomain"];
    options.ServerName = builder.Configuration["Fido2:ServerName"];
    options.TimestampDriftTolerance = builder.Configuration.GetValue<int>("Fido2:TimestampDriftTolerance");
});

builder.Services.AddRazorPages();

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.MapPost("/fido2/createassertion", Fido2Handler.CreateAssertion);
app.MapPost("/fido2/createassertionoptions", Fido2Handler.CreateAssertionOptions);
app.MapPost("/fido2/createattestation", Fido2Handler.CreateAttestation);
app.MapPost("/fido2/createattestationoptions", Fido2Handler.CreateAttestationOptions);

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();
}

app.Run();
