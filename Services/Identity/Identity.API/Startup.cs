using Identity.API.Configurations;
using Identity.API.Data;
using Identity.API.Data.Repositories;
using Identity.API.Domain;
using Identity.API.SharedKernel.Handlers;
using Identity.API.Stores;
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
                options.UseSqlServer(Configuration.GetConnectionString("GooglePlatform")));

            services.ConfigureIdentity(Configuration);

            services.AddScoped<IDomainNotificationHandler, DomainNotificationHandler>();

            services.AddTransient<IUserStore<User>, UserStores>();
            services.AddTransient<IRoleStore<Role>, RoleStores>();

            services.AddTransient<IUserRepository, UserRespository>();
            services.AddTransient<IRoleRepository, RoleRepository>();

            services.ConfigureBus();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            else app.UseHsts();

            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = new ExceptionHandler().Invoke
            });

            app.UseSwagger();

            app.UseSwaggerUI(c
                => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity Api"));
    
            app.UseMvc();
        }
    }
}
