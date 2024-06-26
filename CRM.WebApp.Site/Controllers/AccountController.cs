using CRM.Application.DTOs;
using CRM.WebApp.Site.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CRM.WebApp.Site.Controllers
{
    public class AccountController : BaseController<RegisterViewModel, LoginViewModel>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IHttpClientFactory httpClientFactory, ILogger<AccountController> logger) : base(httpClientFactory, "identity")
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        // GET: Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (model == null)
            {
                return BadRequest("Dados de registro são obrigatórios.");
            }

            try
            {
                var client = _httpClientFactory.CreateClient("CRM.API");
                var response = await client.PostAsJsonAsync("api/auth/register", model);

                if (response.IsSuccessStatusCode)
                {
                    return StatusCode(201, "Usuário registrado com sucesso.");
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("Erro ao registrar usuário: {Error}", errorResponse);
                    return BadRequest(errorResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao registrar usuário.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (model == null)
            {
                return BadRequest("Dados de login são obrigatórios.");
            }

            try
            {
                var client = _httpClientFactory.CreateClient("CRM.API");
                var response = await client.PostAsJsonAsync("api/auth/login", model);

                if (response.IsSuccessStatusCode)
                {
                    var loginContent = await response.Content.ReadAsStringAsync();
                    var loginResult = JsonSerializer.Deserialize<TokenViewModel>(loginContent);

                    // Armazenar o token no HttpContext
                    //HttpContext.Response.Cookies.Append("access_token", loginResult.Result.);

                    return Ok(new { message = "Login realizado com sucesso." });
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    _logger.LogWarning("Erro ao fazer login: {Error}", errorResponse);
                    return Unauthorized(errorResponse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao fazer login.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        [ProducesResponseType(204)]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("access_token");
            return NoContent();
        }
    }
}