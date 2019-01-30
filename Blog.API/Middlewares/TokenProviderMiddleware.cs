using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace WebApplication2
{
    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly TokenProviderOptions _tokenProviderOptions;

        public TokenProviderMiddleware(RequestDelegate requestDelegate, IOptions<TokenProviderOptions> tokenProviderOptions)
        {
            _requestDelegate = requestDelegate;
            _tokenProviderOptions = tokenProviderOptions.Value;
        }

        public Task Invoke (HttpContext httpContext)
        {
            if (!httpContext.Request.Path.Equals(_tokenProviderOptions.Path, StringComparison.Ordinal))
            {
                return _requestDelegate(httpContext);
            }

            if (!httpContext.Request.Method.Equals("POST") || !httpContext.Request.HasFormContentType)
            {
                httpContext.Response.StatusCode = 400;
                return httpContext.Response.WriteAsync("Bad request...");
            }
            return GenerateToken(httpContext);
        }

        private async Task GenerateToken(HttpContext httpContext)
        {
            var username = httpContext.Request.Form["username"];
            var password = httpContext.Request.Form["password"];

            var identify = await GetIdentity(username, password);
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
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToUniversalTime().ToString(), ClaimValueTypes.Integer64)
            };

            var jwt = new JwtSecurityToken
            (
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.Add(_tokenProviderOptions.Expiration),
                signingCredentials: _tokenProviderOptions.SigningCredentials
            );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);


            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsync
            (
                JsonConvert.SerializeObject
                (
                    new { access_token = encodedJwt, expires_in = (int)_tokenProviderOptions.Expiration.TotalSeconds }, 
                    new JsonSerializerSettings { Formatting = Formatting.Indented }
                )
            );
        }

        private Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            //test
            if (username == "r" && password == "r")
            {
                return Task.FromResult(new ClaimsIdentity(new GenericIdentity(username, "Token"), new Claim[] { }));
            }
            return Task.FromResult<ClaimsIdentity>(null);
        }
    }
}
