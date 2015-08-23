using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using EbayAPIHelper;
using eBayWatcher.WebAPI.Models;
using EbayAPIHelper.Models;

namespace eBayWatcher.WebAPI.Core
{
    public class Account
    {
        internal static EBayAuthStatus StartSession()
        {
            var auth = EbayAuth.CreateNewAuthRequest();
            return new EBayAuthStatus
            {
                SessionId = auth.SessionId,
                LoginUrl = auth.LoginUrl
            };
        }

        internal static EBayAuthStatus ConfirmAuthentication(string sessionId)
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