using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.API.BEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteRepository _noteRepository;

        public NoteController(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNoteById(Guid id)
        {
            var note = await _noteRepository.GetNoteByIdAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Note>>> GetAllNotes()
        {
            var notes = await _noteRepository.GetAllNotesAsync();
            return Ok(notes);
        }

        [HttpPost]
        public async Task<ActionResult> AddNote([FromBody] Note note)
        {
            if (note == null)
            {
                return BadRequest();
            }

            await _noteRepository.AddNoteAsync(note);
            return CreatedAtAction(nameof(GetNoteById), new { id = note.NoteID }, note);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateNote(Guid id, [FromBody] Note note)
        {
            if (note == null || note.NoteID != id)
            {
                return BadRequest();
            }

            var existingNote = await _noteRepository.GetNoteByIdAsync(id);
            if (existingNote == null)
            {
                return NotFound();
            }

            await _noteRepository.UpdateNoteAsync(note);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNote(Guid id)
        {
            var existingNote = await _noteRepository.GetNoteByIdAsync(id);
            if (existingNote == null)
            {
                return NotFound();
            }

            await _noteRepository.DeleteNoteAsync(id);
            return NoContent();
        }
    }
}