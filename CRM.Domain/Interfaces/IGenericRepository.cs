using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Domain.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T> GetByIdAsync(Guid id);
    Task UpdateAsync(T entity);
    // Outros métodos genéricos que você possa precisar, como Add, Delete, etc.
    Task<int> GetCountAsync();
}
