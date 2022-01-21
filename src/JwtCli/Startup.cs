using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtCli.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Zen.Host;

namespace JwtCli
{
    public class Startup : BaseStartup
    {
        public override void ConfigureServices(IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddTransient<GenerateCommandSetting>();
        }
    }
}