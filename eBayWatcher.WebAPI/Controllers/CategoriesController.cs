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
    public class CategoriesController : ApiController
    {
        [Route("Categories/Search")]
        [HttpPost]
        public CategoryListItem[] Search([FromBody] dynamic p) => Ebay.FindCategories((string)p.eBayToken, (string)p.searchTerm);
    }
}
