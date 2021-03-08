﻿using System.Linq;
using MyShop.Core.Models;

namespace MyShop.Core.Contracts
{
    public interface IRepository<T> where T : BaseEntity
    {
        IQueryable<T> Collections();
        void Commit();
        void Delete(string Id);
        T Find(string id);
        void Insert(T item);
        void Update(T item);
    }
}