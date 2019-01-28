//using Blog.API.Providers;
//using Microsoft.AspNetCore.Http;
//using System;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace Blog.API.Middlewares
//{
//    public class TokenProviderMiddleware
//    {
//        private readonly RequestDelegate _requestDelegate;
//        private readonly TokenProviderOptions _tokenProviderOptions;

//        public TokenProviderMiddleware(RequestDelegate requestDelegate, TokenProviderOptions tokenProviderOptions)
//        {
//            _requestDelegate = requestDelegate;
//            _tokenProviderOptions = tokenProviderOptions;
//        }

//        public Task Invoke(HttpContext httpContext)
//        {
//            if (!httpContext.Request.Method.Equals("POST") || !httpContext.Request.HasFormContentType)
//            {
//                httpContext.Response.StatusCode = 400;
//                return httpContext.Response.WriteAsync("Bad req");
//            }
//            return new Task(() => { });
//        }

//        private async Task GenerateToken(HttpContext httpContext)
//        {
//            var name = httpContext.Request.Form["username"];
//            var password = httpContext.Request.Form["password"];
//            var identify = await GetIdentify(name, password);

//            if (identify == null)
//            {
//                httpContext.Response.StatusCode = 404;
//                await httpContext.Response.WriteAsync("Invalid name or password");
//                return;
//            }

//            var claims = new Claim[]
//            {
//                new Claim(ClaimTypes.Name, Guid.NewGuid().ToString())
//            };
            
//            //var tokenHandler = new JwtSecurityTokenHandler();
//            //var tokenDescriptor = new SecurityTokenDescriptor
//            //{
//            //    Subject = new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, user.Id.ToString()) }),
//            //    Expires = DateTime.Now.AddHours(1),
//            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_settings.SecurityKey)), SecurityAlgorithms.HmacSha256Signature)
//            //};
//            //var securityToken = tokenHandler.CreateToken(tokenDescriptor);
//            //return new User { Id = user.Id, Name = user.Name, Password = "", Email = user.Email, Token = tokenHandler.WriteToken(securityToken) };


//        }
//    }
//}
