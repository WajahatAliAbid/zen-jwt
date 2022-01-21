using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtCli.Commands;
using Spectre.Console.Cli;
using Zen.SpectreConsole.Extensions;

namespace JwtCli
{
    public class CommandConfiguration : ISpectreConfiguration
    {
        public void ConfigureCommandApp(in IConfigurator configurator)
        {
            configurator.PropagateExceptions();
            configurator.SetApplicationName("jwt");
            configurator.CaseSensitivity(CaseSensitivity.None);
            configurator.AddCommand<GenerateCommand>("generate")
                .WithDescription("Generates a JWT token based on properties set");   
        }
    }
}