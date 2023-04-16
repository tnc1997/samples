namespace Api;

public class BookType : ObjectType<Book>
{
    protected override void Configure(IObjectTypeDescriptor<Book> descriptor)
    {
        descriptor.Field(book => book.AuthorId)
            .Name("author")
            .Resolve(async context =>
            {
                var keyValues = new object[] { context.Parent<Book>().AuthorId };
                var cancellationToken = context.RequestAborted;

                return await context.Service<ApplicationDbContext>().Authors.FindAsync(keyValues, cancellationToken);
            })
            .Serial()
            .Type<NonNullType<AuthorType>>();
    }
}
