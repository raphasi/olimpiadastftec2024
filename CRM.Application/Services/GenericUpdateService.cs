using AutoMapper;
using CRM.Application.Interfaces;
using CRM.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Application.Services
{
    public class GenericUpdateService<T> : IGenericUpdateService<T> where T : class
    {
        private readonly IGenericRepository<T> _repository;
        private readonly IMapper _mapper;

        public GenericUpdateService(IGenericRepository<T> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task UpdateFieldAsync(Guid id, string fieldName, object fieldValue)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Entity with ID {id} not found.");
            }

            var propertyInfo = typeof(T).GetProperty(fieldName);
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Field {fieldName} not found on entity {typeof(T).Name}.");
            }

            // Update the field value
            propertyInfo.SetValue(entity, fieldValue);

            await _repository.UpdateAsync(entity);
        }
    }
}