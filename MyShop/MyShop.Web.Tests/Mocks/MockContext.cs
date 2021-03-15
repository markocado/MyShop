using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Web.Tests.Mocks
{
    public class MockContext<T> : IRepository<T> where T : BaseEntity
    {
        List<T> list = new List<T>();
        string className = "";
        public MockContext()
        {
            if (list == null)
                list = new List<T>();
        }

        public void Commit()
        {
            return;
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
