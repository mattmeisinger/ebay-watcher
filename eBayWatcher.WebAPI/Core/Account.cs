using EbayAPIHelper;
using eBayWatcher.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Web;

namespace eBayWatcher.WebAPI.Core
{
    public class Account
    {
        public EBayAuthStatus Status { get; set; }

        private Account() { }

        public static Account FromSession(string sessionId)
        {
            var account = new Account();
            var cacheKey = "AuthStatus-" + sessionId;
            var minutesToCacheUserProfile = 120;
            account.Status = MemoryCache.Default[cacheKey] as EBayAuthStatus;
            if (account.Status == null)
            {
                account.Status = new EBayAuthStatus
                {
                    Description = "Not logged in",
                    EbaySessionId = null,
                    Username = null,
                    Token = null
                };
                MemoryCache.Default.Add(cacheKey, account.Status, DateTimeOffset.Now.AddMinutes(minutesToCacheUserProfile));
            }
            return account;
        }

        public EBayAuthStatus StartLogin()
        {
            var auth = EbayAuth.CreateNewAuthRequest();
            this.Status.EbaySessionId = auth.SessionId;
            this.Status.LoginUrl = auth.LoginUrl;
            this.Status.Description = "Awaiting eBay Authorization";
            return this.Status;
        }

        public EBayAuthStatus ContinueLoginAfterEbayApproval()
        {
            var auth = EbayAuth.CompleteEbayAuthentication(this.Status.EbaySessionId);
            this.Status.Username = auth.EbayUsername;
            this.Status.Token = auth.Token;
            this.Status.Description = "Logged in as " + this.Status.Username;
            this.Status.LoginUrl = null;
            return this.Status;
        }
    }
}