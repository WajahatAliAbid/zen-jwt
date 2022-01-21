using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Spectre.Console;
using Spectre.Console.Cli;

namespace JwtCli.Commands
{
    public class GenerateCommandSetting : CommandSettings
    {
        [CommandOption("-c|--config")]
        [Description("Configuration file containing parameters for generating JWT Token")]
        [JsonIgnore]
        public string ConfigurationFile { get; set; }
        
        [CommandOption("--secret|-s")]
        [Description("Secret key for generating JWT Token")]
        public string Secret { get; set; }
        
        [CommandOption("--issuer|-i")]
        [Description("Issuer for generating JWT Token")]
        public string Issuer { get; set; }
        
        [CommandOption("--audience|-a")]
        [Description("Audience for generating JWT Token")]
        public string Audience { get; set; }

        [CommandOption("--expiry|-e")]
        [Description("Expiry for generating JWT Token. Default is never.")]
        public DateTime? Expiry { get; set; }


        [CommandOption("--subject")]
        [Description("Subject for generating JWT Token")]
        public string Subject { get; set; }

        [CommandOption("--claim")]
        [Description("Claim for generating JWT Token (format: -c \"nameid:=Test User\")")]
        [JsonIgnore]
        public string[] ClaimArray { get; set; }

        public Dictionary<string,string> Claims { get; set; }
        
        public override ValidationResult Validate()
        {
            var exists = File.Exists(ConfigurationFile);
            if(exists)
            {
                var text = File.ReadAllText(ConfigurationFile);
                var config = JsonSerializer.Deserialize<GenerateCommandSetting>(text);
                this.Merge(config);
            }
            else
            {
                if(ClaimArray != null)
                {
                    Claims = new Dictionary<string, string>();
                    foreach(var claim in ClaimArray)
                    {
                        var parts = claim.Split(":=");
                        if(parts.Length != 2 || string.IsNullOrWhiteSpace(parts[0]) || string.IsNullOrWhiteSpace(parts[1]))
                        {
                            return ValidationResult.Error($"Invalid claim format \"{claim}\". Format: -c \"nameid:=Test User\"");
                        }
                        Claims.Add(parts[0], parts[1]);
                    }
                }
            }
            if(string.IsNullOrWhiteSpace(Secret))
            {
                return ValidationResult.Error("Secret key is required");
            }
            if(Secret.Length < 16)
            {
                return ValidationResult.Error("Secret key must be at least 16 characters long");
            }
            if(Expiry != null)
            {
                if(Expiry.Value < DateTime.Now.AddMinutes(5))
                {
                    return ValidationResult.Error("Expiry must be at least 5 minutes in the future");
                }
            }
            if(Claims != null)
            {
                foreach (var (key,value) in Claims)
                {
                    if(string.IsNullOrWhiteSpace(key))
                    {
                        return ValidationResult.Error("Claim name cannot be empty");
                    }
                    if(string.IsNullOrWhiteSpace(value))
                    {
                        return ValidationResult.Error("Claim value cannot be empty");
                    }
                }
            }

            return ValidationResult.Success();
        }

        public void Merge(GenerateCommandSetting other)
        {
            if(other.Secret != null)
            {
                this.Secret = other.Secret;
            }
            if(other.Issuer != null)
            {
                this.Issuer = other.Issuer;
            }
            if(other.Audience != null)
            {
                this.Audience = other.Audience;
            }
            if(other.Expiry != null)
            {
                this.Expiry = other.Expiry;
            }
            if(other.Subject != null)
            {
                this.Subject = other.Subject;
            }
            if(other.Claims != null)
            {
                this.Claims = other.Claims;
            }
        }
    }
}