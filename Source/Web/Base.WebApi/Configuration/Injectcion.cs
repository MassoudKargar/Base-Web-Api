namespace Base.WebApi.Configuration;

/// <summary>
/// برای مدریت وابستگی ها
/// </summary>
public static class Injectcion
{
    /// <summary>
    /// برای مدریت وابستگی ها
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
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
        services.InitializeAutoMapper(typeof(ApplicationAssembly).Assembly, typeof(InfrastructureAssembly).Assembly, typeof(DomainAssembly).Assembly);
        services.AddCoresSetting(configuration);
        services.AddMinimalMvc();
        services.AddMemoryCache();
        services.AddJwtAuthentication(configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>());
        services.AddCustomApiVersioning();
        services.AddSwagger();
        return services;
    }

    /// <summary>
    /// محدود کردن دسترسی ها برای اتصال به برنامه
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddCoresSetting(this IServiceCollection services, IConfiguration configuration)
    {
        // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
        var list = configuration["App:CorsOriginsFinaly"]
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(o => o.RemovePostFix("/"))
            .ToArray();
        services.AddCors();
        //services.AddCors(
        //    options => options.AddPolicy(configuration["App:CorsOriginsName"],
        //        builder => builder
        //            .WithOrigins(list)
        //            .AllowAnyHeader()
        //            .AllowAnyMethod()
        //            .AllowCredentials()));
    }

    /// <summary>
    /// مدیریت تنظیمات کنترلر ها
    /// </summary>
    /// <param name="services"></param>
    public static void AddMinimalMvc(this IServiceCollection services)
    {

        //https://github.com/aspnet/AspNetCore/blob/0303c9e90b5b48b309a78c2ec9911db1812e6bf3/src/Mvc/Mvc/src/MvcServiceCollectionExtensions.cs
        services.AddControllers(options =>
        {
            options.Filters.Add(new AuthorizeFilter());
        }).AddNewtonsoftJson();
        services.AddSwaggerGenNewtonsoftSupport();
    }

    /// <summary>
    /// مدریت تنظیمات توکن 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="jwtSettings"></param>
    public static void AddJwtAuthentication(this IServiceCollection services, JwtSettings jwtSettings)
    {

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            var secretKey = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);
            var encryptionKey = Encoding.UTF8.GetBytes(jwtSettings.EncryptKey);
            TokenValidationParameters validationParameters = new()
            {
                ClockSkew = TimeSpan.Zero, // default: 5 min
                RequireSignedTokens = true,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ValidateAudience = true, //default : false
                ValidAudience = jwtSettings.Audience,

                ValidateIssuer = true, //default : false
                ValidIssuer = jwtSettings.Issuer,

                TokenDecryptionKey = new SymmetricSecurityKey(encryptionKey)
            };

            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = validationParameters;
            #region Events
            //options.Events = new JwtBearerEvents
            //{
            //    OnMessageReceived = QueryStringTokenResolver
            //};
            #endregion
        });
    }

    /// <summary>
    /// ApiVersioning مدریت تنظیمات 
    /// </summary>
    /// <param name="services"></param>
    public static void AddCustomApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true; //default => false;
            options.DefaultApiVersion = new ApiVersion(1, 0); //v1.0 == v1
            options.ReportApiVersions = true;
        });
    }
}

/// <summary>
/// AutoMapper تنظیمات 
/// </summary>
public static class AutoMapperConfiguration
{
    /// <summary>
    /// AutoMapper تنظیمات 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assemblies"></param>
    public static void InitializeAutoMapper(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddAutoMapper(config =>
        {
            config.AddProfile(
                new CustomMappingProfile(
                    assemblies
                    .SelectMany(a => a.ExportedTypes)
                    .Where(type =>
                        type.IsClass &&
                        !type.IsAbstract &&
                        type.GetInterfaces().Contains(typeof(IHaveCustomMapping)))
                .Select(type => (IHaveCustomMapping?)Activator.CreateInstance(type))));
        }, assemblies);
    }
}
public class CustomMappingProfile : Profile
{
    public CustomMappingProfile(IEnumerable<IHaveCustomMapping> haveCustomMappings)
    {
        foreach (var item in haveCustomMappings)
            item.CreateMappings(this);
    }
}
public static class AutofacConfiguration
{
    public static void AddServices(this ContainerBuilder containerBuilder)
    {
        var application = typeof(ApplicationAssembly).Assembly;
        var infrastructure = typeof(InfrastructureAssembly).Assembly;
        var domain = typeof(DomainAssembly).Assembly;
        containerBuilder.RegisterAssemblyTypes(application, infrastructure, domain)
            .AssignableTo<IScopedDependency>()
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();

        containerBuilder.RegisterAssemblyTypes(application, infrastructure, domain)
            .AssignableTo<ITransientDependency>()
            .AsImplementedInterfaces()
            .InstancePerDependency();

        containerBuilder.RegisterAssemblyTypes(application, infrastructure, domain)
            .AssignableTo<ISingletonDependency>()
            .AsImplementedInterfaces()
            .SingleInstance();
    }
}