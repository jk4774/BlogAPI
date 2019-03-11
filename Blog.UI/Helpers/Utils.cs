using Microsoft.AspNetCore.Http;
using System;

namespace Blog.UI.Helpers
{
    public static class Utils
    {
        public static void DeleteCookie(HttpRequest httpRequest, HttpResponse httpResponse, string cookieName = "access_token")
        {
            if (httpRequest.Cookies[cookieName] != null)
            {
                try
                {
                    httpResponse.Cookies.Delete(cookieName);
                }
                catch
                {
                    throw new Exception(string.Format("Something went wrong. Cannot delete cookie with name: {0}", cookieName));
                }
            }
        }
    }
}
