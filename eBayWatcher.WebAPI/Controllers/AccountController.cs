using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using eBayWatcher.WebAPI.Core;
using eBayWatcher.WebAPI.Models;

namespace eBayWatcher.WebAPI.Controllers
{
    public class AccountController : ApiController
    {
        [Route("Account")]
        [HttpPost]
        public EBayAuthStatus CreateNewSession() => Account.StartSession();

        [Route("Account/ConfirmLogin")]
        [HttpPost]
        public EBayAuthStatus CompleteEBayAuthentication([FromBody] dynamic p) => Account.ConfirmAuthentication((string)p.sessionId);
    }
}
