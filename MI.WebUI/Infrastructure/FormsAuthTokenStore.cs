using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MI.Web.Infrastructure
{
    public class FormsAuthTokenStore : ITokenHandler

    {
        public void SetClientAccess(string token)
        {
            FormsAuthentication.SetAuthCookie(token, true);
        }

        public void RemoveClientAccess()
        {
            FormsAuthentication.SignOut();
        }

        public string GetToken()
        {
          
           // bool b = HttpContext.Current.Request.IsAuthenticated;
            
            //this is not working
            //var cookieName = FormsAuthentication.FormsCookieName;
            //var cookieValue = HttpContext.Current.Response.Cookies[cookieName].Value;
            //return cookieValue == null ? "" : FormsAuthentication.Decrypt(cookieValue).Name;

            return HttpContext.Current.User.Identity.Name;
        }

      
    }

    public interface ITokenHandler
    {
        string GetToken();
        void RemoveClientAccess();
        void SetClientAccess(string token);
    }
}//ns