using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CRM.API.BEND.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountController : ControllerBase
    {
        private readonly IGenericService _genericService;
        private readonly ILogger<CountController> _logger;

        public CountController(IGenericService genericService, ILogger<CountController> logger)
        {
            _genericService = genericService;
            _logger = logger;
        }

        [HttpGet("{entityName}")]
        public async Task<ActionResult<int>> GetCount(string entityName)
        {
            try
            {
                int count;
                switch (entityName.ToLower())
                {
                    case "customer":
                        count = await _genericService.GetCountAsync<Customer>();
                        break;
                    case "opportunity":
                        count = await _genericService.GetCountAsync<Opportunity>();
                        break;
                    case "lead":
                        count = await _genericService.GetCountAsync<Lead>();
                        break;
                    case "quotes":
                        count = await _genericService.GetCountAsync<Quote>();
                        break;
                    default:
                        return BadRequest("Entidade desconhecida.");
                }
                return Ok(count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao obter contagem para a entidade {entityName}.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }
    }
}