using Microsoft.EntityFrameworkCore;

namespace Api;

public class AuthorBooksDataLoader : GroupedDataLoader<Guid, Book>
{
    private ApplicationDbContext Context { get; }
    
    public AuthorBooksDataLoader(IBatchScheduler batchScheduler, ApplicationDbContext context, DataLoaderOptions? options = null) : base(batchScheduler, options)
    {
        Context = context;
    }

    protected override async Task<ILookup<Guid, Book>> LoadGroupedBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var books = await Context.Books.Where(book => keys.Contains(book.AuthorId)).ToListAsync(cancellationToken);

        return books.ToLookup(book => book.AuthorId);
    }
}
