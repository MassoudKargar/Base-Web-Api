namespace Base.WebApi.Configuration.Filters;

public class ReadableBodyStreamAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context) => context.HttpContext.Request.EnableBuffering();
}
