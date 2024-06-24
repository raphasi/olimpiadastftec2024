using CRM.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Domain.Interfaces;

public interface IPriceLevelRepository
{
    Task<PriceLevel> GetPriceLevelByIdAsync(Guid priceLevelId);
    Task<IEnumerable<PriceLevel>> GetAllPriceLevelsAsync();
    Task AddPriceLevelAsync(PriceLevel priceLevelEntity);
    Task UpdatePriceLevelAsync(PriceLevel priceLevelEntity);
    Task DeletePriceLevelAsync(Guid priceLevelId);
    Task<IEnumerable<PriceLevel>> SearchAsync(string query);
    Task<IEnumerable<PriceLevel>> GetTop10Async();
}