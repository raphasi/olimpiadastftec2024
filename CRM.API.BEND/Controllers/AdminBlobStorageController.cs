using CRM.Application.Interfaces;
using CRM.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CRM.API.BEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AdminBlobStorageController : ControllerBase
    {
        private readonly IBlobStorageService _blobStorageService;
        private readonly ILogger<AdminBlobStorageController> _logger;

        public AdminBlobStorageController(IBlobStorageService blobStorageService, ILogger<AdminBlobStorageController> logger)
        {
            _blobStorageService = blobStorageService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Uri>), 200)]
        public async Task<ActionResult<IEnumerable<Uri>>> GetAllBlobs()
        {
            try
            {
                var blobs = await _blobStorageService.ListBlobsAsync();
                return Ok(blobs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os blobs.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPost("upload")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> UploadFiles([FromForm] List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("Arquivo(s) não selecionado(s).");
            }

            if (files.Count > 10)
            {
                return BadRequest("Quantidade de arquivos excedeu o limite de 10 por vez.");
            }

            try
            {
                foreach (var formFile in files)
                {
                    if (formFile.FileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                        formFile.FileName.EndsWith(".gif", StringComparison.OrdinalIgnoreCase) ||
                        formFile.FileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                    {
                        var newFileName = $"{Guid.NewGuid()}{Path.GetExtension(formFile.FileName)}";
                        using (var stream = formFile.OpenReadStream())
                        {
                            await _blobStorageService.UploadBlobAsync(newFileName, stream);
                        }
                    }
                }

                return CreatedAtAction(nameof(GetAllBlobs), null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao fazer upload dos arquivos.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpDelete("{uri}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> DeleteBlob(string uri)
        {
            try
            {
                var blobName = Path.GetFileName(new Uri(uri).LocalPath);
                await _blobStorageService.DeleteBlobAsync(blobName);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar blob.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [HttpPost("delete-all")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> DeleteAllBlobs()
        {
            try
            {
                await _blobStorageService.DeleteAllBlobsAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar todos os blobs.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }



    }
}