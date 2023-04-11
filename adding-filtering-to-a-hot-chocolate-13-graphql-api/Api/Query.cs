namespace Api;

public class Query
{
    [UseFiltering]
    public IQueryable<Author> GetAuthors(ApplicationDbContext context)
    {
        return context.Authors;
    }

    [UseFiltering<BookFilterType>]
    public IQueryable<Book> GetBooks(ApplicationDbContext context)
    {
        return context.Books;
    }
}
