namespace Base.WebApi.Configuration;

public static class WebApiConfiguration
{
    public static void AddQuartzService(this IServiceCollection services)
    {
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        //services.AddTransient<JobVoiceDateCreatorLight>();
        //services.AddTransient<JobServiceGetCallDuration>();
        // Add the required Quartz.NET services
        services.AddQuartz(q =>
        {
            // Use a Scoped container to create jobs. I'll touch on this later
            q.UseMicrosoftDependencyInjectionScopedJobFactory();
            q.UseJobFactory<JobFactory>();
            q.SchedulerId = "quartzWorker.serializerWorker.typeWorker";
            q.SchedulerName = "binaryWorker";
            //q.UseThreadPool<ThreadPool>();
            q.UseDefaultThreadPool(100);
            //q.ScheduleJob<JobVoiceDateCreatorLight>(trigger =>
            //    {
            //        trigger.StartNow();
            //        trigger.StartAt(DateTimeOffset.UtcNow);
            //        trigger.WithIdentity("VoiceDate", "CopyVoiceeDate");
            //    },
            //    job => job.WithIdentity("VoiceDate", "CopyVoiceeDate"));
            //q.ScheduleJob<JobServiceGetCallDuration>(trigger =>
            //{
            //    trigger.StartNow();
            //    trigger.StartAt(DateTimeOffset.UtcNow);
            //    trigger.WithIdentity("CallDurationWorker", "CopyVoiceDurationWorker");
            //},
            //    job => job.WithIdentity("CallDurationWorker", "CopyVoiceWorker"));
        });
        services.AddQuartzHostedService(options =>
        {
            options.AwaitApplicationStarted = true;
            options.WaitForJobsToComplete = true;
        });
    }
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
    public static void AddMinimalMvc(this IServiceCollection services)
    {

        //https://github.com/aspnet/AspNetCore/blob/0303c9e90b5b48b309a78c2ec9911db1812e6bf3/src/Mvc/Mvc/src/MvcServiceCollectionExtensions.cs
        services.AddControllers(options =>
        {
            options.Filters.Add(new AuthorizeFilter());
        }).AddNewtonsoftJson();
        services.AddSwaggerGenNewtonsoftSupport();
    }
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
