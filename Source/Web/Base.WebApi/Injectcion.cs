namespace Base.WebApi;
public static class Injectcion
{
    public static IServiceCollection RegisterWebApiServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }
}
