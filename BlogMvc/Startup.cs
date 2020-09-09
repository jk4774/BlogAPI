using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BlogContext;
using BlogServices;
using BlogData.Entities;

namespace BlogMvc
{
    //class ContractServiceFactory
    //{
    //    private readonly IServiceProvider _serviceProvider;

    //    public ContractServiceFactory(IServiceProvider serviceProvider)
    //    {
    //        _serviceProvider = serviceProvider;
    //    }

    //    public IDbSetExtended<T> GetContractService(string standard)
    //    {
    //        switch (standard)
    //        {
    //            case "user":
    //                return _serviceProvider.GetRequiredService<IDbSetExtended<User>>();
    //            case "article":
    //                return _serviceProvider.GetRequiredService<IDbSetExtended<Article>>();
    //            case "comment":
    //                return _serviceProvider.GetRequiredService<IDbSetExtended<Comment>>();
    //        }
    //    }
    //}

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