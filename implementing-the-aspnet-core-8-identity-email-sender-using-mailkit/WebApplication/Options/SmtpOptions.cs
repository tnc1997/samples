namespace WebApplication.Options;

public class SmtpOptions
{
    public const string Smtp = "Smtp";

    public required string Host { get; set; }

    public required int Port { get; set; }
}
