using AutoMapper;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Application.DTOs;
using CRM.Domain.Interfaces;

namespace CRM.Application.Services;

public class NoteService : INoteService
{
    private readonly INoteRepository _noteRepository;
    private readonly IMapper _mapper;

    public NoteService(INoteRepository noteRepository, IMapper mapper)
    {
        _noteRepository = noteRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<NoteDTO>> GetAllAsync()
    {
        var notes = await _noteRepository.GetAllNotesAsync();
        return _mapper.Map<IEnumerable<NoteDTO>>(notes);
    }

    public async Task<NoteDTO> GetByIdAsync(Guid id)
    {
        var note = await _noteRepository.GetNoteByIdAsync(id);
        return _mapper.Map<NoteDTO>(note);
    }

    public async Task<NoteDTO> AddAsync(NoteDTO note)
    {
        note.NoteID = Guid.NewGuid();
        var noteEntity = _mapper.Map<Note>(note);
        await _noteRepository.AddNoteAsync(noteEntity);
        return note;
    }

    public async Task UpdateAsync(NoteDTO note)
    {
        var noteEntity = _mapper.Map<Note>(note);
        await _noteRepository.UpdateNoteAsync(noteEntity);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _noteRepository.DeleteNoteAsync(id);
    }
}