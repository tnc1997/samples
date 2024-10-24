namespace WebApplication.Data;

public class DevicePublicKey
{
    public required byte[] PublicKeyCredentialId { get; set; }

    public required byte[] Value { get; set; }
}
