﻿using EbayWatcher.BusinessLogic;
using EbayWatcher.Entities;
using EbayWatcher.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EbayWatcher.Controllers
{
    public class WishlistController : Controller
    {
        // GET: Wishlist
        public ActionResult Index()
        {
            using (var context = new EbayWatcherContext())
            {
                if (!context.WishlistItems.Any())
                {
                    context.WishlistItems.Add(new Models.WishlistItem
                    {
                        UserId = EbayWatcher.BusinessLogic.Users.GetCurrentUserId(),
                        Status = "Need",
                        Name = "1"
                    });
                    context.SaveChanges();
                }
                return View(context.WishlistItems.ToArray());
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
                    s = new WishlistItem { UserId = EbayWatcher.BusinessLogic.Users.GetCurrentUserId() };
                    context.WishlistItems.Add(s);
                }
                else
                {
                    s = context.WishlistItems.Single(a => a.Id == o.Id);
                }

                s.Name = o.Name;
                s.Category = o.Category;
                s.Status = o.Status;
                s.Notes = o.Notes;

                context.SaveChanges();
                o.Id = s.Id;

                return RedirectToAction("Edit", new { Id = o.Id });
            }
        }

        public ActionResult ViewEnded(int id)
        {
            using (var context = new EbayWatcherContext())
            {
                var item = context.WishlistItems.Single(a => a.Id == id);
                var completedItems = Ebay.GetCompletedItems(item.Name, "");
                return Content("success");
            }
        }
    }
}