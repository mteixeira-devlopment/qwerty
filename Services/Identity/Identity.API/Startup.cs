using Identity.API.Configurations;
using Identity.API.Data;
using Identity.API.Data.Repositories;
using Identity.API.Entities;
using Identity.API.Handlers;
using Identity.API.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using AuthenticationService = Identity.API.Services.AuthenticationService;
using IAuthenticationService = Identity.API.Services.IAuthenticationService;

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
                "v1", new Info { Title = "Api Getproc", Version = "v1" }));
            
            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("GooglePlatform")));

            services.ConfigureIdentity(Configuration);

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<INotificationHandler, NotificationHandler>();

            services.AddTransient<IUserStore<ApplicationUser>, UserStores>();
            services.AddTransient<IRoleStore<ApplicationRole>, RoleStores>();

            services.AddTransient<IUserRepository, UserRespository>();
            services.AddTransient<IRoleRepository, RoleRepository>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    p => p.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = new ExceptionHandler().Invoke
            });

            app.UseSwagger();

            app.UseSwaggerUI(c
                => c.SwaggerEndpoint("/swagger/v1/swagger.json", "APIs Getproc"));
        
            app.UseMvc();
        }
    }
}
