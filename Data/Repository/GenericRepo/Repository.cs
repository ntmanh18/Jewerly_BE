using Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repository.GenericRepo
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly JewerlyV6Context context;
        private readonly DbSet<T> _entities;

        public Repository(JewerlyV6Context context)
        {
            this.context = context;
            _entities = context.Set<T>();
        }

        public async Task<T?> Get(string id)
        {
            return await _entities.FindAsync(id.ToString());
        }

        public async Task<List<T>> GetAll()
        {
            return await _entities.ToListAsync();
        }

        public async Task Insert(T entity)
        {
            _ = await _entities.AddAsync(entity);
            _ = await context.SaveChangesAsync();
#pragma warning disable CS8605 // Unboxing a possibly null value.

#pragma warning restore CS8605 // Unboxing a possibly null value.
        }

        public async Task<bool> Remove(T entity)
        {
            _ = _entities.Remove(entity);
            _ = await context.SaveChangesAsync();
#pragma warning disable CS8605 // Unboxing a possibly null value.
            return true;
#pragma warning restore CS8605 // Unboxing a possibly null value.
        }

        public async Task<bool> Update(T entity)
        {
            _ = _entities.Update(entity);
            _ = await context.SaveChangesAsync();
#pragma warning disable CS8605 // Unboxing a possibly null value.
            return true;
#pragma warning restore CS8605 // Unboxing a possibly null value.
        }

        public async Task<bool> AddRange(List<T> entities)
        {
            await _entities.AddRangeAsync(entities);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateRange(List<T> entities)
        {
            _entities.UpdateRange(entities);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
