using Microsoft.EntityFrameworkCore;

namespace Api;

public class AuthorDataLoader : BatchDataLoader<Guid, Author>
{
    private ApplicationDbContext Context { get; }
    
    public AuthorDataLoader(IBatchScheduler batchScheduler, ApplicationDbContext context, DataLoaderOptions? options = null) : base(batchScheduler, options)
    {
        Context = context;
    }

    protected override async Task<IReadOnlyDictionary<Guid, Author>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
        var authors = await Context.Authors.Where(author => keys.Contains(author.Id)).ToListAsync(cancellationToken);

        return authors.ToDictionary(author => author.Id);
    }
}
