using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.DataAccess.InMemory;
namespace MyShop.Web.Controllers
{
    public class ProductManagerController : Controller
    {
        IRepository<Product> context;
        IRepository<ProductCategory> productCategoryContext;
        public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoryContext)
        {
            this.context = productContext;
            this.productCategoryContext = productCategoryContext;
        }
        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = context.Collections().ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            ProductManagerViewModel productManagerVM = new ProductManagerViewModel();
            productManagerVM.Product = new Product();
            productManagerVM.ProductCategories = productCategoryContext.Collections();
            return View(productManagerVM);
        }
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            } else
            {
                context.Insert(product);
                context.Commit();
                return RedirectToAction("Index");
            }
    
        }

        public ActionResult Edit(string Id)
        {
            Product product = context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            } else
            {
                ProductManagerViewModel productManagerVM = new ProductManagerViewModel();
                productManagerVM.Product = product;
                productManagerVM.ProductCategories = productCategoryContext.Collections();
                return View(productManagerVM);
            }

        }

        [HttpPost]
        public ActionResult Edit(Product product, string Id)
        {
            Product productToUpdate = context.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
               if (!ModelState.IsValid)
                {
                    return View(product);
                } 
                else
                {
                    productToUpdate.Name = product.Name;
                    productToUpdate.Category = product.Category;
                    productToUpdate.Description = product.Description;
                    productToUpdate.Image = product.Image;
                    productToUpdate.Price = product.Price;
                    context.Update(productToUpdate);
                    context.Commit();
                    return RedirectToAction("Index");
                }
            
            }

        }

        public ActionResult Delete(string Id)
        {
            Product productToDelete = context.Find(Id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productToDelete);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Product productToDelete = context.Find(Id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);
                context.Commit();
                return RedirectToAction("Index");
            }
        }
    }
}