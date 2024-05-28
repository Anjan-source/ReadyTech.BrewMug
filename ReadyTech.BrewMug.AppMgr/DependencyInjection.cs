using Microsoft.Extensions.DependencyInjection;
using ReadyTech.BrewMug.AppMgr.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ReadyTech.BrewMug.AppMgr
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppMgrDI(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
