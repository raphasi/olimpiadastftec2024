using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

[ApiController]
[Route("api/[controller]")]
public class GenericController<TViewModel> : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _entityName;

    public GenericController(IHttpClientFactory httpClientFactory, string entityName)
    {
        _httpClientFactory = httpClientFactory;
        _entityName = entityName;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query = null)
    {
        var client = _httpClientFactory.CreateClient("CRM.API");
        var response = await client.GetAsync($"/api/{_entityName}/search?query={query}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        var entities = JsonConvert.DeserializeObject<IEnumerable<TViewModel>>(content);

        return Ok(entities);
    }
}