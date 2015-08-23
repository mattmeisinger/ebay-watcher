using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Runtime.Caching;

namespace eBayWatcher.WebAPI.Filters
{
    public class EnsureMatchingTokenAndUsernameAttribute : ActionFilterAttribute
    {
        private static NLog.Logger log = NLog.LogManager.GetCurrentClassLogger();

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            // Want to prevent a user from impersonating another user just by changing their cookies or headers,
            // so match up the username and token to all usernames and tokens that we know have been distributed to
            // them by this system.  If we can't find a record of them having received that token, then we throw an
            // exception and don't give them access to the data.
            var username = actionContext.Request.Headers.GetValues("eBayWatcherUsername").FirstOrDefault();
            var token = actionContext.Request.Headers.GetValues("eBayWatcherToken").FirstOrDefault();

            var cacheKey = "LastToken-" + username;
            var lastTokenUsedByUser = MemoryCache.Default[cacheKey] as string;
            if (lastTokenUsedByUser != null && lastTokenUsedByUser == token)
            {
                // This user is okay, we remember giving them this token. Don't do anything. Let them pass.
                log.Debug("Found cached token for user " + username);
            }
            else
            {
                // We need to check whether this user was actually issued this token
                var user = DynamoDB.Users.Get(username);
                if (user == null)
                {
                    log.Warn($"User was trying to get in with a username that wasn't theirs by using an existing token. User: {username}, Token: {token}");
                    throw new Exception("Invalid request. No record of this user having logged into the system.");
                }
                else if (user.Tokens == null || !user.Tokens.Contains(token))
                {
                    log.Warn($"User was trying to get in with a username or token that wasn't theirs. User: {username}, Token: {token}");
                    throw new Exception("Invalid request. This token does not belong to this user.");
                }
                else
                {
                    throw new Exception("test");
                    // This user is okay, we gave them this token before.
                    log.Info("Matched up user token with username " + username);
                    MemoryCache.Default.Add(cacheKey, token, DateTimeOffset.Now.AddHours(1));
                }
            }

            base.OnActionExecuting(actionContext);
        }
    }
}