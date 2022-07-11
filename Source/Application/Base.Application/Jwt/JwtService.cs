namespace Base.Application.Jwt;
public class JwtService : IJwtInterface, IScopedDependency
{
    private JwtSettings JwtSetting { get; }
    public JwtService(IOptions<JwtSettings> settings)
    {
        JwtSetting = settings.Value;
    }

    #region Generate Token
    private AccessToken GenerateTokens(IEnumerable<Claim> claims)
    {
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(JwtSetting.SecretKey)),
            SecurityAlgorithms.HmacSha256Signature);

        var encryptingCredentials = new EncryptingCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(JwtSetting.EncryptKey)),
            SecurityAlgorithms.Aes128KW,
            SecurityAlgorithms.Aes128CbcHmacSha256);

        var descriptor = new SecurityTokenDescriptor
        {
            TokenType = JwtSetting.Issuer,
            Issuer = JwtSetting.Issuer,
            Audience = JwtSetting.Audience,
            IssuedAt = DateTime.Now,
            NotBefore = DateTime.Now.AddMinutes(JwtSetting.NotBeforeMinutes),
            Expires = DateTime.Now.AddMinutes(JwtSetting.ExpirationMinutes),
            SigningCredentials = signingCredentials,
            EncryptingCredentials = encryptingCredentials,
            Subject = new ClaimsIdentity(claims)
        };
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        JwtSecurityTokenHandler.DefaultMapInboundClaims = true;
        JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
        var tokenHandler = new JwtSecurityTokenHandler()
        {
        };
        var securityToken = tokenHandler.CreateJwtSecurityToken(descriptor);
        tokenHandler.WriteToken(securityToken);
        return new AccessToken(securityToken);
    }

    public AccessToken Generate(Dictionary<string, string> otherCash)
    {
        var v = GenerateTokens(GetClaims(otherCash));
        return v;
    }

    public string UpdateToken(IEnumerable<Claim> claims)
    {
        var v = GenerateTokens(GetClaims(claims));
        return v.Token;
    }
    #endregion

    #region Generate Claims
    private static IEnumerable<Claim> GetClaims(Dictionary<string, string> otherCash)
    {
        //add custom claims
        List<Claim> list = new();
        if (otherCash is null)
        {
            return list;
        }
        list.AddRange(otherCash.Select(item => new Claim(item.Key, item.Value)));
        return list;
    }
    //private void GetClaims(RedisKey key, params RedisValue[] values)
    //{
    //    CashInterface.SetChashAsync(key, values);
    //}

    private static IEnumerable<Claim> GetClaims(IEnumerable<Claim> claims)
    {
        //add custom claims
        List<Claim> list = new();
        foreach (var claim in claims)
        {
            string[] non = { "nbf", "exp", "iat", "iss", "aud" };
            if (non.Contains(claim.Type))
            {
                continue;
            }
            list.Add(new Claim(claim.Type, claim.Value));
        }
        return list;
    }
    #endregion

    #region ReadToken
    public List<Claim> ReadToken(string token, params string[] claimTypes)
    {
        if (claimTypes is null)
        {
            throw new ArgumentNullException(nameof(claimTypes));
        }
        (ClaimsPrincipal claimsPrincipal, JwtSecurityToken jwtSecurityToken) = this.DecodeJwtToken(token);
        return claimTypes.Select(t => jwtSecurityToken.Claims.FirstOrDefault(w => w.Type == t)).ToList();
    }
    #endregion

    #region Decode Token
    public (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new SecurityTokenException("توکن اجباری می باشد");
        }

        var principal = new JwtSecurityTokenHandler()
            .ValidateToken(token,
                new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = JwtSetting.Issuer,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSetting.SecretKey)),
                    TokenDecryptionKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSetting.EncryptKey)),
                    ValidAudience = JwtSetting.Audience,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                },
                out var validatedToken);
        return (principal, validatedToken as JwtSecurityToken);
    }

    #endregion

}