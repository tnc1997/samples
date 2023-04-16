namespace Api;

public class Query
{
    public IQueryable<Author> GetAuthors(ApplicationDbContext context)
    {
        return context.Authors;
    }

    public IQueryable<Book> GetBooks(ApplicationDbContext context)
    {
        return context.Books;
    }
}
