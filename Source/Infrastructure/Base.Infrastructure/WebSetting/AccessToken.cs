namespace Base.Infrastructure.WebSetting;

public class AccessToken
{
    public AccessToken()
    {
        Token = String.Empty;
        RefreshToken = String.Empty;
        FullName = String.Empty;
        UserName = String.Empty;
    }
    public string Token { get; set; }
    public double? ExpiresIn { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpireAt { get; set; }
    public string UserName { get; set; }
    public long? RoleId { get; set; }
    public string FullName { get; set; }
    public long UserId { get; set; }

    public AccessToken(JwtSecurityToken securityToken) : this()
    {
        Token = new JwtSecurityTokenHandler().WriteToken(securityToken);
        ExpiresIn = (securityToken.ValidTo - securityToken.IssuedAt).TotalMinutes;
    }
}
public class AccessTokenModel
{
    public AccessTokenModel()
    {
        Token = String.Empty;
    }
    public AccessTokenModel(string token) : this()
    {
        Token = token;
    }

    public string Token { get; set; }
}

