using Blog.API.Models;
using DevOne.Security.Cryptography.BCrypt;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Blog.API.Middlewares
{
    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly SigningCredentials _signingCredentials;
        private readonly BlogContext _blogContext;

        public TokenProviderMiddleware(RequestDelegate requestDelegate, SigningCredentials signingCredentials, BlogContext blogContext)
        {
            _requestDelegate = requestDelegate;
            _signingCredentials = signingCredentials;
            _blogContext = blogContext;
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

            var identify = await GetIdentity(username, password, _blogContext);
            if (identify == null)
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.WriteAsync("Invalid username/password");
                return;
            }

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
            };

            var jwt = new JwtSecurityToken
            (
                claims: claims,  
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(5),
                signingCredentials: _signingCredentials
            );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            httpContext.Response.ContentType = "application/json";

            var json = JsonConvert.SerializeObject 
            ( 
                new { access_token = encodedJwt, expires_in = (int)TimeSpan.FromMinutes(5).TotalSeconds }, 
                new JsonSerializerSettings { Formatting = Formatting.Indented }
            );
            await httpContext.Response.WriteAsync(json);
        }

        private Task<ClaimsIdentity> GetIdentity(string username, string password, BlogContext blogContext)
        {
            if (new[] { username, password }.Any(x => string.IsNullOrWhiteSpace(x)))
                return Task.FromResult<ClaimsIdentity>(null);

            var userFromDb = blogContext.Users.SingleOrDefault(x => x.Name.ToLower() == username.ToLower());

            if (userFromDb == null)
                return Task.FromResult<ClaimsIdentity>(null);

            if (!BCryptHelper.CheckPassword(password, userFromDb.Password))
                return Task.FromResult<ClaimsIdentity>(null);

            return Task.FromResult(new ClaimsIdentity(new GenericIdentity(username, "Token"), new Claim[] { }));
        }
    }
}
