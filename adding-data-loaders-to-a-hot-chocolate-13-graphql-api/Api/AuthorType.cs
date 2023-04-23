namespace Api;

public class AuthorType : ObjectType<Author>
{
    protected override void Configure(IObjectTypeDescriptor<Author> descriptor)
    {
        descriptor.Field("books")
            .Resolve(async context =>
            {
                var key = context.Parent<Author>().Id;
                var cancellationToken = context.RequestAborted;

                return await context.DataLoader<AuthorBooksDataLoader>().LoadAsync(key, cancellationToken);
            })
            .Type<NonNullType<ListType<BookType>>>();
    }
}
