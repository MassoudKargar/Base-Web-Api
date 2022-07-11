namespace Base.Infrastructure;
public static class Injectcion
{
    public static IServiceCollection RegisterInfrastructerServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services;
    }
}