using Blog.API.Middlewares;
using Blog.API.Models;
using Blog.API.Providers;
using Blog.API.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
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
            _appSettings = Configuration.GetSection("Settings").Get<Settings>();
        }

        public IConfiguration Configuration { get; set; }
        private readonly Settings _appSettings;
        //private BlogContext blogContext;

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(r =>
            {
                r.CheckConsentNeeded = x => true;
                r.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.SecurityKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(x =>
            {
                x.Cookie = new CookieBuilder { Name = "access_token" };
                x.TicketDataFormat = new CustomJwtDataFormat(tokenValidationParameters);
            });
            services.AddDbContext<BlogContext>(o => o.UseInMemoryDatabase("BlogDb"));

            //blogContext = services.BuildServiceProvider().GetService<BlogContext>();
            //services.Configure<RazorViewEngineOptions>(x => { x.FileProviders.Add(new EntityFrameworkFileProvider() });

            //services.AddTransient(x => blogContext = x.GetService<BlogContext>());


            services.AddScoped<UserService>();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var signingCredentials = new SigningCredentials
                (new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.SecurityKey)), SecurityAlgorithms.HmacSha256Signature);

            var blogContext = app.ApplicationServices.GetService<BlogContext>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCookiePolicy();
            app.UseMiddleware<TokenProviderMiddleware>(signingCredentials, blogContext);
            app.UseMvc();
        }
    }
}
