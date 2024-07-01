using System;
using System.Threading.Tasks;

namespace CRM.Application.Interfaces
{
    public interface IGenericUpdateService<T> where T : class
    {
        Task UpdateFieldAsync(Guid id, string fieldName, object fieldValue);
    }
}