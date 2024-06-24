using Microsoft.AspNetCore.Mvc;
using CRM.WebApp.Ingresso.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using CRM.Application.DTOs;

namespace CRM.WebApp.Ingresso.Controllers;
public class CartController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private static List<ProductViewModel> Cart = new List<ProductViewModel>();

    public CartController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View(Cart);
    }

    [HttpPost]
    public async Task<IActionResult> Add(Guid id)
    {
        var client = _httpClientFactory.CreateClient("CRM.API");
        var response = await client.GetAsync($"api/product/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var product = await response.Content.ReadFromJsonAsync<ProductViewModel>();
        Cart.Add(product);

        // Obter o ID do usuário logado
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        // Criar oportunidade no CRM
        var opportunity = new OpportunityDTO
        {
            CustomerID = Guid.Parse(userId), // ID do usuário logado
            Name = "Nova Oportunidade",
            // Outros campos necessários
        };

        response = await client.PostAsJsonAsync("api/opportunity", opportunity);
        response.EnsureSuccessStatusCode();

        var opportunityId = await response.Content.ReadFromJsonAsync<Guid>();

        // Criar cotação no CRM
        var quote = new QuoteDTO
        {
            OpportunityID = opportunityId,
            ProductID = product.ProductID,
            // Outros campos necessários
        };

        response = await client.PostAsJsonAsync("api/quote", quote);
        response.EnsureSuccessStatusCode();

        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Remove(Guid id)
    {
        var product = Cart.Find(p => p.ProductID == id);
        if (product != null)
        {
            Cart.Remove(product);

            // Verificar se o carrinho está vazio
            if (Cart.Count == 0)
            {
                var client = _httpClientFactory.CreateClient("CRM.API");

                // Obter o ID da oportunidade (deve ser armazenado em algum lugar, como em sessão)
                var opportunityId = GetOpportunityId();

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
        Cart.Clear();

        var client = _httpClientFactory.CreateClient("CRM.API");

        // Obter o ID da oportunidade (deve ser armazenado em algum lugar, como em sessão)
        var opportunityId = GetOpportunityId();

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

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Checkout()
    {
        var client = _httpClientFactory.CreateClient("CRM.API");

        // Obter o ID da oportunidade (deve ser armazenado em algum lugar, como em sessão)
        var opportunityId = GetOpportunityId();

        // Criar pedido no CRM
        var order = new OrderDTO
        {
            OpportunityID = opportunityId,
            OrderDate = DateTime.Now,
            // Outros campos necessários
        };

        var response = await client.PostAsJsonAsync("api/order", order);
        response.EnsureSuccessStatusCode();

        var orderId = await response.Content.ReadFromJsonAsync<Guid>();

        // Criar itens do pedido no CRM
        foreach (var item in Cart)
        {
            var orderItem = new OrderItemDTO
            {
                OrderID = orderId,
                ProductID = item.ProductID,
                Quantity = 1, // Ajustar conforme necessário
                UnitPrice = item.Price.Value,
                // Outros campos necessários
            };

            response = await client.PostAsJsonAsync("api/orderitem", orderItem);
            response.EnsureSuccessStatusCode();
        }

        // Limpar carrinho após finalizar compra
        Cart.Clear();

        return RedirectToAction("OrderConfirmation");
    }

    public IActionResult OrderConfirmation()
    {
        return View();
    }

    private Guid GetOpportunityId()
    {
        // Implementar a lógica para obter o ID da oportunidade
        // Pode ser armazenado em sessão, banco de dados, etc.
        return Guid.NewGuid(); // Exemplo de retorno
    }
}