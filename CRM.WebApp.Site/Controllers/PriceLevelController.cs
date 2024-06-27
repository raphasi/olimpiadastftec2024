using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CRM.WebApp.Site.Models;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Authorization;

namespace CRM.WebApp.Site.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class PriceLevelController : BaseController<PriceLevelViewModel, PriceLevelViewModel>
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PriceLevelController(IHttpClientFactory httpClientFactory) : base(httpClientFactory, "pricelevel")
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: PriceLevels
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            var response = await client.GetAsync("api/pricelevel");
            response.EnsureSuccessStatusCode();

            var priceLevels = await response.Content.ReadFromJsonAsync<IEnumerable<PriceLevelViewModel>>();
            return View(priceLevels);
        }

        // GET: PriceLevels/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            var response = await client.GetAsync($"api/pricelevel/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var priceLevel = await response.Content.ReadFromJsonAsync<PriceLevelViewModel>();
            return View(priceLevel);
        }

        // GET: PriceLevels/Create
        public IActionResult Create()
        {
            var model = InitializeEntity();
            return View(model);
        }

        // POST: PriceLevels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PriceLevelViewModel priceLevelViewModel)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("CRM.API");
                var response = await client.PostAsJsonAsync("api/pricelevel", priceLevelViewModel);
                response.EnsureSuccessStatusCode();

                return RedirectToAction(nameof(Index));
            }
            return View(priceLevelViewModel);
        }

        // GET: PriceLevels/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            var response = await client.GetAsync($"api/pricelevel/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var priceLevel = await response.Content.ReadFromJsonAsync<PriceLevelViewModel>();
            priceLevel.IsNew = false;
            return View(priceLevel);
        }

        // POST: PriceLevels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, PriceLevelViewModel priceLevelViewModel)
        {
            if (id != priceLevelViewModel.PriceLevelID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("CRM.API");
                UpdateEntity(priceLevelViewModel);
                var response = await client.PutAsJsonAsync($"api/pricelevel/{id}", priceLevelViewModel);
                if (!response.IsSuccessStatusCode)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(priceLevelViewModel);
        }

        // GET: PriceLevels/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            var response = await client.GetAsync($"api/pricelevel/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var priceLevel = await response.Content.ReadFromJsonAsync<PriceLevelViewModel>();
            return View(priceLevel);
        }

        // POST: PriceLevels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            var response = await client.DeleteAsync($"api/pricelevel/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}