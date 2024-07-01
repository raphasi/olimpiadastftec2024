using CRM.Domain.Interfaces;
using CRM.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _genericContext;

        public GenericRepository(ApplicationDbContext genericContext)
        {
            _genericContext = genericContext;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _genericContext.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _genericContext.Set<T>().Update(entity);
            await _genericContext.SaveChangesAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _genericContext.Set<T>().CountAsync();
        }
    }
}