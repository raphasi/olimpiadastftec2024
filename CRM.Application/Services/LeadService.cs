using AutoMapper;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Application.DTOs;
using CRM.Domain.Interfaces;

namespace CRM.Application.Services
{
    public class LeadService : ILeadService
    {
        private readonly ILeadRepository _leadRepository;
        private readonly IMapper _mapper;

        public LeadService(ILeadRepository leadRepository, IMapper mapper)
        {
            _leadRepository = leadRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LeadDTO>> GetAllAsync()
        {
            var leads = await _leadRepository.GetAllLeadsAsync();
            return _mapper.Map<IEnumerable<LeadDTO>>(leads);
        }

        public async Task<LeadDTO> GetByIdAsync(Guid id)
        {
            var lead = await _leadRepository.GetLeadByIdAsync(id);
            return _mapper.Map<LeadDTO>(lead);
        }

        public async Task<LeadDTO> AddAsync(LeadDTO lead)
        {
            lead.LeadID = Guid.NewGuid();   
            var leadEntity = _mapper.Map<Lead>(lead);
            await _leadRepository.AddLeadAsync(leadEntity);

            return lead;
        }

        public async Task UpdateAsync(LeadDTO lead)
        {
            var leadEntity = _mapper.Map<Lead>(lead);
            await _leadRepository.UpdateLeadAsync(leadEntity);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _leadRepository.DeleteLeadAsync(id);
        }

        public async Task<IEnumerable<LeadDTO>> SearchAsync(string query)
        {
            var customers = string.IsNullOrEmpty(query)
                ? await _leadRepository.GetTop10Async()
                : await _leadRepository.SearchAsync(query);
            return customers.Select(c => new LeadDTO
            {
                LeadID = c.LeadID,
                FullName = c.FullName,
                Email = c.Email
            });
        }
    }
}