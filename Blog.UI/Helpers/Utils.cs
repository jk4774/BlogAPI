using Microsoft.AspNetCore.Http;
using System;

namespace Blog.UI.Helpers
{
    public static class Utils
    {
        public static void DeleteCookie(HttpContext httpContext, string cookieName = "access_token")
        {
            if (httpContext.Request.Cookies[cookieName] != null)
            {
                try
                {
                    httpContext.Response.Cookies.Delete(cookieName);
                }
                catch
                {
                    throw new Exception(string.Format("Something went wrong. Cannot delete cookie with name: {0}", cookieName));
                }
            }
        }
    }
}
