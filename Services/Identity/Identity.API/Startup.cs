using Identity.API.Configurations;
using Identity.API.Domain;
using Identity.API.Domain.Handlers;
using Identity.API.Domain.Services;
using Identity.API.Infrastructure.Data;
using Identity.API.Infrastructure.Data.Repositories;
using Identity.API.Infrastructure.Stores;
using Identity.API.SharedKernel.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Identity.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c => c.SwaggerDoc(
                "v1", new Info { Title = "Identity Api", Version = "v1" }));
            
            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("STF")));

            services.ConfigureIdentity(Configuration);

            services.AddScoped<INotificationHandler, NotificationHandler>();

            services.AddTransient<IUserStore<User>, UserStores>();
            services.AddTransient<IRoleStore<Role>, RoleStores>();

            services.AddTransient<IUserRepository, UserRespository>();
            services.AddTransient<IRoleRepository, RoleRepository>();

            services.AddTransient<ISignUpService, SignUpService>();

            services.ConfigureEventBus(Configuration);
            services.ConfigureRabbitMQEventBus(Configuration);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = new ExceptionHandler().Invoke
            });

            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity Api"));
    
            app.UseMvc();
        }
    }
}
