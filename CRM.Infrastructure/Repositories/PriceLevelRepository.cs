using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using CRM.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Repositories
{
    public class PriceLevelRepository : IPriceLevelRepository
    {
        private readonly ApplicationDbContext _priceLevelContext;

        public PriceLevelRepository(ApplicationDbContext priceLevelContext)
        {
            _priceLevelContext = priceLevelContext;
        }

        public async Task<PriceLevel> GetPriceLevelByIdAsync(Guid priceLevelId)
        {
            return await _priceLevelContext.Set<PriceLevel>().FindAsync(priceLevelId);
        }

        public async Task<IEnumerable<PriceLevel>> GetAllPriceLevelsAsync()
        {
            return await _priceLevelContext.Set<PriceLevel>().ToListAsync();
        }

        public async Task AddPriceLevelAsync(PriceLevel priceLevelEntity)
        {
            await _priceLevelContext.Set<PriceLevel>().AddAsync(priceLevelEntity);
            await _priceLevelContext.SaveChangesAsync();
        }

        public async Task UpdatePriceLevelAsync(PriceLevel priceLevelEntity)
        {
            _priceLevelContext.Set<PriceLevel>().Update(priceLevelEntity);
            await _priceLevelContext.SaveChangesAsync();
        }

        public async Task DeletePriceLevelAsync(Guid priceLevelId)
        {
            var priceLevel = await _priceLevelContext.Set<PriceLevel>().FindAsync(priceLevelId);
            if (priceLevel != null)
            {
                _priceLevelContext.Set<PriceLevel>().Remove(priceLevel);
                await _priceLevelContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<PriceLevel>> SearchAsync(string query)
        {
            return await _priceLevelContext.PriceLevels
                .Where(c => c.LevelName.Contains(query))
                .ToListAsync();
        }
        public async Task<IEnumerable<PriceLevel>> GetTop10Async()
        {
            return await _priceLevelContext.PriceLevels
                .OrderByDescending(c => c.CreatedOn)
                .Take(10)
                .ToListAsync();
        }
    }
}