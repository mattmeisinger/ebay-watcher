using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EbayWatcher.BusinessLogic.Exceptions
{
    [Serializable]
    public class SettingsFileNotFoundException : Exception
    {
        // The path where the settings file was expected to be
        public string SettingsFilePath { get; set; }

        public SettingsFileNotFoundException(string settingsFilePath)
        {
            SettingsFilePath = settingsFilePath;
        }
    }
}