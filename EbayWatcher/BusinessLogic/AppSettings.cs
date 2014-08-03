using EbayWatcher.BusinessLogic.Exceptions;
using EbayWatcher.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
                var filename = HttpContext.Current.Server.MapPath(Properties.Settings.Default.ApiKeysJsonFile);
                if (!File.Exists(filename))
                {
                    throw new SettingsFileNotFoundException(filename);
                }
                else
                {
                    var json = File.ReadAllText(filename);
                    var settings = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
                    if (!settings.ContainsKey(settingName))
                    {
                        throw new Exception("Setting '" + settingName + "' was not found in settings file '" + filename + "'.");
                    }
                    else
                    {
                        
                        return settings[settingName];
                    }
                }
            }
        }
        public static void Set(string settingName, string value)
        {
            throw new NotImplementedException();
        }
    }
}