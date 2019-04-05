using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Gateway.API.Configurations;
using Gateway.API.Handlers;
using Ocelot.Configuration.Creator;

namespace Gateway.API
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
            services.ConfigureProvider(Configuration);

            services.AddCors(
                options => options.AddPolicy("*", policy
                    => policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()));

            services.AddSignalR();

            services.ConfigureBus();

            services.AddOcelot(Configuration);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = new ExceptionHandler().Invoke
            });

            var configuration = new OcelotPipelineConfiguration
            {
                AuthorisationMiddleware = async (ctx, next) =>
                {
                    var user = ctx.HttpContext.User;

                    var userIdentityHeader = new AddHeader("userIdentity", "");
                    ctx.DownstreamReRoute.AddHeadersToUpstream.Add(userIdentityHeader);

                    await next.Invoke();
                }
            };

            app.UseCors("*");

            app.UseAuthentication();

            app.UseWebSockets();
            app.UseOcelot(configuration).Wait();

            app.UseMvc();
        }
    }
}
