using Microsoft.AspNetCore.Mvc;
using CRM.WebApp.Site.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CRM.WebApp.Site.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class QuoteController : BaseController<QuoteViewModel, QuoteViewModel>
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public QuoteController(IHttpClientFactory httpClientFactory) : base(httpClientFactory, "quote")
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: Quotes
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            PutTokenInHeaderAuthorization(GetAccessToken(), client);
            var response = await client.GetAsync("api/quote");
            response.EnsureSuccessStatusCode();

            var quotes = await response.Content.ReadFromJsonAsync<IEnumerable<QuoteViewModel>>();
            return View(quotes);
        }

        // GET: Quotes/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            PutTokenInHeaderAuthorization(GetAccessToken(), client);
            var response = await client.GetAsync($"api/quote/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var quote = await response.Content.ReadFromJsonAsync<QuoteViewModel>();
            return View(quote);
        }

        // GET: Quotes/Create
        public IActionResult Create()
        {
            var model = InitializeEntity();
            return View(model);
        }

        // POST: Quotes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(QuoteViewModel quoteViewModel)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("CRM.API");
                PutTokenInHeaderAuthorization(GetAccessToken(), client);
                var response = await client.PostAsJsonAsync("api/quote", quoteViewModel);
                response.EnsureSuccessStatusCode();

                return RedirectToAction(nameof(Index));
            }
            return View(quoteViewModel);
        }

        // GET: Quotes/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            PutTokenInHeaderAuthorization(GetAccessToken(), client);
            var response = await client.GetAsync($"api/quote/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var quote = await response.Content.ReadFromJsonAsync<QuoteViewModel>();
            quote.IsNew = false;
            return View(quote);
        }

        // POST: Quotes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, QuoteViewModel quoteViewModel)
        {
            if (id != quoteViewModel.QuoteID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("CRM.API");
                PutTokenInHeaderAuthorization(GetAccessToken(), client);
                UpdateEntity(quoteViewModel);
                var response = await client.PutAsJsonAsync($"api/quote/{id}", quoteViewModel);
                if (!response.IsSuccessStatusCode)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(quoteViewModel);
        }

        // GET: Quotes/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            PutTokenInHeaderAuthorization(GetAccessToken(), client);
            var response = await client.GetAsync($"api/quote/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var quote = await response.Content.ReadFromJsonAsync<QuoteViewModel>();
            return View(quote);
        }

        // POST: Quotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            PutTokenInHeaderAuthorization(GetAccessToken(), client);
            var response = await client.DeleteAsync($"api/quote/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}