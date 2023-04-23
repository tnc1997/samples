namespace Api;

public class BookType : ObjectType<Book>
{
    protected override void Configure(IObjectTypeDescriptor<Book> descriptor)
    {
        descriptor.Field(book => book.AuthorId)
            .Name("author")
            .Resolve(async context =>
            {
                var key = context.Parent<Book>().AuthorId;
                var cancellationToken = context.RequestAborted;

                return await context.DataLoader<AuthorDataLoader>().LoadAsync(key, cancellationToken);
            })
            .Type<NonNullType<AuthorType>>();
    }
}
