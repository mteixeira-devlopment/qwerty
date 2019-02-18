using Identity.API.Data;
using Identity.API.Models;
using Identity.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Identity.API.Configurations
{
    public static class ServiceConfiguration
    {
        public static void ConfigureIdentity(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<IdentityOptions>(identityOptions =>
            {
                identityOptions.Password.RequireDigit = false;
                identityOptions.Password.RequireLowercase = false;
                identityOptions.Password.RequireNonAlphanumeric = false;
                identityOptions.Password.RequireUppercase = false;
                identityOptions.Password.RequiredLength = 6;
                identityOptions.Password.RequiredUniqueChars = 0;
            });
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddDefaultTokenProviders();

            // Configuração de login
            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            var tokenConfigurations = new TokenConfigurations();

            new ConfigureFromConfigurationOptions<TokenConfigurations>(
                    configuration.GetSection("TokenConfigurations"))
                .Configure(tokenConfigurations);

            services.AddSingleton(tokenConfigurations);

            // Configura a autenticação
            services.AddAuthentication(authenticationOptions =>
            {
                authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });
                
            services.AddAuthentication().AddJwtBearer(bearerOptions => 
            {
                // Parâmetros para a validação dos tokens
                var validationParams = bearerOptions.TokenValidationParameters;

                // Obtém a chave de assinaturas
                validationParams.IssuerSigningKey = signingConfigurations.Key;

                validationParams.ValidAudience = tokenConfigurations.Audience;
                validationParams.ValidIssuer = tokenConfigurations.Issuer;

                // Habilita a verificação da assinatura de um token
                validationParams.ValidateIssuerSigningKey = true;

                // Habilita a verificação do tempo de validação de um token
                validationParams.ValidateLifetime = true;
            });

            // Habilita controle do acesso à API pelo token
            services.AddAuthorization(
                auth => auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build()));
        }
    }
}
