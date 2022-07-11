namespace Base.WebApi.Configuration.Filters;

public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private readonly int[] _personRoles;
    public CustomAuthorizeAttribute(params int[] personRoles)
    {
        _personRoles = personRoles;
    }
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        string roleId = context.HttpContext.User.Claims.FirstOrDefault(f => f.Type == UserClaimName.RoleId)?.Value;
        if (roleId == null)
        {
            throw new SecurityTokenExpiredException("عدم دسترسی");
        }
        if (context.HttpContext.User.Claims.FirstOrDefault(f => f.Type == UserClaimName.PersonRoleId)?.Value is null)
        {
            throw new SecurityTokenExpiredException("عدم دسترسی");
        }
        if (_personRoles.All(p => p.ToString() != roleId))
        {
            throw new SecurityTokenExpiredException("عدم دسترسی");
        }
    }
}
