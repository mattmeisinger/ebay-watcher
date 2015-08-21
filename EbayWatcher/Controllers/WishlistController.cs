using EbayWatcher.BusinessLogic;
using EbayWatcher.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EbayWatcher.Controllers
{
    public class WishlistController : BaseController
    {
        public ActionResult Index()
        {
            using (var context = new EbayWatcherContext())
            {
                var data = context.WishlistItems.ToArray()
                    .Select(a => Converters.WishlistItemConverter.Convert(a))
                    .ToArray();
                return View(data);
            }
        }

        [HttpGet]
        public ActionResult Edit(int? Id = null)
        {
            using (var context = new EbayWatcherContext())
            {
                if (Id.HasValue)
                    return View(context.WishlistItems.Single(a => a.Id == Id));
                else
                    return View(new WishlistItem { });
            }
        }

        [HttpPost]
        public ActionResult Edit(WishlistItem o)
        {
            using (var context = new EbayWatcherContext())
            {
                WishlistItem s = null;
                if (o.Id == 0)
                {
                    s = new Entities.WishlistItem
                    {
                        //UserId = EbayWatcher.BusinessLogic.Users.GetCurrentUsername()
                    };
                    context.WishlistItems.Add(s);
                }
                else
                {
                    s = context.WishlistItems.Single(a => a.Id == o.Id);
                }

                s.Name = o.Name;
                s.CategoryId = o.CategoryId;
                s.Status = o.Status;
                s.Notes = o.Notes;

                context.SaveChanges();
                o.Id = s.Id;

                return RedirectToAction("Edit", new { Id = o.Id });
            }
        }

        public ActionResult _FindEnded(int id)
        {
            using (var context = new EbayWatcherContext())
            {
                var item = context.WishlistItems.Single(a => a.Id == id);
                var completedItems = Ebay.GetCompletedItems(item.Name, item.CategoryId);
                return PartialView(completedItems);
            }
        }

        public ActionResult _FindCurrent(int id)
        {
            using (var context = new EbayWatcherContext())
            {
                var item = context.WishlistItems.Single(a => a.Id == id);
                var completedItems = Ebay.GetCurrentItems(item.Name, item.CategoryId);
                return Json(completedItems, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult _FindCategory(string searchTerm)
        {
            ViewBag.SearchTerm = searchTerm;

            if (searchTerm != null && searchTerm.Length > 2)
            {
                var data = Ebay.FindCategories(searchTerm);
                return PartialView(data);
            }
            else
            {
                return Content("Unable to find categories. Please enter a valid search term larger than 2 characters.");
            }
        }
    }
}