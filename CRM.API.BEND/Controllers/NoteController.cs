using CRM.Application.DTOs;
using CRM.Application.Interfaces;
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
        private readonly INoteService _noteService;

        public NoteController(INoteService noteService)
        {
            _noteService = noteService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Note>> GetNoteById(Guid id)
        {
            var note = await _noteService.GetByIdAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NoteDTO>>> GetAllNotes()
        {
            var notes = await _noteService.GetAllAsync();
            return Ok(notes);
        }

        [HttpPost]
        public async Task<ActionResult> AddNote([FromBody] NoteDTO note)
        {
            if (note == null)
            {
                return BadRequest();
            }

            await _noteService.AddAsync(note);
            return CreatedAtAction(nameof(GetNoteById), new { id = note.NoteID }, note);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateNote(Guid id, [FromBody] NoteDTO note)
        {
            if (note == null || note.NoteID != id)
            {
                return BadRequest();
            }

            var existingNote = await _noteService.GetByIdAsync(id);
            if (existingNote == null)
            {
                return NotFound();
            }

            await _noteService.UpdateAsync(note);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteNote(Guid id)
        {
            var existingNote = await _noteService.GetByIdAsync(id);
            if (existingNote == null)
            {
                return NotFound();
            }

            await _noteService.DeleteAsync(id);
            return NoContent();
        }
    }
}