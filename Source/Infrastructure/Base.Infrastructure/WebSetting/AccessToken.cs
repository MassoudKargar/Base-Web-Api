namespace Base.Infrastructure.WebSetting;

public class AccessToken
{
    public AccessToken()
    {
        Token = String.Empty;
        FullName = String.Empty;
    }
    public string Token { get; set; }
    public long? RoleId { get; set; }
    public string FullName { get; set; }

    public AccessToken(JwtSecurityToken securityToken) : this()
    {
        Token = new JwtSecurityTokenHandler().WriteToken(securityToken);
        //ExpiresIn = (securityToken.ValidTo - securityToken.IssuedAt).TotalMinutes;
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

