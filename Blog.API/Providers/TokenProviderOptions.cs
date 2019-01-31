using Microsoft.IdentityModel.Tokens;
using System;

namespace Blog.API.Providers
{
    public class TokenProviderOptions
    {
        public string Path { get; set; } = "/token";
        public TimeSpan Expiration { get; set; } = TimeSpan.FromMinutes(5);
        public SigningCredentials signingCredentials { get; set; }
    }
}
