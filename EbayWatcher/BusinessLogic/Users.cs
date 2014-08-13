using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EbayWatcher.BusinessLogic
{
    public class Users
    {
        public static string GetCurrentUsername()
        {
            return HttpContext.Current.Session["UserId"].ToStringOrDefault();
        }
        public static object GetCurrentUserToken()
        {
            return HttpContext.Current.Session["Token"].ToStringOrDefault();
        }
        public static bool IsLoggedIn()
        {
            return GetCurrentUsername() != null && GetCurrentUserToken() != null;
        }
    }
}