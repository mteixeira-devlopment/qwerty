using System;
using System.Threading.Tasks;
using Gateway.API.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Identity.API.Configurations
{
    public static class ServiceConfiguration
    {
        public static void ConfigureProvider(
            this IServiceCollection services, IConfiguration configuration)
        {
            var tokenConfigurations = new TokenConfigurations();

            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                    configuration.GetSection("TokenConfigurations"))
                .Configure(tokenConfigurations);

            // Chave do provider
            var authenticationProviderKey = "qw3rty#provider";

            // Configura a autenticação
            services.AddAuthentication(authenticationOptions =>
            {
                authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });

            services.AddAuthentication()
                .AddJwtBearer(authenticationProviderKey, bearerOptions =>
                {
                    bearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        // Obtém a chave de assinaturas
                        IssuerSigningKey = tokenConfigurations.SigningCredentialsSymmetricSecurityKey,

                        ValidAudience = tokenConfigurations.Audience,
                        ValidIssuer = tokenConfigurations.Issuer,

                        // Habilita a verificação da assinatura de um token
                        ValidateIssuerSigningKey = false,

                        // Habilita a verificação do tempo de validação de um token
                        ValidateLifetime = true
                    };

                    bearerOptions.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                            return Task.CompletedTask;
                        }
                    };
                });
        }
    }
}
