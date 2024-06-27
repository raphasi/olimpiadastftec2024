using CRM.Application.DTOs;
using CRM.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.API.BEND.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INoteService _noteService;
        private readonly ILogger<NoteController> _logger;

        public NoteController(INoteService noteService, ILogger<NoteController> logger)
        {
            _noteService = noteService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NoteDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<NoteDTO>> GetNoteById(Guid id)
        {
            try
            {
                var note = await _noteService.GetByIdAsync(id);
                if (note == null)
                {
                    _logger.LogWarning("Nota com ID {NoteId} não encontrada.", id);
                    return NotFound();
                }
                return Ok(note);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter nota por ID.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<NoteDTO>), 200)]
        public async Task<ActionResult<IEnumerable<NoteDTO>>> GetAllNotes()
        {
            try
            {
                var notes = await _noteService.GetAllAsync();
                return Ok(notes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todas as notas.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> AddNote([FromBody] NoteDTO note)
        {
            if (note == null)
            {
                return BadRequest("Dados da nota são obrigatórios.");
            }

            try
            {
                await _noteService.AddAsync(note);
                return CreatedAtAction(nameof(GetNoteById), new { id = note.NoteID }, note);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar nota.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> UpdateNote(Guid id, [FromBody] NoteDTO note)
        {
            if (note == null || note.NoteID != id)
            {
                return BadRequest("Dados da nota são inválidos.");
            }

            try
            {
                await _noteService.UpdateAsync(note);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar nota.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteNote(Guid id)
        {
            try
            {
                var existingNote = await _noteService.GetByIdAsync(id);
                if (existingNote == null)
                {
                    return NotFound();
                }

                await _noteService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar nota.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }
    }
}