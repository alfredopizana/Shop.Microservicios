using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts;
using Ordering.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
    public class AsyncRepository<T> : IAsyncRepository<T> where T : EntityBase
    {
        private readonly OrderContext context;

        public AsyncRepository(OrderContext context)
        {
            this.context = context;
        }

        public async Task<T> AddAsync(T entity)
        {
            context.Set<T>().Add(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeString = null)
        {
            IQueryable<T> query = context.Set<T>(); //Select * from T
            if(!string.IsNullOrWhiteSpace(includeString))
                query = query.Include(includeString); // join other table
            if(predicate != null)
                query = query.Where(predicate); //Where predicate = 1
            if(orderBy != null)
                return await orderBy(query).ToListAsync(); //order by column, column 2
            return await query.ToListAsync();

        }

        public async Task<T> GetByIdAsync(int id)
        {
            
            return await context.Set<T>().FindAsync(id);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return entity;
            
        }
    }
}
