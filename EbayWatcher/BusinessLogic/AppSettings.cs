using EbayWatcher.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EbayWatcher.BusinessLogic
{
    public class AppSettings
    {
        public static string Get(string settingName)
        {
            using (var context = new EbayWatcherContext())
            {
                return context.AppSettings
                    .Where(a => a.Name == settingName)
                    .Select(a => a.Value)
                    .FirstOrDefault();
            }
        }
        public static void Set(string settingName, string value)
        {
            using (var context = new EbayWatcherContext())
            {
                var item = context.AppSettings
                    .Where(a => a.Name == settingName)
                    .FirstOrDefault();

                // If setting does not exist, create it
                if (item == null)
                {
                    item = new Models.AppSetting { Name = settingName };
                    context.AppSettings.Add(item);
                }

                item.Value = value;
                context.SaveChanges();
            }
        }
    }
}