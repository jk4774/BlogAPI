using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            APIStartup = new API.Startup(configuration);
        }

        public IConfiguration Configuration { get; set; }
        public API.Startup APIStartup { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            APIStartup.ConfigureServices(services);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            APIStartup.Configure(app, env);
        }
    }
}
