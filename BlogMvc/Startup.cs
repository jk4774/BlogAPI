using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BlogContext;
using BlogServices;
using BlogEntities;
using System.Reflection.Metadata;
using System.Data.Entity;

namespace BlogMvc
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
            services.AddHttpContextAccessor();            
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder => 
                builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin()));

            services.AddScoped<UserService>();
            services.AddScoped<IBlogDbContext, BlogDbContext>();
            services.AddScoped<IDbSet<BlogEntities.User>, System.Data.Entity.DbSet<BlogEntities.User>>();
            services.AddScoped<IDbSet<BlogEntities.Article>, System.Data.Entity.DbSet<BlogEntities.Article>>();
            services.AddScoped<IDbSet<BlogEntities.Comment>, System.Data.Entity.DbSet<BlogEntities.Comment>>();

            services.AddDbContext<BlogDbContext>(o => o.UseInMemoryDatabase("BlogDb"));
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(config => {
                    config.LoginPath = "/user/login";
                    config.Cookie.Name = "auth_cookie";
                });
                
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");
        
            app.UseStatusCodePagesWithRedirects("/Home/Error");
            app.UseHsts();
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => 
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"));
        }
    }
}