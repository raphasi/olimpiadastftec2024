using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CRM.WebApp.Site.Models;

namespace CRM.WebApp.Site.Controllers
{
    public class EventController : BaseController<EventViewModel, EventViewModel>
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EventController(IHttpClientFactory httpClientFactory) : base(httpClientFactory, "event")
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            var response = await client.GetAsync("api/event");
            response.EnsureSuccessStatusCode();

            var events = await response.Content.ReadFromJsonAsync<IEnumerable<EventViewModel>>();
            return View(events);
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            var response = await client.GetAsync($"api/event/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var eventItem = await response.Content.ReadFromJsonAsync<EventViewModel>();
            return View(eventItem);
        }

        // GET: Events/Create
        public async Task<IActionResult> Create()
        {
            var model = InitializeEntity();
            await LoadAvailableProducts(model);
            return View(model);
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EventViewModel eventViewModel)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("CRM.API");
                var response = await client.PostAsJsonAsync("api/event", eventViewModel);
                response.EnsureSuccessStatusCode();

                return RedirectToAction(nameof(Index));
            }
            await LoadAvailableProducts(eventViewModel);
            return View(eventViewModel);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            var response = await client.GetAsync($"api/event/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var eventItem = await response.Content.ReadFromJsonAsync<EventViewModel>();
            eventItem.IsNew = false;
            await LoadAvailableProducts(eventItem);
            return View(eventItem);
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, EventViewModel eventViewModel)
        {
            if (id != eventViewModel.EventID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("CRM.API");
                UpdateEntity(eventViewModel);
                var response = await client.PutAsJsonAsync($"api/event/{id}", eventViewModel);
                if (!response.IsSuccessStatusCode)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }
            await LoadAvailableProducts(eventViewModel);
            return View(eventViewModel);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            var response = await client.GetAsync($"api/event/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var eventItem = await response.Content.ReadFromJsonAsync<EventViewModel>();
            return View(eventItem);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            var response = await client.DeleteAsync($"api/event/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadAvailableProducts(EventViewModel model)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            var response = await client.GetAsync("api/product");
            response.EnsureSuccessStatusCode();

            var products = await response.Content.ReadFromJsonAsync<IEnumerable<ProductViewModel>>();
            model.AvailableProducts = new List<ProductViewModel>(products);
        }
    }
}