using Microsoft.AspNetCore.Mvc;
using CRM.WebApp.Ingresso.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using CRM.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace CRM.WebApp.Ingresso.Controllers
{
    public class CartController : BaseController<CartDTO, CartDTO>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AccountController> _logger;

        public CartController(IHttpClientFactory httpClientFactory, ILogger<AccountController> logger) : base(httpClientFactory, "cart")
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [Authorize]
        public IActionResult Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<ProductViewModel>>("Cart") ?? new List<ProductViewModel>();
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Guid id)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            PutTokenInHeaderAuthorization(GetAccessToken(), client);
            var userInfo = GetUserInfo();
            var response = await client.GetAsync($"api/product/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var product = await response.Content.ReadFromJsonAsync<ProductViewModel>();
            var cart = HttpContext.Session.GetObjectFromJson<List<ProductViewModel>>("Cart") ?? new List<ProductViewModel>();
            cart.Add(product);
            HttpContext.Session.SetObjectAsJson("Cart", cart);

            // Obter o ID do usuário logado
            var user = GetUserInfo();
            var userId = user.id ?? throw new InvalidOperationException("User ID cannot be null");

            // Criar oportunidade no CRM
            var opportunity = new OpportunityDTO
            {
                LeadID = user.leadID,
                Name = "Oportunidade - " + user.userName,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                CreatedBy = new Guid(userId),
                ModifiedBy = new Guid(userId),
            };

            response = await client.PostAsJsonAsync("api/opportunity", opportunity);
            response.EnsureSuccessStatusCode();

            var opportunityId = await response.Content.ReadFromJsonAsync<Guid>();

            // Armazenar o ID da oportunidade na sessão
            HttpContext.Session.SetString("opportunity_id", opportunityId.ToString());

            // Criar cotação no CRM
            var quote = new QuoteDTO
            {
                OpportunityID = opportunityId,
                ProductID = product.ProductID,
                Name = "Cotação - " + user.userName,
                LeadID = user.leadID,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                CreatedBy = new Guid(userId),
                ModifiedBy = new Guid(userId),
            };

            response = await client.PostAsJsonAsync("api/quote", quote);
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Guid id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<ProductViewModel>>("Cart") ?? new List<ProductViewModel>();
            var product = cart.Find(p => p.ProductID == id);
            if (product != null)
            {
                cart.Remove(product);
                HttpContext.Session.SetObjectAsJson("Cart", cart);

                // Verificar se o carrinho está vazio
                if (cart.Count == 0)
                {
                    var client = _httpClientFactory.CreateClient("CRM.API");
                    PutTokenInHeaderAuthorization(GetAccessToken(), client);
                    // Obter o ID da oportunidade
                    var opportunityId = Guid.Parse(HttpContext.Session.GetString("opportunity_id"));

                    // Atualizar status da oportunidade para perdida
                    var opportunityUpdate = new OpportunityDTO
                    {
                        StatusCode = 5
                    };

                    var response = await client.PutAsJsonAsync($"api/opportunity/{opportunityId}", opportunityUpdate);
                    response.EnsureSuccessStatusCode();

                    // Cancelar cotações
                    var quotes = await client.GetFromJsonAsync<IEnumerable<QuoteDTO>>($"api/opportunity/{opportunityId}/quotes");
                    foreach (var quote in quotes)
                    {
                        var quoteUpdate = new QuoteDTO
                        {
                            StatusCode = 5
                        };

                        response = await client.PutAsJsonAsync($"api/quote/{quote.QuoteID}", quoteUpdate);
                        response.EnsureSuccessStatusCode();
                    }
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            var cart = new List<ProductViewModel>();
            HttpContext.Session.SetObjectAsJson("Cart", cart);

            var client = _httpClientFactory.CreateClient("CRM.API");
            PutTokenInHeaderAuthorization(GetAccessToken(), client);

            // Obter o ID da oportunidade
            var opportunityId = Guid.Parse(HttpContext.Session.GetString("opportunity_id"));

            // Atualizar status da oportunidade para perdida
            var opportunityUpdate = new UpdateFieldDTO
            {
                FieldName = "StatusCode",
                FieldValue = JsonDocument.Parse("5").RootElement
            };

            // opp perdida
            var response = await client.PatchAsync($"api/opportunity/{opportunityId}/update-field", JsonContent.Create(opportunityUpdate));
            response.EnsureSuccessStatusCode();

            // Obter as cotações vinculadas à oportunidade
            response = await client.GetAsync($"api/quote/{opportunityId}/quotesopp");
            response.EnsureSuccessStatusCode();
            var quotes = await response.Content.ReadFromJsonAsync<IEnumerable<QuoteDTO>>();
            foreach (var quote in quotes)
            {
                var quoteUpdate = new UpdateFieldDTO
                {
                    FieldName = "StatusCode",
                    FieldValue = JsonDocument.Parse("5").RootElement
                };

                response = await client.PatchAsync($"api/quote/{quote.QuoteID}/update-field", JsonContent.Create(quoteUpdate));
                response.EnsureSuccessStatusCode();
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<ProductViewModel>>("Cart");
            var opportunityId = HttpContext.Session.GetString("opportunity_id");

            if (cart == null || !cart.Any() || string.IsNullOrEmpty(opportunityId))
            {
                return RedirectToAction("Index", "Cart");
            }

            var client = _httpClientFactory.CreateClient("CRM.API");
            PutTokenInHeaderAuthorization(GetAccessToken(), client);

            // Obter a oportunidade
            var response = await client.GetAsync($"api/opportunity/{opportunityId}");
            response.EnsureSuccessStatusCode();
            var opportunity = await response.Content.ReadFromJsonAsync<OpportunityDTO>();

            // Obter as cotações vinculadas à oportunidade
            response = await client.GetAsync($"api/quote/{opportunityId}/quotesopp");
            response.EnsureSuccessStatusCode();
            var quotes = await response.Content.ReadFromJsonAsync<IEnumerable<QuoteDTO>>();

            // Obter as informações do lead
            response = await client.GetAsync($"api/lead/{opportunity.LeadID}");
            response.EnsureSuccessStatusCode();
            var lead = await response.Content.ReadFromJsonAsync<LeadDTO>();

            var checkoutViewModel = new CheckoutViewModel
            {
                CartItems = cart,
                Opportunity = opportunity,
                Quotes = quotes,
                Lead = lead
            };

            return View(checkoutViewModel);
        }
        public IActionResult OrderConfirmation()
        {
            return View();
        }
    }
}