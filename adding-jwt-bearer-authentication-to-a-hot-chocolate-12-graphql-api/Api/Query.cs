using System.Security.Claims;

namespace Api;

public class Query
{
    public Book GetBook()
    {
        return new Book("C# in depth.", new Author("Jon Skeet"));
    }

    public string? GetMe(ClaimsPrincipal? user)
    {
        return user?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}

public record Book(string Title, Author Author);

public record Author(string Name);
