using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CRM.WebApp.Site.Models;
using Microsoft.AspNetCore.Authorization;

namespace CRM.WebApp.Site.Controllers;
[Authorize(Policy = "AdminOnly")]
public class NoteController : BaseController<NoteViewModel, NoteViewModel>
{
    private readonly IHttpClientFactory _httpClientFactory;

    public NoteController(IHttpClientFactory httpClientFactory) : base(httpClientFactory, "note")
    {
        _httpClientFactory = httpClientFactory;
    }

    // GET: Notes
    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient("CRM.API");
        PutTokenInHeaderAuthorization(GetAccessToken(), client);
        var response = await client.GetAsync("api/note");
        response.EnsureSuccessStatusCode();

        var notes = await response.Content.ReadFromJsonAsync<IEnumerable<NoteViewModel>>();
        return View(notes);
    }

    // GET: Notes/Details/5
    public async Task<IActionResult> Details(Guid id)
    {
        var client = _httpClientFactory.CreateClient("CRM.API");
        PutTokenInHeaderAuthorization(GetAccessToken(), client);
        var response = await client.GetAsync($"api/note/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var note = await response.Content.ReadFromJsonAsync<NoteViewModel>();
        return View(note);
    }

    // GET: Notes/Create
    public IActionResult Create()
    {
        var model = InitializeEntity();
        return View(model);
    }

    // POST: Notes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NoteViewModel noteViewModel)
    {
        if (ModelState.IsValid)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            PutTokenInHeaderAuthorization(GetAccessToken(), client);
            var response = await client.PostAsJsonAsync("api/note", noteViewModel);
            response.EnsureSuccessStatusCode();

            return RedirectToAction(nameof(Index));
        }
        return View(noteViewModel);
    }

    // GET: Notes/Edit/5
    public async Task<IActionResult> Edit(Guid id)
    {
        var client = _httpClientFactory.CreateClient("CRM.API");
        PutTokenInHeaderAuthorization(GetAccessToken(), client);
        var response = await client.GetAsync($"api/note/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var note = await response.Content.ReadFromJsonAsync<NoteViewModel>();
        note.IsNew = false;
        return View(note);
    }

    // POST: Notes/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, NoteViewModel noteViewModel)
    {
        if (id != noteViewModel.NoteID)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            PutTokenInHeaderAuthorization(GetAccessToken(), client);
            UpdateEntity(noteViewModel);
            var response = await client.PutAsJsonAsync($"api/note/{id}", noteViewModel);
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
        return View(noteViewModel);
    }

    // GET: Notes/Delete/5
    public async Task<IActionResult> Delete(Guid id)
    {
        var client = _httpClientFactory.CreateClient("CRM.API");
        PutTokenInHeaderAuthorization(GetAccessToken(), client);
        var response = await client.GetAsync($"api/note/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var note = await response.Content.ReadFromJsonAsync<NoteViewModel>();
        return View(note);
    }

    // POST: Notes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var client = _httpClientFactory.CreateClient("CRM.API");
        PutTokenInHeaderAuthorization(GetAccessToken(), client);
        var response = await client.DeleteAsync($"api/note/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }
}