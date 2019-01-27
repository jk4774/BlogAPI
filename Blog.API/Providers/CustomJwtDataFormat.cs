using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Blog.API.Providers
{
    public class CustomJwtDataFormat : ISecureDataFormat<AuthenticationTicket>
    {
        private readonly TokenValidationParameters _tokenValidationParameters;

        public CustomJwtDataFormat(TokenValidationParameters tokenValidationParameters)
        {
            _tokenValidationParameters = tokenValidationParameters;
        }

        public AuthenticationTicket Unprotect(string protectedText)
        {
            return Unprotect(protectedText, null);
        }

        public AuthenticationTicket Unprotect(string protectedText, string purpose)
        {
            var handler = new JwtSecurityTokenHandler();
            ClaimsPrincipal claimsPrincipal;

            try
            {
                claimsPrincipal = handler.ValidateToken(protectedText, _tokenValidationParameters, out SecurityToken securityToken);
                var validJwt = (JwtSecurityToken)securityToken;
                if (validJwt == null)
                {
                    throw new Exception("");
                }
                if (!validJwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.Ordinal))
                {
                    throw new Exception("");
                }
            }
            catch
            {
                throw new Exception("");
            }

            return new AuthenticationTicket(claimsPrincipal, new AuthenticationProperties(), "Cookie");
        }

        public string Protect(AuthenticationTicket data)
        {
            throw new NotImplementedException();
        }

        public string Protect(AuthenticationTicket data, string purpose)
        {
            throw new NotImplementedException();
        }
    }
}
