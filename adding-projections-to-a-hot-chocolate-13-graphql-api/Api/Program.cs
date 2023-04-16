using Api;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, dbContextOptionsBuilder) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();

    dbContextOptionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddGraphQLServer()
    .AddFiltering()
    .AddMutationConventions()
    .AddMutationType<Mutation>()
    .AddProjections()
    .AddQueryType<Query>()
    .AddSorting()
    .AddType<BookType>()
    .RegisterDbContext<ApplicationDbContext>();

var app = builder.Build();

app.MapGraphQL();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();

    await using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await context.Database.MigrateAsync();

    if (!await context.Authors.AnyAsync())
    {
        await context.Authors.AddRangeAsync(new List<Author>
        {
            new(Guid.Parse("b7cfc48b-7cb2-43aa-a9f5-604be03632fb"), "Jon Skeet"),
            new(Guid.Parse("7a7e7cd5-1890-4da3-9898-0edc70620d9c"), "Jamie Chan"),
            new(Guid.Parse("565a9488-475e-43fc-8b90-cdfa2b4601c8"), "Andrew Stellman"),
            new(Guid.Parse("188c6bf0-b67d-440a-9106-a9903cd15aff"), "Stephen Cleary"),
            new(Guid.Parse("9a434980-de15-409e-a5fc-28e813dcf2ac"), "Rob Miles"),
            new(Guid.Parse("e376d297-a348-4275-8c63-b4e2dca32b72"), "Micah Martin"),
            new(Guid.Parse("a59f2804-5e7e-4d94-9829-7506f8d80437"), "Nathan Clark"),
            new(Guid.Parse("01500c20-e232-4174-9397-37bcbc04cc82"), "R. B. Whitaker"),
            new(Guid.Parse("595c0019-7b77-4f08-bf1c-7f4c49fa037e"), "John Sharp")
        });

        await context.SaveChangesAsync();
    }

    if (!await context.Books.AnyAsync())
    {
        await context.Books.AddRangeAsync(new List<Book>
        {
            new(Guid.Parse("5d1540f2-9eba-481c-b5ce-a07ac11f28b8"), "C# in Depth", Guid.Parse("b7cfc48b-7cb2-43aa-a9f5-604be03632fb")),
            new(Guid.Parse("a9678ba5-775b-43d6-bbbe-7770ed549fd8"), "Learn C# in One Day and Learn It Well", Guid.Parse("7a7e7cd5-1890-4da3-9898-0edc70620d9c")),
            new(Guid.Parse("83a24e9e-85d2-4a68-9240-41cb71b0ecdb"), "Head First C#", Guid.Parse("565a9488-475e-43fc-8b90-cdfa2b4601c8")),
            new(Guid.Parse("df3ed837-f99b-435c-b633-d6b3d9d6bc35"), "Concurrency in C# Cookbook", Guid.Parse("188c6bf0-b67d-440a-9106-a9903cd15aff")),
            new(Guid.Parse("5a5e62e7-8f59-4586-9b85-31fcb16f027b"), "The C# Programming Yellow Book", Guid.Parse("9a434980-de15-409e-a5fc-28e813dcf2ac")),
            new(Guid.Parse("0cc8ec4f-9459-46ee-9d0b-3eebfeaaa188"), "Agile Principles, Patterns, and Practices in C#", Guid.Parse("e376d297-a348-4275-8c63-b4e2dca32b72")),
            new(Guid.Parse("e4871490-7ddf-492d-ae52-22bfadaada78"), "C#: Programming Basics for Absolute Beginners", Guid.Parse("a59f2804-5e7e-4d94-9829-7506f8d80437")),
            new(Guid.Parse("18aef1e9-d8bb-40de-a834-e2caf08ad1b5"), "The C# Player's Guide", Guid.Parse("01500c20-e232-4174-9397-37bcbc04cc82")),
            new(Guid.Parse("b5d30815-5eeb-496b-abe6-977aaeb6256a"), "Microsoft Visual C# 2005 Step by Step", Guid.Parse("595c0019-7b77-4f08-bf1c-7f4c49fa037e"))
        });

        await context.SaveChangesAsync();
    }
}

app.Run();
