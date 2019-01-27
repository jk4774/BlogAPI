using Blog.API.Models;
using Blog.API.Providers;
using Blog.API.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace Blog.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("Settings");
            var appSettings = appSettingsSection.Get<Settings>();
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.SecurityKey)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var cookieAuthentication = new CookieAuthenticationOptions
            {
                CookieHttpOnly = true,
                CookieName = "access_cookie_token",
                TicketDataFormat = new CustomJwtDataFormat(tokenValidationParameters)
            };



            #region old
            //var appSettingsSection = Configuration.GetSection("Settings");
            //var appSettings = appSettingsSection.Get<Settings>();
            //services.Configure<Settings>(appSettingsSection);
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
            //{
            //    x.RequireHttpsMetadata = false;
            //    x.SaveToken = true;
            //    x.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.SecurityKey)),
            //        ValidateIssuer = false,
            //        ValidateAudience = false,
            //    };
            //});

            //services.AddDbContext<BlogContext>(o => o.UseInMemoryDatabase("BlogDb"));
            //services.AddScoped<UserService>();
            //services.AddMvc();
            #endregion
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
