using AutoMapper;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Application.DTOs;
using CRM.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace CRM.Application.Services;

public class OpportunityService : IOpportunityService
{
    private readonly IOpportunityRepository _opportunityRepository;
    private readonly IMapper _mapper;

    public OpportunityService(IOpportunityRepository opportunityRepository, IMapper mapper)
    {
        _opportunityRepository = opportunityRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OpportunityDTO>> GetAllAsync()
    {
        var opportunities = await _opportunityRepository.GetAllOpportunitiesAsync();
        return _mapper.Map<IEnumerable<OpportunityDTO>>(opportunities);
    }

    public async Task<(int Count, decimal? EstimatedValue)> GetCountAndEstimatedValueAsync()
    {
        return await _opportunityRepository.GetCountAndEstimatedValueAsync();
    }


    public async Task<OpportunityDTO> GetByIdAsync(Guid id)
    {
        var opportunity = await _opportunityRepository.GetOpportunityByIdAsync(id);
        return _mapper.Map<OpportunityDTO>(opportunity);
    }

    public async Task<OpportunityDTO> AddAsync(OpportunityDTO opportunity)
    {
        opportunity.OpportunityID = Guid.NewGuid();
        var opportunityEntity = _mapper.Map<Opportunity>(opportunity);
        await _opportunityRepository.AddOpportunityAsync(opportunityEntity);
        return opportunity;
    }

    public async Task UpdateAsync(OpportunityDTO opportunity)
    {
        var opportunityEntity = _mapper.Map<Opportunity>(opportunity);
        await _opportunityRepository.UpdateOpportunityAsync(opportunityEntity);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _opportunityRepository.DeleteOpportunityAsync(id);
    }

    public async Task<IEnumerable<OpportunityDTO>> SearchAsync(string query)
    {
        var opps = string.IsNullOrEmpty(query)
            ? await _opportunityRepository.GetTop10Async()
            : await _opportunityRepository.SearchAsync(query);
        return opps.Select(c => new OpportunityDTO
        {
            OpportunityID = c.OpportunityID,
            Name = c.Name,
            CustomerID = c.CustomerID
        });
    }
}