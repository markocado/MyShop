using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.Web.Controllers
{
    public class HomeController : Controller
    {

        IRepository<Product> productContext;
        IRepository<ProductCategory> productCategoryContext;

        public HomeController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            this.productContext = productContext;
            this.productCategoryContext = productCategoryContext;
        }

        public ActionResult Index(string category = null)
        {
            List<Product> products;
            List<ProductCategory> productCategories = this.productCategoryContext.Collections().ToList();

            if (category == null)
            {
                products = this.productContext.Collections().ToList();
            } 
            else
            {
                products = this.productContext.Collections().Where(w => w.Category == category).ToList();
            }

            ProductListViewModel model = new ProductListViewModel();
            model.Products = products;
            model.ProductCategories = productCategories;

            return View(model);
        }

        public ActionResult ProductDetails(string id)
        {
            Product product = this.productContext.Find(id);
            if (product == null)
                return HttpNotFound();
            else 
                return View(product);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}