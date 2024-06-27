using Microsoft.AspNetCore.Mvc;
using CRM.WebApp.Site.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CRM.WebApp.Site.Models;
using Microsoft.AspNetCore.Authorization;

namespace CRM.WebApp.Site.Controllers;
[Authorize(Policy = "AdminOnly")]
public class ProductController : BaseController<ProductViewModel, ProductViewModel>
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductController(IHttpClientFactory httpClientFactory) : base(httpClientFactory, "product")
    {
        _httpClientFactory = httpClientFactory;
    }

    // GET: Products
    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("CRM.API");
        var response = await client.GetAsync("api/product");
        response.EnsureSuccessStatusCode();

        var products = await response.Content.ReadFromJsonAsync<IEnumerable<ProductViewModel>>();
        return View(products);
    }

    // GET: Products/Details/5
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

    // GET: Products/Create
    public async Task<IActionResult> Create()
    {
        var model = InitializeEntity();
        var client = _httpClientFactory.CreateClient("CRM.API");
        var response = await client.GetAsync("api/event");
        if (response.IsSuccessStatusCode)
        {
            var events = await response.Content.ReadFromJsonAsync<IEnumerable<EventViewModel>>();
            model.AvailableEvents = events != null ? new List<EventViewModel>(events) : new List<EventViewModel>();
        }
        return View(model);
    }

    // POST: Products/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductViewModel productViewModel)
    {
        if (ModelState.IsValid)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            var response = await client.PostAsJsonAsync("api/product", productViewModel);
            response.EnsureSuccessStatusCode();

            // Associar o produto aos eventos selecionados
            foreach (var eventId in productViewModel.SelectedEventIds)
            {
                var productEvent = new ProductEventViewModel
                {
                    ProductID = productViewModel.ProductID,
                    EventID = eventId
                };
                await client.PostAsJsonAsync("api/productevent", productEvent);
            }

            return RedirectToAction(nameof(Index));
        }
        return View(productViewModel);
    }

    // GET: Products/Edit/5
    public async Task<IActionResult> Edit(Guid id)
    {
        var client = _httpClientFactory.CreateClient("CRM.API");
        var response = await client.GetAsync($"api/product/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var product = await response.Content.ReadFromJsonAsync<ProductViewModel>();
        product.IsNew = false;

        // Buscar eventos disponíveis
        var eventsResponse = await client.GetAsync("api/event");
        if (eventsResponse.IsSuccessStatusCode)
        {
            var events = await eventsResponse.Content.ReadFromJsonAsync<IEnumerable<EventViewModel>>();
            product.AvailableEvents = events != null ? new List<EventViewModel>(events) : new List<EventViewModel>();
        }

        // Buscar eventos associados ao produto
        var productEventsResponse = await client.GetAsync($"api/productevent/product/{id}");
        if (productEventsResponse.IsSuccessStatusCode)
        {
            var productEvents = await productEventsResponse.Content.ReadFromJsonAsync<IEnumerable<ProductEventViewModel>>();
            product.SelectedEventIds = productEvents != null ? new List<Guid>(productEvents.Select(pe => pe.EventID)) : new List<Guid>();
        }

        return View(product);
    }

    // POST: Products/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, ProductViewModel productViewModel)
    {
        if (id != productViewModel.ProductID)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            UpdateEntity(productViewModel);
            var response = await client.PutAsJsonAsync($"api/product/{id}", productViewModel);
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            // Atualizar associações de eventos
            var existingProductEventsResponse = await client.GetAsync($"api/productevent/product/{id}");
            if (existingProductEventsResponse.IsSuccessStatusCode)
            {
                var existingProductEvents = await existingProductEventsResponse.Content.ReadFromJsonAsync<IEnumerable<ProductEventViewModel>>();
                var existingEventIds = existingProductEvents.Select(pe => pe.EventID).ToList();

                // Adicionar novas associações
                foreach (var eventId in productViewModel.SelectedEventIds.Except(existingEventIds))
                {
                    var productEvent = new ProductEventViewModel
                    {
                        ProductID = productViewModel.ProductID,
                        EventID = eventId
                    };
                    await client.PostAsJsonAsync("api/productevent", productEvent);
                }

                // Remover associações antigas
                foreach (var eventId in existingEventIds.Except(productViewModel.SelectedEventIds))
                {
                    var productEvent = existingProductEvents.First(pe => pe.EventID == eventId);
                    await client.DeleteAsync($"api/productevent/{productEvent.ProductID}");
                }
            }

            return RedirectToAction(nameof(Index));
        }
        return View(productViewModel);
    }

    // GET: Products/Delete/5
    public async Task<IActionResult> Delete(Guid id)
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

    // POST: Products/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var client = _httpClientFactory.CreateClient("CRM.API");
        var response = await client.DeleteAsync($"api/product/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }
}