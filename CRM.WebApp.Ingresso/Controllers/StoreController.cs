using CRM.WebApp.Ingresso.Models;
using Microsoft.AspNetCore.Mvc;

namespace CRM.WebApp.Ingresso.Controllers;

public class StoreController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public StoreController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("CRM.API");
        var response = await client.GetAsync("api/product");
        response.EnsureSuccessStatusCode();

        var products = await response.Content.ReadFromJsonAsync<IEnumerable<ProductViewModel>>();
        return View(products);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var client = _httpClientFactory.CreateClient("CRM.API");
        var response = await client.GetAsync($"api/product/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var product = await response.Content.ReadFromJsonAsync<ProductViewModel>();
        return View(product);
    }

    public async Task<IActionResult> AddToCart(Guid id)
    {
        var client = _httpClientFactory.CreateClient("CRM.API");
        var response = await client.GetAsync($"api/product/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var product = await response.Content.ReadFromJsonAsync<ProductViewModel>();
        // Adicionar lógica para adicionar o produto ao carrinho
        // ...

        return RedirectToAction("Index", "Cart");
    }
}