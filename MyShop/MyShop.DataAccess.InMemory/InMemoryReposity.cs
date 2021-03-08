using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
   public class InMemoryReposity<T> where T : BaseEntity
    {
        ObjectCache cache = MemoryCache.Default;
        List<T> list = new List<T>();
        string className;
        public InMemoryReposity()
        {
            className = typeof(T).Name;
            list = cache[className] as List<T>;
            if (list == null)
                list = new List<T>();
        }

        public void Commit()
        {
            cache[className] = list;
        }
        public void Insert(T item)
        {
            list.Add(item);
        }

        public void Update(T item)
        {
            T itemToUpdate = list.Find(p => p.Id == item.Id);
            if (itemToUpdate != null)
            {
                itemToUpdate = item;
            }
            else
            {
                throw new Exception(className + " not Found");
            }
        }

        public T Find(string id)
        {
            T item = list.Find(p => p.Id == id);
            if (item != null)
            {
                return item;
            }
            else
            {
                throw new Exception(className + " not Found");
            }
        }

        public IQueryable<T> Collections()
        {
            return list.AsQueryable();
        }

        public void Delete(string Id)
        {
            T itemToDelete = list.Find(p => p.Id == Id);
            if (itemToDelete != null)
            {
                list.Remove(itemToDelete);
            }
            else
            {
                throw new Exception(className + "not Found");
            }
        }
    }
}
