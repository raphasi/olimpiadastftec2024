using CRM.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Interfaces;

public interface IPriceLevelService
{
    Task<IEnumerable<PriceLevelDTO>> GetAllAsync();
    Task<PriceLevelDTO> GetByIdAsync(Guid id);
    Task AddAsync(PriceLevelDTO priceLevel);
    Task UpdateAsync(PriceLevelDTO priceLevel);
    Task DeleteAsync(Guid id);
}
