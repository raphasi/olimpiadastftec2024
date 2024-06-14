using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Domain.Interfaces;

public interface INoteRepository
{
    Task<Note> GetNoteByIdAsync(Guid noteId);
    Task<IEnumerable<Note>> GetAllNotesAsync();
    Task AddNoteAsync(Note noteEntity);
    Task UpdateNoteAsync(Note noteEntity);
    Task DeleteNoteAsync(Guid noteId);
}