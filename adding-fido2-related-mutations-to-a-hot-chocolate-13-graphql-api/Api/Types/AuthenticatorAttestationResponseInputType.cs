using Fido2NetLib;

namespace Api.Types;

public class AuthenticatorAttestationResponseInputType : InputObjectType<AuthenticatorAttestationRawResponse.AttestationResponse>
{
    protected override void Configure(IInputObjectTypeDescriptor<AuthenticatorAttestationRawResponse.AttestationResponse> descriptor)
    {
        descriptor.BindFieldsExplicitly();

        descriptor.Field(response => response.AttestationObject).Name("attestationObject").Type<NonNullType<StringType>>();
        descriptor.Field(response => response.ClientDataJson).Name("clientDataJSON").Type<NonNullType<StringType>>();
        descriptor.Field(response => response.Transports).Name("transports").Type<NonNullType<ListType<NonNullType<StringType>>>>();

        descriptor.Name("AuthenticatorAttestationResponseInput");
    }
}
