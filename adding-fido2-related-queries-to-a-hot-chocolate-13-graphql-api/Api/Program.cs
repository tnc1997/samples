using Api;
using Api.Data;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    options.UseNpgsql(connectionString);
});

builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddTypeConverter<byte[], string>(WebEncoders.Base64UrlEncode)
    .AddTypeConverter<string, byte[]>(WebEncoders.Base64UrlDecode)
    .RegisterDbContext<ApplicationDbContext>();

var app = builder.Build();

app.MapGraphQL();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();
}

app.Run();
