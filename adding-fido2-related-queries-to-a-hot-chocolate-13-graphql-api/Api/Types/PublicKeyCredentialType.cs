using Api.Data;

namespace Api.Types;

public class PublicKeyCredentialType : ObjectType<PublicKeyCredential>
{
    protected override void Configure(IObjectTypeDescriptor<PublicKeyCredential> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(credential => credential.Id).Type<NonNullType<IdType>>();

        descriptor.Name("PublicKeyCredential");
    }
}
