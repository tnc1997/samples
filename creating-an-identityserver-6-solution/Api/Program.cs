using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

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

builder.Services.AddAuthorization(authorizationOptions =>
{
    authorizationOptions.AddPolicy("ApiScope", authorizationPolicyBuilder =>
    {
        authorizationPolicyBuilder.RequireAuthenticatedUser().RequireClaim("scope", "https://www.example.com/api");
    });
});

builder.Services.AddControllers();

builder.Services.AddCors(corsOptions =>
{
    corsOptions.AddDefaultPolicy(corsPolicyBuilder =>
    {
        corsPolicyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(swaggerGenOptions =>
{
    swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });

    swaggerGenOptions.AddSecurityDefinition("oauth2",
        new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                ClientCredentials = new OpenApiOAuthFlow
                {
                    TokenUrl =
                        new Uri($"{builder.Configuration["Authentication:Authority"]}/connect/token"),
                    Scopes = { { "https://www.example.com/api", "API" } }
                }
            }
        });

    swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
            },
            new List<string> { "https://www.example.com/api" }
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
