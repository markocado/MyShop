using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core;
using MyShop.Core.Models;

namespace MyShop.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<ProductCategory> productCategories = new List<ProductCategory>();
        public ProductCategoryRepository()
        {
            productCategories = cache["productCategory"] as List<ProductCategory>;
            if (productCategories == null)
            {
                productCategories = new List<ProductCategory>();
            }
        }

        public void Commit()
        {
            cache["productCategory"] = productCategories;
        }
        public void Insert(ProductCategory p)
        {
            productCategories.Add(p);
        }

        public void Update(ProductCategory productCategory)
        {
            ProductCategory productCategoryToUpdate = productCategories.Find(p => p.Id == productCategory.Id);
            if (productCategoryToUpdate != null)
            {
                productCategoryToUpdate = productCategory;
            }
            else
            {
                throw new Exception("Product Category Not Found");
            }
        }

        public ProductCategory Find(string id)
        {
            ProductCategory productCategory = productCategories.Find(p => p.Id == id);
            if (productCategory != null)
            {
                return productCategory;
            }
            else
            {
                throw new Exception("Product Category Not Found");
            }
        }

        public IQueryable<ProductCategory> Collections()
        {
            return productCategories.AsQueryable();
        }

        public void Delete(string id)
        {
            ProductCategory productCategoryToDelete = productCategories.Find(p => p.Id == id);
            if (productCategoryToDelete != null)
            {
                productCategories.Remove(productCategoryToDelete);
            }
            else
            {
                throw new Exception("Product Category Not Found");
            }
        }
    }
}
