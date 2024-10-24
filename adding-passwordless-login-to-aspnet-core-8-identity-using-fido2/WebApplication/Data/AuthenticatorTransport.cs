namespace WebApplication.Data;

public class AuthenticatorTransport
{
    public required byte[] PublicKeyCredentialId { get; set; }

    public required Fido2NetLib.Objects.AuthenticatorTransport Value { get; set; }
}
