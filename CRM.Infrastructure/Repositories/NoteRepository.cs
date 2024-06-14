using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using CRM.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly ApplicationDbContext _noteContext;

        public NoteRepository(ApplicationDbContext noteContext)
        {
            _noteContext = noteContext;
        }

        public async Task<Note> GetNoteByIdAsync(Guid noteId)
        {
            return await _noteContext.Set<Note>().FindAsync(noteId);
        }

        public async Task<IEnumerable<Note>> GetAllNotesAsync()
        {
            return await _noteContext.Set<Note>().ToListAsync();
        }

        public async Task AddNoteAsync(Note noteEntity)
        {
            await _noteContext.Set<Note>().AddAsync(noteEntity);
            await _noteContext.SaveChangesAsync();
        }

        public async Task UpdateNoteAsync(Note noteEntity)
        {
            _noteContext.Set<Note>().Update(noteEntity);
            await _noteContext.SaveChangesAsync();
        }

        public async Task DeleteNoteAsync(Guid noteId)
        {
            var note = await _noteContext.Set<Note>().FindAsync(noteId);
            if (note != null)
            {
                _noteContext.Set<Note>().Remove(note);
                await _noteContext.SaveChangesAsync();
            }
        }
    }
}