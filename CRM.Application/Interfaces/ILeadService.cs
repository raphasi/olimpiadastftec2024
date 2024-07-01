using CRM.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Interfaces;

public interface ILeadService
{
    Task<IEnumerable<LeadDTO>> GetAllAsync();
    Task<LeadDTO> GetByIdAsync(Guid id);
    Task<LeadDTO> AddAsync(LeadDTO lead);
    Task UpdateAsync(LeadDTO lead);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<LeadDTO>> SearchAsync(string query);
}
