using Api;
using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, dbContextOptionsBuilder) =>
{
    var connectionString = Array.IndexOf(args, "--connection-string") >= 0
        ? args[Array.IndexOf(args, "--connection-string") + 1]
        : serviceProvider.GetRequiredService<IMultiTenantContextAccessor<TenantInfo>>()
            .MultiTenantContext!
            .TenantInfo!
            .ConnectionString!;

    dbContextOptionsBuilder.UseNpgsql(connectionString);
});

builder.Services.AddMultiTenant<TenantInfo>()
    .WithHostStrategy()
    .WithConfigurationStore();

var app = builder.Build();

app.UseMultiTenant();

app.MapGet("/api/books", ([FromServices] ApplicationDbContext context) => context.Books);

app.Run();
