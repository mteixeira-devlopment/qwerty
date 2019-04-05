using Account.API.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notification.API.Configurations;
using Notification.API.Domain;
using Notification.API.Hubs;
using Notification.API.Infrastructure.Data;
using Notification.API.Infrastructure.Data.Repositories;
using Notification.API.SharedKernel.Handlers;
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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

            services.ConfigureBus();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = new ExceptionHandler().Invoke
            });

            app.UseCors("*");

            app.UseSignalR(routes => { routes.MapHub<SignUpHub>("/jesus"); });

            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notification Api"));

            app.UseMvc();
        }
    }
}
