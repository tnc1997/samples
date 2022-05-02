using Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(jwtBearerOptions =>
    {
        jwtBearerOptions.Authority = builder.Configuration["Authentication:Authority"];
        jwtBearerOptions.Audience = builder.Configuration["Authentication:Audience"];

        jwtBearerOptions.TokenValidationParameters.ValidateAudience = true;
        jwtBearerOptions.TokenValidationParameters.ValidateIssuer = true;
        jwtBearerOptions.TokenValidationParameters.ValidateIssuerSigningKey = true;
    });

builder.Services.AddGraphQLServer()
    .AddAuthorization()
    .AddQueryType<Query>();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();

app.MapGraphQL();

app.Run();
