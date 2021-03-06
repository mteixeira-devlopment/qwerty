﻿using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Deposit.API.Application.IntegrationEvents.EventHandlers;
using Deposit.API.Configurations;
using Deposit.API.Domain;
using Deposit.API.Infrastructure.AutofacModules;
using Deposit.API.Infrastructure.Data;
using Deposit.API.Infrastructure.Data.ExternalRepositories;
using Deposit.API.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using ServiceSeed.Configurations;
using ServiceSeed.Handlers;
using Swashbuckle.AspNetCore.Swagger;

namespace Deposit.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public IConfiguration Configuration { get; }
        
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c => c.SwaggerDoc(
                "v1", new Info { Title = "Deposit Api", Version = "v1" }));

            services.AddDbContext<DepositContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("STF")));

            services.AddScoped<INotificationHandler, NotificationHandler>();

            services.AddScoped<IPayExternalRepository, PayExternalRepository>();
            services.AddScoped<IDepositRepository, DepositRepository>();

            services.ConfigureRabbitMQEventBus(Configuration);
            services.ConfigureEventBus(Configuration);

            services.AddTransient<IncreaseBalanceInvalidatedIntegrationEventHandler>();

            var container = new ContainerBuilder();
            container.Populate(services);

            container.RegisterModule(new MediatorModule());

            return new AutofacServiceProvider(container.Build());
        }
       
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = new ExceptionHandler().Invoke
            });

            app.ConfigureEventBusSubscribers();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Deposit Api"));

            app.UseMvc();
        }
    }
}
