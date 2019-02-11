﻿using System;
using Identity.API.Data;
using Identity.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Identity.API.Configurations
{
    public static class IdentityServicesConfiguration
    {
        public static void ConfigureIdentity(
            this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
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
