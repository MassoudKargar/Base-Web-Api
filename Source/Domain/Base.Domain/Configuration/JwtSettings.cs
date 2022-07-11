namespace Base.Domain.Configuration;
public class JwtSettings
{
    public JwtSettings()
    {
        SecretKey = String.Empty;
        EncryptKey = String.Empty;
        Issuer = String.Empty;
        Audience = String.Empty;
    }
    public string SecretKey { get; set; }
    public string EncryptKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int NotBeforeMinutes { get; set; }
    public int ExpirationMinutes { get; set; }
}