using Blog.API.Providers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Blog.API.Middlewares
{
    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly TokenProviderOptions _tokenProviderOptions;

        public TokenProviderMiddleware(RequestDelegate requestDelegate, TokenProviderOptions tokenProviderOptions)
        {
            _requestDelegate = requestDelegate;
            _tokenProviderOptions = tokenProviderOptions;
        }

        public Task Invoke (HttpContext httpContext)
        {
            if (!httpContext.Request.Path.Equals("/user/login", StringComparison.Ordinal))
            {
                return _requestDelegate(httpContext);
            }
            if (!httpContext.Request.Method.Equals("POST") || !httpContext.Request.HasFormContentType)
            {
                httpContext.Response.StatusCode = 400;
                return httpContext.Response.WriteAsync("Something went back request...");
            }
            return GenerateToken(httpContext);
        }

        private async Task GenerateToken(HttpContext httpContext)
        {
            var username = httpContext.Request.Form["username"];
            var password = httpContext.Request.Form["password"];

            var identify = await GetIdentity(username, password);
        }

        private Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            //if (user == null)
            //    return NotFound();

            //if (new[] { user.Name, user.Password }.Any(x => string.IsNullOrWhiteSpace(x)))
            //    return NotFound();

            //var userFromDatabase = _blogContext.Users.SingleOrDefault(x => x.Name.ToLower() == user.Name.ToLower());
            //if (userFromDatabase == null)
            //    return NotFound();

            //if (!BCryptHelper.CheckPassword(user.Password, userFromDatabase.Password))
            //    return NotFound();

            //return _userService.Authenticate(userFromDatabase);

            return Task.FromResult(new ClaimsIdentity(new GenericIdentity(username, "Token"), new Claim[] { }));


        }




    }
}
