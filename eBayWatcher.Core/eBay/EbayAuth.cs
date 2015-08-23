using eBay.Service.Call;
using eBayWatcher.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eBayWatcher.Core.eBay
{
    public class EbayAuth
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        public static EbayAuthRequest CreateNewAuthRequest()
        {
            var client = EbayClientHelper.GetSdkClient();
            var call = new GetSessionIDCall(client);
            var sessionId = call.GetSessionID(EbaySettings.RuName);
            var urlEncodedSessionID = System.Net.WebUtility.UrlEncode(sessionId);
            var loginUrl = string.Format("https://signin.ebay.com/ws/eBayISAPI.dll?SignIn&RuName={0}&SessID={1}&AcceptURL=http://google.com", EbaySettings.RuName, urlEncodedSessionID);

            var data = new EbayAuthRequest
            {
                SessionId = sessionId,
                LoginUrl = loginUrl
            };

            log.Debug($"Created New Auth Request: {data}");

            return data;
        }
        public static EbayAuthenticatedCredentials CompleteEbayAuthentication(string sessionId)
        {
            // Get token from Ebay
            var client = EbayClientHelper.GetSdkClient();


            // Otherwise get the user id of the logged in user from Ebay
            log.Debug("Fetching username for Session ID " + sessionId);
            var userCall = new CallHandlers.ConfirmIdentityCall(client);
            var ebayUsername = userCall.GetUsername(sessionId).ToLower(); // Always use the lowercase version of the username

            // Fetch token
            log.Debug("Fetching token for Session ID " + sessionId);
            var call = new FetchTokenCall(client);
            var token = call.FetchToken(sessionId);

            // If the token comes back empty, that means they didn't complete the sign-in process.
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }
            else
            {
                var data = new EbayAuthenticatedCredentials
                {
                    SessionId = sessionId,
                    EbayUsername = ebayUsername,
                    Token = token
                };

                log.Debug($"User authenticated: {data}");
                return data;
            }
        }
    }
}
