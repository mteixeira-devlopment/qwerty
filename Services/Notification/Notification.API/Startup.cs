using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.API.Application.IntegrationEvents.EventHandlers;
using Notification.API.Configurations;
using Notification.API.Domain;
using Notification.API.Infrastructure.AutofacModules;
using Notification.API.Infrastructure.Data;
using Notification.API.Infrastructure.Data.Repositories;
using SharedKernel.Configurations;
using SharedKernel.Handlers;
using Swashbuckle.AspNetCore.Swagger;

namespace Notification.API
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
                "v1", new Info { Title = "Notification Api", Version = "v1" }));

            services.AddDbContext<NotificationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("STF")));

            services.AddTransient<INotificationRepository, NotificationRepository>();

            services.AddCors(
                options => options.AddPolicy("*", policy
                    => policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()));

            services.AddSignalR();

            services.ConfigureRabbitMQEventBus(Configuration);
            services.ConfigureEventBus(Configuration);

            services.AddTransient<AccountInvalidatedIntegrationEventHandler>();
            services.AddTransient<AccountCreatedIntegrationEventHandler>();

            var container = new ContainerBuilder();
            container.Populate(services);

            container.RegisterModule(new MediatorModule());

            return new AutofacServiceProvider(container.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = new ExceptionHandler().Invoke
            });

            app.ConfigureEventBusSubscribers();

            app.UseCors("*");

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notification Api"));

            app.UseSignalR();

            app.UseMvc();
        }
    }
}
