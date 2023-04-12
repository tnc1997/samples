namespace Api;

public class Mutation
{
    public async Task<Author> AddAuthor(ApplicationDbContext context, string name)
    {
        var author = new Author(Guid.NewGuid(), name);

        await context.Authors.AddAsync(author);
        await context.SaveChangesAsync();

        return author;
    }

    public async Task<Book> AddBook(ApplicationDbContext context, string title, Guid authorId)
    {
        var author = await context.Authors.FindAsync(authorId);

        if (author == null)
        {
            throw new ArgumentException($"Failed to find the author '{authorId}'.", nameof(authorId));
        }

        var book = new Book(Guid.NewGuid(), title, authorId) { Author = author };

        await context.Books.AddAsync(book);
        await context.SaveChangesAsync();

        return book;
    }
}
