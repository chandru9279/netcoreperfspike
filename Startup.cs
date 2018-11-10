using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace spikes
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            app.UseMiddleware<RequestLoggingMiddleware>();

            app.UseMvc(routes =>
                {
                    routes.Routes.Clear();
                    routes.Routes.Add(new Router());

                });
        }
    }

    public class Router : IRouter
    {
        public Task RouteAsync(RouteContext context)
        {
            return Task.Run(() =>
            {
                context.Handler = httpContext => context.HttpContext.Response.WriteAsync("SOME VALUE");
            });
        }

        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            return new VirtualPathData(this, "/myroute");
        }
    }
}
