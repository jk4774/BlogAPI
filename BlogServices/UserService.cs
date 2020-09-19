﻿using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using BlogData.Entities;
using BlogContext;
using System.Linq;

namespace BlogServices
{
    public class UserService
    {
        private const int SaltSize = 16;
        private const int HashSize = 20;        
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public virtual async Task SignIn(User user)
        {
            var expires = DateTime.UtcNow.AddMinutes(60);

            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var userIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
            var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });
            var authProperties = new AuthenticationProperties { ExpiresUtc = expires };

            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme, 
                userPrincipal,
                authProperties);

            _httpContextAccessor.HttpContext.User = userPrincipal;
        }

        public virtual async Task SingOut()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public virtual string Hash(string password)
        {
            var iterations = 10000;
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = pbkdf2.GetBytes(HashSize);

            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            var base64Hash = Convert.ToBase64String(hashBytes);

            return string.Format("$MYHASH$V1${0}${1}", iterations, base64Hash);
        }

        public virtual bool Verify(string password, string hashedPassword)
        {
            if (!hashedPassword.Contains("$MYHASH$V1$"))
                throw new NotSupportedException("The hashtype is not supported");

            var splittedHashString = hashedPassword.Replace("$MYHASH$V1$", "").Split('$');
            var iterations = int.Parse(splittedHashString[0]);
            var base64Hash = splittedHashString[1];

            var hashBytes = Convert.FromBase64String(base64Hash);

            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = pbkdf2.GetBytes(HashSize);

            for (var i = 0; i < HashSize; i++)
                if (hashBytes[i + SaltSize] != hash[i])
                    return false;
            
            return true;
        }

        public virtual bool IsEmailAvailable(IBlogDbContext blogDbContext, string emailToCheck)
        {
            return blogDbContext.Users.Any(x => x.Email.Equals(emailToCheck, StringComparison.CurrentCultureIgnoreCase));
        }

        public virtual User GetUserByEmail(IBlogDbContext blogDbContext, string email)
        {
            return blogDbContext.Users.FirstOrDefault(x => x.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}