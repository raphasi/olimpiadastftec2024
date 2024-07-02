using System.Net.Http;
using System.Threading.Tasks;
using CRM.WebApp.Site.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CRM.WebApp.Site.Controllers
{
    [Authorize]
    public class HomeController : BaseController<CustomerViewModel, OpportunityViewModel>
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IHttpClientFactory httpClientFactory) : base(httpClientFactory, "count")
        {
            _httpClientFactory = httpClientFactory;
        }

        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Index()
        {
            var customerCount = await GetCountAsync("Customer");
            var (opportunityCount, estimatedValue) = await GetOpportunityCountAndEstimatedValueAsync();
            var quotesCount = await GetCountAsync("Quotes");
            var leadCount = await GetCountAsync("Lead");

            var model = new DashboardViewModel
            {
                CustomerCount = customerCount,
                OpportunityCount = opportunityCount,
                OpportunityEstimatedValue = estimatedValue,
                QuotesCount = quotesCount,
                LeadCount = leadCount
            };

            return View(model);
        }

        private async Task<int> GetCountAsync(string entityName)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            PutTokenInHeaderAuthorization(GetAccessToken(), client);
            var response = await client.GetAsync($"api/count/{entityName}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<int>(content);
        }

        private async Task<(int Count, decimal EstimatedValue)> GetOpportunityCountAndEstimatedValueAsync()
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            PutTokenInHeaderAuthorization(GetAccessToken(), client);
            var response = await client.GetAsync("api/opportunity/count-and-estimated-value");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<(int Count, decimal EstimatedValue)>(content);
        }
    }
}