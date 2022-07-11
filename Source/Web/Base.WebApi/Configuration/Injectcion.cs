using Autofac;

using Base.Domain;

namespace Base.WebApi.Configuration;

public static class Injectcion
{
    public static IServiceCollection RegisterWebApiServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
               .Enrich.FromLogContext()
               .ReadFrom.Configuration(new ConfigurationBuilder()
                   .AddJsonFile("appsettings.json")
                   .Build())
               .CreateLogger();
        services.Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)));
        //typeof(EntitiesAssembly).Assembly, typeof(ServicesAssembly).Assembly
        services.InitializeAutoMapper(typeof(ApplicationAssembly).Assembly, typeof(InfrastructureAssembly).Assembly, typeof(DomainAssembly).Assembly);
        services.AddCoresSetting(configuration);
        //services.AddQuartzService();
        services.UseSftp(new SftpConfig
        {
            Host = configuration["Cati_Internal:IP"],
            UserName = configuration["Cati_Internal:Username"],
            Password = configuration["Cati_Internal:Password"],
            Port = 22
        });
        services.AddMinimalMvc();
        services.AddMemoryCache();
        services.AddJwtAuthentication(configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>());
        services.AddCustomApiVersioning();
        services.AddSwagger();
        //builder.AddServices(typeof(ApplicationAssembly).Assembly, typeof(InfrastructureAssembly).Assembly, typeof(DomainAssembly).Assembly);
        return services;
    }
}
