namespace Base.Application;
public static class Injectcion
{
    public static IServiceCollection RegisterApplicationServices(
        this IServiceCollection service,
        IConfiguration configuration)
    {
        return service;
    }
}
