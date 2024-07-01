using CRM.Application.Interfaces;
using CRM.Domain.Interfaces;
using System.Threading.Tasks;

namespace CRM.Application.Services
{
    public class GenericService : IGenericService
    {
        private readonly IServiceProvider _serviceProvider;

        public GenericService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> GetCountAsync<T>() where T : class
        {
            var repository = (IGenericRepository<T>)_serviceProvider.GetService(typeof(IGenericRepository<T>));
            if (repository == null)
            {
                throw new InvalidOperationException($"Repositório para o tipo {typeof(T).Name} não encontrado.");
            }
            return await repository.GetCountAsync();
        }
    }
}