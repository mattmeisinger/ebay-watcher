using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EbayWatcher.Entities;

namespace EbayWatcher.BusinessLogic
{
    public class Users
    {
        public static bool IsLoggedIn()
        {
            return !GetCurrentUsername().IsNullOrWhiteSpace();
        }
        public static string GetCurrentUsername()
        {
            return HttpContext.Current.Session["EbayUsername"].ToStringOrDefault();
        }
        public static void LogOut()
        {
            HttpContext.Current.Session["EbayUsername"] = null;
        }
    }
}