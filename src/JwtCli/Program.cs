using System;
using System.Threading.Tasks;
using Zen.SpectreConsole.Extensions;

namespace JwtCli
{
    class Program
    {
        public static async Task<int> Main(string[] args) => 
            await SpectreConsoleHost
                .WithStartup<Startup>()
                .UseConfigurator<CommandConfiguration>()
                .RunAsync(args);
    }
}
