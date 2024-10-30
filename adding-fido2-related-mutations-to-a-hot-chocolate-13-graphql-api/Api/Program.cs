using Api;
using Api.Data;
using Fido2NetLib;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    options.UseNpgsql(connectionString);
});

builder.Services.AddDistributedMemoryCache();

builder.Services.AddFido2(options =>
{
    options.Origins = builder.Configuration.GetSection("Fido2:Origins").Get<HashSet<string>>();
    options.ServerDomain = builder.Configuration["Fido2:ServerDomain"];
    options.ServerName = builder.Configuration["Fido2:ServerName"];
    options.TimestampDriftTolerance = builder.Configuration.GetValue<int>("Fido2:TimestampDriftTolerance");
});

builder.Services.AddGraphQLServer()
    .AddMutationConventions()
    .AddMutationType<Mutation>()
    .AddQueryType<Query>()
    .AddTypeConverter<byte[], string>(WebEncoders.Base64UrlEncode)
    .AddTypeConverter<string, byte[]>(WebEncoders.Base64UrlDecode)
    .AddTypeConverter<Fido2NetLib.Objects.AuthenticatorAttachment, string>(source => source.ToEnumMemberValue())
    .AddTypeConverter<string, Fido2NetLib.Objects.AuthenticatorAttachment>(source => source.ToEnum<Fido2NetLib.Objects.AuthenticatorAttachment>())
    .AddTypeConverter<Fido2NetLib.Objects.AuthenticatorTransport, string>(source => source.ToEnumMemberValue())
    .AddTypeConverter<string, Fido2NetLib.Objects.AuthenticatorTransport>(source => source.ToEnum<Fido2NetLib.Objects.AuthenticatorTransport>())
    .AddTypeConverter<Fido2NetLib.Objects.PublicKeyCredentialType, string>(source => source.ToEnumMemberValue())
    .AddTypeConverter<string, Fido2NetLib.Objects.PublicKeyCredentialType>(source => source.ToEnum<Fido2NetLib.Objects.PublicKeyCredentialType>())
    .AddTypeConverter<Fido2NetLib.Objects.ResidentKeyRequirement, string>(source => source.ToEnumMemberValue())
    .AddTypeConverter<string, Fido2NetLib.Objects.ResidentKeyRequirement>(source => source.ToEnum<Fido2NetLib.Objects.ResidentKeyRequirement>())
    .AddTypeConverter<Fido2NetLib.Objects.UserVerificationRequirement, string>(source => source.ToEnumMemberValue())
    .AddTypeConverter<string, Fido2NetLib.Objects.UserVerificationRequirement>(source => source.ToEnum<Fido2NetLib.Objects.UserVerificationRequirement>())
    .RegisterDbContext<ApplicationDbContext>()
    .RegisterService<IDistributedCache>()
    .RegisterService<IFido2>();

var app = builder.Build();

app.UseStaticFiles();

app.MapGraphQL();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();
}

app.Run();
