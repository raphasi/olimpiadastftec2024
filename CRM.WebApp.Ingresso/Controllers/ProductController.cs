using Microsoft.AspNetCore.Mvc;
using CRM.WebApp.Ingresso.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CRM.WebApp.Ingresso.Controllers;

public class ProductController : BaseController<ProductViewModel, ProductViewModel>
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductController(IHttpClientFactory httpClientFactory) : base(httpClientFactory, "product")
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("CRM.API");
        PutTokenInHeaderAuthorization(GetAccessToken(), client);
        var response = await client.GetAsync("api/product");
        response.EnsureSuccessStatusCode();

        var products = await response.Content.ReadFromJsonAsync<IEnumerable<ProductViewModel>>();
        return View(products);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var client = _httpClientFactory.CreateClient("CRM.API");
        PutTokenInHeaderAuthorization(GetAccessToken(), client);
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
        PutTokenInHeaderAuthorization(GetAccessToken(), client);
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