using eBayWatcher.Core.eBay;
using eBayWatcher.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace eBayWatcher.Core
{
    public class Account
    {
        public static EBayAuthStatus StartSession()
        {
            var auth = EbayAuth.CreateNewAuthRequest();
            return new EBayAuthStatus
            {
                SessionId = auth.SessionId,
                LoginUrl = auth.LoginUrl
            };
        }

        public static EBayAuthStatus ConfirmAuthentication(string sessionId)
        {
            EbayAuthenticatedCredentials auth;
            try
            {
                auth = EbayAuth.CompleteEbayAuthentication(sessionId);
            }
            catch (Exception)
            {
                throw new Exception("Unable to complete authentication. Please ensure you have logged into eBay.");
            }

            return new EBayAuthStatus
            {
                SessionId = sessionId,
                Token = auth.Token,
                Username = auth.EbayUsername
            };
        }
    }
}