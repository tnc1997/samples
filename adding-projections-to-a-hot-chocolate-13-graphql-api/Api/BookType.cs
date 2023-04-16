namespace Api;

public class BookType : ObjectType<Book>
{
    protected override void Configure(IObjectTypeDescriptor<Book> descriptor)
    {
        descriptor.Field(book => book.AuthorId).IsProjected();
    }
}
