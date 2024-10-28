using Api.Data;
using Api.Types;

namespace Api;

public class Query
{
    [GraphQLType(typeof(NonNullType<ListType<NonNullType<PublicKeyCredentialType>>>))]
    public IQueryable<PublicKeyCredential> GetPublicKeyCredentials(ApplicationDbContext context)
    {
        return context.PublicKeyCredentials;
    }
}
