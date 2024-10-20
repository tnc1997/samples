namespace WebApplication.Data;

public class PublicKeyCredential
{
    public required byte[] Id { get; set; }
    
    public required byte[] PublicKey { get; set; }
    
    public required uint SignatureCounter { get; set; }
    
    public bool IsBackupEligible { get; set; }
    
    public bool IsBackedUp { get; set; }
    
    public required byte[] AttestationObject { get; set; }
    
    public required byte[] AttestationClientDataJson { get; set; }
    
    public required string AttestationFormat { get; set; }
    
    public required Guid AaGuid { get; set; }
    
    public required string UserId { get; set; }

    public ICollection<AuthenticatorTransport> AuthenticatorTransports { get; } = new List<AuthenticatorTransport>();

    public ICollection<DevicePublicKey> DevicePublicKeys { get; } = new List<DevicePublicKey>();
}
