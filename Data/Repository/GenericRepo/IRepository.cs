﻿using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.GenericRepo
{
    public interface IRepository<T> where T : class
    {
        Task<T?> Get(string id);
        Task<List<T>> GetAll();
        Task Insert(T entity);
        Task<bool> Update(T entity);
        Task<bool> Remove(T entity);
        Task<bool> RemoveRange(List<T> entity);


    }
}

