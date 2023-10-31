using Microsoft.AspNetCore.Http;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UserJourney.Core.Contracts;

namespace UserJourney.Core.Services
{
    public class HelperService : IHelperService
    {
        private IHttpContextAccessor _contextAccessor;

        public HelperService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public void AddCookie(string cookieName, string cookieValue)
        {
            // add cookie
            CookieOptions option = new CookieOptions()
            {
                Expires = DateTime.Now.AddMonths(1),
                SameSite = SameSiteMode.Strict
            };

            string encryptedCookieValue = EncryptionDecryption.GetEncrypt(cookieValue);
            _contextAccessor.HttpContext.Response.Cookies.Append(cookieName, encryptedCookieValue, option);
        }

        public void RemoveCookie(string cookieName)
        {
            _contextAccessor.HttpContext.Response.Cookies.Delete(cookieName);
        }

        public string GetCookieValue(string cookieName)
        {
            var cookie = _contextAccessor.HttpContext.Request.Cookies[cookieName];
            return cookie != null ? EncryptionDecryption.GetDecrypt(cookie) : string.Empty;
        }

    }
}
