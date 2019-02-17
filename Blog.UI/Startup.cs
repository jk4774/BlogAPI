using Blog.API.Middlewares;
using Blog.API.Models;
using Blog.API.Providers;
using Blog.API.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Blog.UI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly Settings _settings;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _settings = Configuration.GetSection("Settings").Get<Settings>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settings.SecurityKey)),
                ValidateIssuer = false,
                ValidateAudience  = false,
                ValidateLifetime = true,
                ClockSkew = System.TimeSpan.Zero
            };

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(c =>
            {
                c.Cookie = new CookieBuilder { Name = "access_token" };
                c.TicketDataFormat = new CustomJwtDataFormat(tokenValidationParameters);
            });

            services.AddDbContext<BlogContext>(x => x.UseInMemoryDatabase("BlogDB"));
            services.AddScoped<UserService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var signingCredentials = new SigningCredentials
                (new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settings.SecurityKey)), SecurityAlgorithms.HmacSha256Signature);

            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseExceptionHandler("/Home/Error");
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseMiddleware<TokenProviderMiddleware>(signingCredentials);
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
