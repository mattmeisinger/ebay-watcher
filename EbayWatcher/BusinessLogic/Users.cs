using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Owin;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using EbayWatcher.Entities.Models;
using EbayWatcher.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EbayWatcher.BusinessLogic
{
    public class Users
    {
        public static ApplicationUser GetCurrentUser()
        {
            using (var context = new EbayWatcherContext())
            {
                var id = HttpContext.Current.User.Identity.GetUserId();
                if (id == null)
                    return null;
                else
                    return context.Users.Single(a => a.Id == id);
            }
        }
        public static string GetCurrentUsername()
        {
            var user = GetCurrentUser();
            if (user == null)
                return null;
            else
                return user.UserName;
        }
        public static bool IsLoggedIn()
        {
            var user = GetCurrentUser();
            return user != null && user.EbayToken != null;
        }
        public static void SaveUserEbaySessionId(string sessionId)
        {
            var store = new UserStore<ApplicationUser>(new EbayWatcherContext());
            var manager = new ApplicationUserManager(store);
            var context = (EbayWatcherContext)store.Context;

            var user = manager.Users.Single(a => a.UserName == GetCurrentUser().UserName);
            user.EbaySessionId = sessionId;
            manager.Update(user);
            context.SaveChanges();
        }

        internal static void SaveUserEbayToken(string token)
        {
            var store = new UserStore<ApplicationUser>(new EbayWatcherContext());
            var manager = new ApplicationUserManager(store);
            var context = (EbayWatcherContext)store.Context;

            var user = manager.Users.Single(a => a.UserName == GetCurrentUser().UserName);
            user.EbayToken = token;
            manager.Update(user);
            context.SaveChanges();
        }
    }
}