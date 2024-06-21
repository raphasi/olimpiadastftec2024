using Microsoft.AspNetCore.Mvc;
using CRM.WebApp.Site.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CRM.WebApp.Site.Controllers
{
    public class ActivityController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ActivityController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: Activities
        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            var response = await client.GetAsync("api/activity");
            response.EnsureSuccessStatusCode();

            var activities = await response.Content.ReadFromJsonAsync<IEnumerable<ActivityViewModel>>();
            return View(activities);
        }

        // GET: Activities/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            var response = await client.GetAsync($"api/activity/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var activity = await response.Content.ReadFromJsonAsync<ActivityViewModel>();
            return View(activity);
        }

        // GET: Activities/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Activities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ActivityViewModel activityViewModel)
        {
            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("CRM.API");
                var response = await client.PostAsJsonAsync("api/activity", activityViewModel);
                response.EnsureSuccessStatusCode();

                return RedirectToAction(nameof(Index));
            }
            return View(activityViewModel);
        }

        // GET: Activities/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            var response = await client.GetAsync($"api/activity/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var activity = await response.Content.ReadFromJsonAsync<ActivityViewModel>();
            return View(activity);
        }

        // POST: Activities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ActivityViewModel activityViewModel)
        {
            if (id != activityViewModel.ActivityID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var client = _httpClientFactory.CreateClient("CRM.API");
                var response = await client.PutAsJsonAsync($"api/activity/{id}", activityViewModel);
                if (!response.IsSuccessStatusCode)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(activityViewModel);
        }

        // GET: Activities/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            var response = await client.GetAsync($"api/activity/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var activity = await response.Content.ReadFromJsonAsync<ActivityViewModel>();
            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            var response = await client.DeleteAsync($"api/activity/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}