using Microsoft.AspNetCore.Mvc;
using CRM.WebApp.Site.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace CRM.WebApp.Site.Controllers;

[Authorize(Policy = "AdminOnly")]
public class OpportunityController : BaseController<OpportunityViewModel, OpportunityViewModel>
{
    private readonly IHttpClientFactory _httpClientFactory;

    public OpportunityController(IHttpClientFactory httpClientFactory) : base(httpClientFactory, "opportunity")
    {
        _httpClientFactory = httpClientFactory;
    }

    // GET: Opportunities
    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("CRM.API");
        PutTokenInHeaderAuthorization(GetAccessToken(), client);
        var response = await client.GetAsync("api/opportunity");
        response.EnsureSuccessStatusCode();

        var opportunities = await response.Content.ReadFromJsonAsync<IEnumerable<OpportunityViewModel>>();
        return View(opportunities);
    }

    // GET: Opportunities/Details/5
    public async Task<IActionResult> Details(Guid id)
    {
        var client = _httpClientFactory.CreateClient("CRM.API");
        PutTokenInHeaderAuthorization(GetAccessToken(), client);
        var response = await client.GetAsync($"api/opportunity/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var opportunity = await response.Content.ReadFromJsonAsync<OpportunityViewModel>();
        return View(opportunity);
    }

    // GET: Opportunities/Create
    public IActionResult Create()
    {
        var model = InitializeEntity();
        return View(model);
    }

    // POST: Opportunities/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(OpportunityViewModel opportunityViewModel)
    {
        if (ModelState.IsValid)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            PutTokenInHeaderAuthorization(GetAccessToken(), client);
            var response = await client.PostAsJsonAsync("api/opportunity", opportunityViewModel);
            response.EnsureSuccessStatusCode();

            return RedirectToAction(nameof(Index));
        }
        return View(opportunityViewModel);
    }

    // GET: Opportunities/Edit/5
    public async Task<IActionResult> Edit(Guid id)
    {
        var client = _httpClientFactory.CreateClient("CRM.API");
        PutTokenInHeaderAuthorization(GetAccessToken(), client);
        var response = await client.GetAsync($"api/opportunity/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var opportunity = await response.Content.ReadFromJsonAsync<OpportunityViewModel>();
        opportunity.IsNew = false;
        return View(opportunity);
    }

    // POST: Opportunities/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, OpportunityViewModel opportunityViewModel)
    {
        if (id != opportunityViewModel.OpportunityID)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            PutTokenInHeaderAuthorization(GetAccessToken(), client);
            UpdateEntity(opportunityViewModel);
            var response = await client.PutAsJsonAsync($"api/opportunity/{id}", opportunityViewModel);
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
        return View(opportunityViewModel);
    }

    // GET: Opportunities/Delete/5
    public async Task<IActionResult> Delete(Guid id)
    {
        var client = _httpClientFactory.CreateClient("CRM.API");
        PutTokenInHeaderAuthorization(GetAccessToken(), client);
        var response = await client.GetAsync($"api/opportunity/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var opportunity = await response.Content.ReadFromJsonAsync<OpportunityViewModel>();
        return View(opportunity);
    }

    // POST: Opportunities/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var client = _httpClientFactory.CreateClient("CRM.API");
        PutTokenInHeaderAuthorization(GetAccessToken(), client);
        var response = await client.DeleteAsync($"api/opportunity/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }
}