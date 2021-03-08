using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core.Contracts;
using MyShop.Core.Models;

namespace MyShop.DataAccess.Sql
{
    public class SQLRepository<T> : IRepository<T> where T : BaseEntity
    {
        internal DataContext context;
        internal DbSet<T> dbSet;

        public SQLRepository(DataContext context) {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        public IQueryable<T> Collections()
        {
            return this.dbSet;
        }

        public void Commit()
        {
            this.context.SaveChanges();
        }

        public void Delete(string Id)
        {
            T item = Find(Id);
            if (this.context.Entry(item).State == EntityState.Detached)
            {
                dbSet.Attach(item);
            }
            dbSet.Remove(item);
        }

        public T Find(string id)
        {
            return this.dbSet.Find(id);
        }

        public void Insert(T item)
        {
            this.dbSet.Add(item);
        }

        public void Update(T item)
        {
            this.dbSet.Attach(item);
            this.context.Entry(item).State = EntityState.Modified;

        }
    }
}
