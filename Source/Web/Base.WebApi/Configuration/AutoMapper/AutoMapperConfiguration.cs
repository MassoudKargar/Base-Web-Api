
using System.Reflection;

namespace Base.WebApi.Configuration.AutoMapper;

public static class AutoMapperConfiguration
{
    public static void InitializeAutoMapper(this IServiceCollection services, params Assembly[] assemblies)
    {
        //With AutoMapper Instance, you need to call AddAutoMapper services and pass assemblies that contains automapper Profile class
        //services.AddAutoMapper(assembly1, assembly2, assembly3);
        //See http://docs.automapper.org/en/stable/Configuration.html
        //And https://code-maze.com/automapper-net-core/

        services.AddAutoMapper(config =>
        {
            //config.AddCustomMappingProfile();
            IEnumerable<Type> allTypes = assemblies.SelectMany(a => a.ExportedTypes);

            IEnumerable<IHaveCustomMapping> list = allTypes.Where(type => type.IsClass && !type.IsAbstract &&
                type.GetInterfaces().Contains(typeof(IHaveCustomMapping)))
                .Select(type => (IHaveCustomMapping)Activator.CreateInstance(type));

            var profile = new CustomMappingProfile(list);
            config.AddProfile(profile);
            //var configuration = new MapperConfiguration(cfg =>
            //{
            //    cfg = config
            //});
            //configuration.CompileMappings();

            //config.Advanced.BeforeSeal(configProvicer =>
            //{
            //    configProvicer.CompileMappings();
            //});
        }, assemblies);

    }

}