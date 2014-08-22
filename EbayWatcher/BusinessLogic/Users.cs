using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EbayWatcher.Entities;

namespace EbayWatcher.BusinessLogic
{
    public class Users
    {
        internal static void LogOut()
        {
            HttpContext.Current.Session["EbayUsername"] = null;
            HttpContext.Current.Session["EbaySessionId"] = null;
            HttpContext.Current.Session["EbayToken"] = null;
        }

        public static bool IsLoggedIn()
        {
            return !GetCurrentUsername().IsNullOrWhiteSpace();
        }

        public static string GetCurrentUsername()
        {
            return HttpContext.Current.Session["EbayUsername"].ToStringOrDefault();
        }
        internal static string GetCurrentSessionId()
        {
            return HttpContext.Current.Session["EbaySessionId"].ToStringOrDefault();
        }
        internal static string GetCurrentEbayToken()
        {
            return HttpContext.Current.Session["EbayToken"].ToStringOrDefault();
        }

        public static User GetCurrentUser()
        {
            using (var context = new EbayWatcherContext())
            {
                var username = GetCurrentUsername();
                if (username == null)
                    return null;
                else
                    return context.Users.SingleOrDefault(a => a.EbayUsername == username); // Using SingleOrDefault because user may have been deleted, but their cookie is still valid
            }
        }
    }
}