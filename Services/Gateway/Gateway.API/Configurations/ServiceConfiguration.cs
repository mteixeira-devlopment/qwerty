using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Gateway.API.Configurations
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
                        IssuerSigningKey = tokenConfigurations.SigningCredentialsSymmetricKey,

                        ValidAudience = tokenConfigurations.Audience,
                        ValidIssuer = tokenConfigurations.Issuer,

                        // Habilita a verificação da assinatura de um token
                        ValidateIssuerSigningKey = false,

                        // Habilita a verificação do tempo de validação de um token
                        ValidateLifetime = true
                    };
                });

            services.AddAuthorization(authorizationOptions =>
            {
                authorizationOptions.AddPolicy("Bearer", policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser()
                        .Build();
                });
            });
        }
    }
}
