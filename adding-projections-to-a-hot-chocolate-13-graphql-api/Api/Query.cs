namespace Api;

public class Query
{
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Author> GetAuthors(ApplicationDbContext context)
    {
        return context.Authors;
    }

    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Book> GetBooks(ApplicationDbContext context)
    {
        return context.Books;
    }
}
