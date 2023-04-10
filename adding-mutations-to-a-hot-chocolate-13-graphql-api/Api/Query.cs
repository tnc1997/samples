namespace Api;

public class Query
{
    public IEnumerable<Book> GetBooks(BookService bookService)
    {
        return bookService.Books;
    }
}
