using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMultiTenant<TenantInfo>()
    .WithHostStrategy()
    .WithConfigurationStore();

var app = builder.Build();

app.UseMultiTenant();

app.MapGet("/",
    ([FromServices] IMultiTenantContextAccessor<TenantInfo> multiTenantContextAccessor) =>
        $"Hello {multiTenantContextAccessor.MultiTenantContext!.TenantInfo!.Name!}!");

app.Run();
