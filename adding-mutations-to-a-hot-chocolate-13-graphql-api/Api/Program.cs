using Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGraphQLServer()
    .AddMutationConventions()
    .AddMutationType<Mutation>()
    .AddQueryType<Query>()
    .RegisterService<BookService>();

builder.Services.AddSingleton<BookService>();

var app = builder.Build();

app.MapGraphQL();

app.Run();
