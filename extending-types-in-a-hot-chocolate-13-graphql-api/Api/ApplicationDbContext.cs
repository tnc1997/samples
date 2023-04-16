using Microsoft.EntityFrameworkCore;

namespace Api;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Author> Authors => Set<Author>();

    public DbSet<Book> Books => Set<Book>();
}

public class Author
{
    public Author(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; set; }

    public string Name { get; set; }
}

public class Book
{
    public Book(Guid id, string title, Guid authorId)
    {
        Id = id;
        Title = title;
        AuthorId = authorId;
    }

    public Guid Id { get; set; }

    public string Title { get; set; }

    public Guid AuthorId { get; set; }
}
