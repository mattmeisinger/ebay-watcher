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
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        public static string Get(string settingName)
        {
            // Check the environment variables first
            var envSettingName = ("EbayWatcher_" + settingName).ToUpper();
            var fromEnv = Environment.GetEnvironmentVariable(envSettingName);
            if (fromEnv != null)
                return fromEnv;
            else
                throw new Exception($"Environment variable not set, needed for application to run: {envSettingName}");
        }
    }
}