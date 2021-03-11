using MyShop.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.Web.Controllers
{
    public class BasketController : Controller
    {
        IBasketService basketService;
        // GET: Basket
        public BasketController(IBasketService basketService)
        {
            this.basketService = basketService;
        }
        public ActionResult Index()
        {
            var model = basketService.GetBasketItems(this.HttpContext);
            return View(model);
        }
        public ActionResult AddBasket(string productId)
        {
            basketService.AddToBasket(this.HttpContext, productId);
            return RedirectToAction("Index");
        }
        public ActionResult RemoveBasket(string Id)
        {
            basketService.RemoveToBasket(this.HttpContext, Id);
            return RedirectToAction("Index");
        }
        public PartialViewResult BasketSummary()
        {
            var basketSummary = basketService.GetBasketSummary(this.HttpContext);
            return PartialView(basketSummary);
        }
    }
}