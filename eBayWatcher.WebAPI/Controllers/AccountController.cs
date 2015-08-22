using EbayAPIHelper;
using EbayAPIHelper.Models;
using eBayWatcher.WebAPI.Core;
using eBayWatcher.WebAPI.Models;
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
        public string CreateNewSession() => Guid.NewGuid().ToString();


        [Route("Account/{sessionId}")]
        [HttpGet]
        public EBayAuthStatus GetStatus(string sessionId) => Account.FromSession(sessionId).Status;

        [Route("Account/{sessionId}")]
        [HttpDelete]
        public void LogOut(string sessionId) { }


        [Route("Account/{sessionId}/StartLogin")]
        [HttpPost]
        public EBayAuthStatus RequestAuthorization(string sessionId) => Account.FromSession(sessionId).StartLogin();

        [Route("Account/{sessionId}/ContinueLogin")]
        [HttpPost]
        public EBayAuthStatus ConfirmEbayAccess(string sessionId) => Account.FromSession(sessionId).ContinueLoginAfterEbayApproval();
    }
}
