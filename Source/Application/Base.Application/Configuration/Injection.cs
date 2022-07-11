using Base.Application.Users;

namespace Base.Application;
public static class Injectcion
{
    public static IServiceCollection RegisterApplicationServices(
        this IServiceCollection service,
        IConfiguration configuration)
    {
        //service.AddScoped<IUserInterfaces, UserServices>();
        //service.AddScoped<IJwtInterface, JwtService>();
        return service;
    }
}
