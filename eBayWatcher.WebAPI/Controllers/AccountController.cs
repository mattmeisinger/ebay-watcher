using eBayWatcher.Core;
using eBayWatcher.Core.Models;
using eBayWatcher.DynamoDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace eBayWatcher.WebAPI.Controllers
{
    public class AccountController : ApiController
    {
        [Route("Account")]
        [HttpPost]
        public EBayAuthStatus CreateNewSession() => Account.StartSession();

        [Route("Account/ConfirmLogin")]
        [HttpPost]
        public EBayAuthStatus CompleteEBayAuthentication([FromBody] dynamic p)
        {
            var auth = Account.ConfirmAuthentication((string)p.sessionId);
            Users.AddUserToken(auth.Username, auth.Token);
            return auth;
        }
    }
}
