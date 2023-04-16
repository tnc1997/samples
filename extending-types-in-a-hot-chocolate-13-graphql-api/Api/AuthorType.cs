namespace Api;

public class AuthorType : ObjectType<Author>
{
    protected override void Configure(IObjectTypeDescriptor<Author> descriptor)
    {
        descriptor.Field("books")
            .Resolve(context =>
            {
                var id = context.Parent<Author>().Id;

                return context.Service<ApplicationDbContext>().Books.Where(book => book.AuthorId == id);
            })
            .Serial()
            .Type<NonNullType<ListType<BookType>>>();
    }
}
