using NetCore.AutoRegisterDi;
using System.Reflection;

namespace TaskListManager.API.Extensions
{
    public static class LibraryDependencyInjection
    {
        public static IServiceCollection RegisterApplicationService<T>(this IServiceCollection services)
        {
            var assemblyToScan = Assembly.GetAssembly(typeof(T));
            services.RegisterAssemblyPublicNonGenericClasses(assemblyToScan)
                .Where(x => x.Name.EndsWith("Service"))
                .AsPublicImplementedInterfaces();
            return services;
        }
        /*public static IServiceCollection RegisterApplicationRepository<T>(this IServiceCollection services)
        {
            var assemblyToScan = Assembly.GetAssembly (typeof(T));
            services.RegisterAssemblyPublicNonGenericClasses(assemblyToScan)
                .Where(x => x.Name.EndsWith("Repository"))
                .AsPublicImplementedInterfaces();
            return services;
        }
        public static IServiceCollection RegisterAdapters<T> (this IServiceCollection services)
        {
            var assemblyToScan = Assembly.GetAssembly(typeof (T));
            services.RegisterAssemblyPublicNonGenericClasses(assemblyToScan)
                .Where(x => x.Name.EndsWith("Adapter"))
                .AsPublicImplementedInterfaces();
            return services;
        }*/
    }
}
