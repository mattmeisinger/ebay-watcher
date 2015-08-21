using EbayWatcher.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace EbayWatcherTest
{
    public class Program
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            Console.WriteLine("Getting settings...");
            EbayAPIHelper.EbaySettings.AppID = AppSettings.Get("AppID");
            EbayAPIHelper.EbaySettings.CertID = AppSettings.Get("CertID");
            EbayAPIHelper.EbaySettings.DevID = AppSettings.Get("DevID");
            EbayAPIHelper.EbaySettings.EBayToken = AppSettings.Get("EBayToken");
            EbayAPIHelper.EbaySettings.RuName = AppSettings.Get("RuName");
            EbayAPIHelper.EbaySettings.TradingServerAddress = ConfigurationManager.AppSettings["TradingServerAddress"];
            EbayAPIHelper.EbaySettings.FindingServerAddress = ConfigurationManager.AppSettings["FindingServerAddress"];
            EbayAPIHelper.EbaySettings.EbayXMLAPIURL = ConfigurationManager.AppSettings["EbayXMLAPIURL"];

            Console.WriteLine("Creating Auth Request...");
            var ebayAuthRequest = EbayAPIHelper.EbayAuth.CreateNewAuthRequest();

            Console.WriteLine("Got auth request response.");
            Console.WriteLine($"  SesssionID: {ebayAuthRequest.SessionId}");
            Console.WriteLine($"  LoginURL:   {ebayAuthRequest.LoginUrl}");

            Console.WriteLine("Press any key to open browser window for eBay authentication.");
            Process.Start(ebayAuthRequest.LoginUrl);

            Console.WriteLine("Press any key once login process is complete at the login URL.");
            Console.ReadLine();

            Console.WriteLine("Completing authorization...");
            var auth = EbayAPIHelper.EbayAuth.CompleteEbayAuthentication(ebayAuthRequest.SessionId);

            Console.WriteLine("Authorized successfully!");

            Console.WriteLine($"  Username: {auth.EbayUsername}");
            Console.WriteLine($"  Token:    {auth.Token}");
            Console.ReadLine();
        }
    }
}
