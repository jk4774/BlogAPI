using Blog.API.Models;
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
            //var signingKey = new SymmetricSecurityKey(key);

            var validationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.SecurityKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
            };

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
            {
                x.Cookie.Expiration = TimeSpan.FromMinutes(5);
                x.LoginPath = "/user/login";
                x.LogoutPath = "/user/logout";

            });

            services.AddMvc();

            //services.AddScoped<IDataSerializer, TicketSerializer>();


            //services.AddDataProtection(x =>
            //{
            //    x.ApplicationDiscriminator = string.Format(Environment.)
            //});




            #region old
            //var appSettingsSection = Configuration.GetSection("Settings");
            //var appSettings = appSettingsSection.Get<Settings>();
            //var key = Encoding.ASCII.GetBytes(appSettings.SecurityKey);
            //services.Configure<Settings>(appSettingsSection);
            ////services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
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
