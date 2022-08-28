namespace SnackerBox.Configs;

public class JwtConfig
{
    public string Secret { get; set; }
    public string ValidIssuer { get; set; }
    public string ValidAudience { get; set; }
    public int ExpirationMinutes { get; set; }
    public int RefreshTokenValidityInDays { get; set; }
}