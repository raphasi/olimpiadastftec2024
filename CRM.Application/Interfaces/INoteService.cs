using CRM.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Interfaces;

public interface INoteService
{
    Task<IEnumerable<NoteDTO>> GetAllAsync();
    Task<NoteDTO> GetByIdAsync(Guid id);
    Task<NoteDTO> AddAsync(NoteDTO note);
    Task UpdateAsync(NoteDTO note);
    Task DeleteAsync(Guid id);
}
