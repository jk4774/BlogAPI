using Blog.API.Models;
using Blog.API.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
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
            var key = Encoding.ASCII.GetBytes(appSettings.SecurityKey);

            services.Configure<Settings>(appSettingsSection);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSettings.SecurityKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });
            #region old 
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddJwtBearer(x =>
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

            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(r => 
            //{

            //    r.Cookie.Expiration = System.TimeSpan.FromMinutes(10);


            //});
            #endregion
            services.AddDbContext<BlogContext>(opt => opt.UseInMemoryDatabase("BlogDb"));
            services.AddScoped<UserService>();
            services.AddMvc();
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
