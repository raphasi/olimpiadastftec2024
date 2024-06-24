using CRM.Domain.Entities;
using CRM.WebApp.Site.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;

public abstract class BaseController<T, TViewModel> : Controller where T : EntityBase, new()
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _entityName;

    protected BaseController(IHttpClientFactory httpClientFactory, string entityName)
    {
        _httpClientFactory = httpClientFactory;
        _entityName = entityName;
    }

    protected T InitializeEntity()
    {
        return new T
        {
            CreatedOn = DateTime.Now,
            CreatedBy = Guid.NewGuid(), // ou obtenha o ID do usuário logado
            ModifiedOn = DateTime.Now,
            ModifiedBy = Guid.NewGuid(), // ou obtenha o ID do usuário logado
            IsNew = true
        };
    }

    protected void UpdateEntity(T entity)
    {
        entity.ModifiedOn = DateTime.Now;
        entity.ModifiedBy = Guid.NewGuid(); // ou obtenha o ID do usuário logado
        entity.IsNew = false;
    }

    public async Task<IActionResult> Search([FromQuery] string query = null)
    {
        var client = _httpClientFactory.CreateClient("CRM.API");
        var response = await client.GetAsync($"/api/{_entityName}/search?query={query}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var entities = JsonConvert.DeserializeObject<IEnumerable<TViewModel>>(content);

        return Ok(entities);
    }

    public async Task<IActionResult> GetById(string id)
    {
        var client = _httpClientFactory.CreateClient("CRM.API");
        var response = await client.GetAsync($"api/{_entityName}/{id}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var entities = JsonConvert.DeserializeObject<TViewModel>(content);

        return Ok(entities);
    }
}