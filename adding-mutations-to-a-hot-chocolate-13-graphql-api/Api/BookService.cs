namespace Api;

public class BookService
{
    public BookService()
    {
        Books = new List<Book> { new("C# in depth.", new Author("Jon Skeet")) };
    }

    public ICollection<Book> Books { get; }
}

public record Book(string Title, Author Author);

public record Author(string Name);
