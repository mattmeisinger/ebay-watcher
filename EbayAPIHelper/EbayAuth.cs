using eBay.Service.Call;
using EbayAPIHelper.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbayAPIHelper
{
    public class EbayAuth
    {
        public static EbayAuthRequest CreateNewAuthRequest()
        {
            var client = EbayClientHelper.GetSdkClient();
            var call = new GetSessionIDCall(client);
            var sessionId = call.GetSessionID(EbaySettings.RuName);
            var urlEncodedSessionID = System.Net.WebUtility.UrlEncode(sessionId);
            var loginUrl = string.Format("https://signin.ebay.com/ws/eBayISAPI.dll?SignIn&runame={0}&SessID={1}", EbaySettings.RuName, urlEncodedSessionID);

            return new EbayAuthRequest
            {
                SessionId = sessionId,
                LoginUrl = loginUrl
            };
        }
        public static EbayAuthenticatedCredentials CompleteEbayAuthentication(string sessionId)
        {
            // Get token from Ebay
            var client = EbayClientHelper.GetSdkClient();
            var call = new FetchTokenCall(client);
            var token = call.FetchToken(sessionId);

            // If the token comes back empty, that means they didn't complete the sign-in process.
            if (string.IsNullOrWhiteSpace(token))
            {
                return null;
            }
            else
            {
                // Otherwise get the user id of the logged in user from Ebay
                var userCall = new ConfirmIdentityCall(client);
                var ebayUsername = userCall.ConfirmIdentity(sessionId).ToLower(); // Always use the lowercase version of the username
                return new EbayAuthenticatedCredentials
                {
                    SessionId = sessionId,
                    EbayUsername = ebayUsername,
                    Token = token
                };
            }
        }
    }
}
