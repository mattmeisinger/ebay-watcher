using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eBayWatcher.eBay
{
    public class EbaySettings
    {
        public static string AppID { get; set; }
        public static string DevID { get; set; }
        public static string CertID { get; set; }
        public static string RuName { get; set; }
        public static string TradingServerAddress { get; set; }
        public static string FindingServerAddress { get; set; }
        public static string EbayXMLAPIURL { get; set; }

        public static void Validate()
        {
            if (AppID == null || DevID == null || CertID == null || RuName == null || TradingServerAddress == null)
                throw new Exception("Ebay AppID, DevID, CertID, EBayToken, RuName, and TradingServerAddress must be set for API to operate properly.");
        }
    }
}
