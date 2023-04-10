using Api;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, dbContextOptionsBuilder) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();

    dbContextOptionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddGraphQLServer()
    .AddMutationConventions()
    .AddMutationType<Mutation>()
    .AddQueryType<Query>()
    .RegisterDbContext<ApplicationDbContext>();

var app = builder.Build();

app.MapGraphQL();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();
}

app.Run();
