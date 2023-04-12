using HotChocolate.Data.Sorting;

namespace Api;

public class BookSortType : SortInputType<Book>
{
    protected override void Configure(ISortInputTypeDescriptor<Book> descriptor)
    {
        descriptor.Field(book => book.Title).Type<BookTitleSortEnumType>();
    }
}
