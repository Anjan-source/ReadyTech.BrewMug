using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ReadyTech.BrewMug.AppMgr.common;
using ReadyTech.BrewMug.AppMgr.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using configurationManager = Microsoft.Extensions.Configuration.ConfigurationManager;

namespace ReadyTech.BrewMug.AppMgr
{
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
