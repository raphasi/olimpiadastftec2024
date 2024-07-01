using CRM.WebApp.Site.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

[ApiController]
[Route("api/[controller]")]
public class GenericController<TViewModel> : BaseController<CustomerViewModel, TViewModel>
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _entityName;

    public GenericController(IHttpClientFactory httpClientFactory, string entityName) : base (httpClientFactory, "customer")
    {
        _httpClientFactory = httpClientFactory;
        _entityName = entityName;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query = null)
    {
        var client = _httpClientFactory.CreateClient("CRM.API");
        PutTokenInHeaderAuthorization(GetAccessToken(), client);
        var response = await client.GetAsync($"/api/{_entityName}/search?query={query}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var entities = JsonConvert.DeserializeObject<IEnumerable<TViewModel>>(content);

        return Ok(entities);
    }
}