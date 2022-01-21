using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using System.IdentityModel.Tokens;
using Spectre.Console.Cli;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace JwtCli.Commands
{
    public class GenerateCommand : Command<GenerateCommandSetting>
    {
        public override int Execute([NotNull] CommandContext context, [NotNull] GenerateCommandSetting settings)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.Secret));
            var tokenHandler = new JwtSecurityTokenHandler();
            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = settings.Issuer,
                Audience = settings.Audience,
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature),
                Expires = settings.Expiry is null ? DateTime.MaxValue : settings.Expiry.Value
            };
            var claims = new List<Claim>();
            if(!string.IsNullOrWhiteSpace(settings.Subject))
            {
                claims.Add(new Claim("sub", settings.Subject));
            }
            if(settings.Claims != null)
            {
                foreach(var claim in settings.Claims)
                {
                    claims.Add(new Claim(claim.Key, claim.Value));
                }
            }

            descriptor.Subject = new ClaimsIdentity(claims);
            
            var securityToken = tokenHandler.CreateToken(descriptor);
            var token = tokenHandler.WriteToken(securityToken);

            AnsiConsole.WriteLine(token);
            return 0;
        }
    }
}