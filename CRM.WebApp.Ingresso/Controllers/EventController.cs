using Microsoft.AspNetCore.Mvc;
using CRM.WebApp.Ingresso.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CRM.WebApp.Ingresso.Controllers
{
    public class EventController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EventController(IHttpClientFactory httpClientFactory)
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

        // GET: Events
        public async Task<IActionResult> List()
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

            var eventDetails = await response.Content.ReadFromJsonAsync<EventViewModel>();

            // Buscar produtos associados ao evento
            var productsResponse = await client.GetAsync($"api/event/{id}/products");
            if (productsResponse.IsSuccessStatusCode)
            {
                var products = await productsResponse.Content.ReadFromJsonAsync<IEnumerable<ProductViewModel>>();
                eventDetails.AvailableProducts = products != null ? new List<ProductViewModel>(products) : new List<ProductViewModel>();
            }

            return View(eventDetails);
        }
    }
}