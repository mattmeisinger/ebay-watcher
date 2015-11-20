using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Xml.Linq;
using eBayWatcher.Core.Helpers;
using eBay.Service.Core.Sdk;

namespace eBayWatcher.Core.eBay.CallHandlers
{
    public class ConfirmIdentityCall
    {
        private ApiContext client;
        public ConfirmIdentityCall(ApiContext client)
        {
            this.client = client;
        }

        public string GetUsername(string sessionId)
        {
            var headers = new WebHeaderCollection();
            headers.Add("X-EBAY-API-SITEID", "0");
            headers.Add("X-EBAY-API-COMPATIBILITY-LEVEL", "837");
            headers.Add("X-EBAY-API-DEV-NAME", EbaySettings.DevID);
            headers.Add("X-EBAY-API-APP-NAME", EbaySettings.AppID);
            headers.Add("X-EBAY-API-CERT-NAME", EbaySettings.CertID);
            headers.Add("X-EBAY-API-CALL-NAME", "ConfirmIdentity");

            var xmlTemplate =
                @"<?xml version=""1.0"" encoding=""utf-8""?>
                    <ConfirmIdentityRequest xmlns=""urn:ebay:apis:eBLBaseComponents"">
                      <SessionID>{0}</SessionID>
                    </ConfirmIdentityRequest>";
            var xmlRequest = string.Format(xmlTemplate, sessionId);

            string xmlResponse = Utilities.PostXML(EbaySettings.EbayXMLAPIURL, xmlRequest, headers);

            var doc = XDocument.Parse(xmlResponse);
            return doc.Descendants()
                .Single(a => a.Name.LocalName == "UserID") // Get the element named UserID
                .Value;
        }
    }
}
