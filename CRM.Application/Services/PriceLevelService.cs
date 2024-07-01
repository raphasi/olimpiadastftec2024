using AutoMapper;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Application.DTOs;
using CRM.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace CRM.Application.Services;

public class PriceLevelService : IPriceLevelService
{
    private readonly IPriceLevelRepository _priceLevelRepository;
    private readonly IMapper _mapper;

    public PriceLevelService(IPriceLevelRepository priceLevelRepository, IMapper mapper)
    {
        _priceLevelRepository = priceLevelRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PriceLevelDTO>> GetAllAsync()
    {
        var priceLevels = await _priceLevelRepository.GetAllPriceLevelsAsync();
        return _mapper.Map<IEnumerable<PriceLevelDTO>>(priceLevels);
    }

    public async Task<PriceLevelDTO> GetByIdAsync(Guid id)
    {
        var priceLevel = await _priceLevelRepository.GetPriceLevelByIdAsync(id);
        return _mapper.Map<PriceLevelDTO>(priceLevel);
    }

    public async Task<PriceLevelDTO> AddAsync(PriceLevelDTO priceLevel)
    {
        priceLevel.PriceLevelID = Guid.NewGuid();
        var priceLevelEntity = _mapper.Map<PriceLevel>(priceLevel);
        await _priceLevelRepository.AddPriceLevelAsync(priceLevelEntity);
        return priceLevel;
    }

    public async Task UpdateAsync(PriceLevelDTO priceLevel)
    {
        var priceLevelEntity = _mapper.Map<PriceLevel>(priceLevel);
        await _priceLevelRepository.UpdatePriceLevelAsync(priceLevelEntity);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _priceLevelRepository.DeletePriceLevelAsync(id);
    }

    public async Task<IEnumerable<PriceLevelDTO>> SearchAsync(string query)
    {
        var opps = string.IsNullOrEmpty(query)
            ? await _priceLevelRepository.GetTop10Async()
            : await _priceLevelRepository.SearchAsync(query);
        return opps.Select(c => new PriceLevelDTO
        {
            PriceLevelID = c.PriceLevelID,
            LevelName = c.LevelName
        });
    }
}