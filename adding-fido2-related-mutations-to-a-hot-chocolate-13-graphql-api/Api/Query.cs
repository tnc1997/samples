using Api.Data;

namespace Api;

public class Query
{
    [GraphQLType(typeof(NonNullType<ListType<NonNullType<Types.PublicKeyCredentialType>>>))]
    public IQueryable<PublicKeyCredential> GetPublicKeyCredentials(ApplicationDbContext context)
    {
        return context.PublicKeyCredentials;
    }
}
