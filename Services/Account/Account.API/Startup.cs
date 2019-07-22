using Account.API.Application.IntegrationEvents.EventHandlers;
using Account.API.Configurations;
using Account.API.Domain;
using Account.API.Domain.Commands.CreateAccount;
using Account.API.Infrastructure.Data;
using Account.API.Infrastructure.Data.Repositories;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceSeed.Configurations;
using ServiceSeed.Handlers;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;

namespace Account.API
{
    public sealed class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c => c.SwaggerDoc(
                "v1", new Info { Title = "Account Api", Version = "v1" }));

            services.AddDbContext<AccountContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("STF")));

            services.AddScoped<INotificationHandler, NotificationHandler>();

            services.AddTransient<IAccountRepository, AccountRepository>();
            
            services.ConfigureRabbitMQEventBus(Configuration);
            services.ConfigureEventBus(Configuration);

            services.AddTransient<UserValidatedIntegrationEventHandler>();
            services.AddTransient<DepositCreatedIntegrationEventHandler>();

            services.AddMediatR(typeof(CreateAccountCommandHandler).GetTypeInfo().Assembly);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = new ExceptionHandler().Invoke
            });

            app.ConfigureEventBusSubscribers();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Account Api"));

            app.UseMvc();
        }
    }
}
