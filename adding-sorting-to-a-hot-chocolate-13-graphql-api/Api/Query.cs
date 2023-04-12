namespace Api;

public class Query
{
    [UseSorting]
    public IQueryable<Author> GetAuthors(ApplicationDbContext context)
    {
        return context.Authors;
    }

    [UseSorting<BookSortType>]
    public IQueryable<Book> GetBooks(ApplicationDbContext context)
    {
        return context.Books;
    }
}
