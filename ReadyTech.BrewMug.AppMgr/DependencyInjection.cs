
namespace ReadyTech.BrewMug.AppMgr
{
    using Microsoft.Extensions.DependencyInjection;
    using ReadyTech.BrewMug.AppMgr.common;
    using System.Reflection;
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppMgrDI(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddSingleton<ExternalServices>();
            return services;
        }
    }
}
