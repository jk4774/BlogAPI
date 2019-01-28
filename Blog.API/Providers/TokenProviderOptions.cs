using Microsoft.IdentityModel.Tokens;
using System;

namespace Blog.API.Providers
{
    public class TokenProviderOptions
    {
        public string Path { get; set; }
        public TimeSpan Expiration { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
    }
}
