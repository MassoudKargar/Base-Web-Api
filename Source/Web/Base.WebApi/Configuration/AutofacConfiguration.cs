
using Autofac;
using Base.Domain;
namespace Base.Infrastructure.Configuration;
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