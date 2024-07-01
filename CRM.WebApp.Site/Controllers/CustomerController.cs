using Microsoft.AspNetCore.Mvc;
using CRM.WebApp.Site.Models;
using Microsoft.AspNetCore.Authorization;

namespace CRM.WebApp.Site.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    public class CustomerController : BaseController<CustomerViewModel, CustomerViewModel>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(IHttpClientFactory httpClientFactory, ILogger<CustomerController> logger) : base(httpClientFactory, "customer")
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            try
            {
                var customers = await GetCustomersAsync();
                return View(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter a lista de clientes.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var customer = await GetCustomerByIdAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }
                UpdateEntity(customer);
                return View(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter detalhes do cliente.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            var model = InitializeEntity();
            return View(model);
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerViewModel customerViewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient("CRM.API");
                    PutTokenInHeaderAuthorization(GetAccessToken(), client);
                    var response = await client.PostAsJsonAsync("api/customer", customerViewModel);
                    response.EnsureSuccessStatusCode();

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao criar cliente.");
                    return StatusCode(500, "Erro interno do servidor.");
                }
            }
            return View(customerViewModel);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            try
            {
                var customer = await GetCustomerByIdAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }
                customer.IsNew = false;
                return View(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter cliente para edição.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CustomerViewModel customerViewModel)
        {
            if (id != customerViewModel.CustomerID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient("CRM.API");
                    PutTokenInHeaderAuthorization(GetAccessToken(), client);
                    UpdateEntity(customerViewModel);
                    var response = await client.PutAsJsonAsync($"api/customer/{id}", customerViewModel);
                    if (!response.IsSuccessStatusCode)
                    {
                        return NotFound();
                    }

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao atualizar cliente.");
                    return StatusCode(500, "Erro interno do servidor.");
                }
            }
            return View(customerViewModel);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var customer = await GetCustomerByIdAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }
                return View(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter cliente para exclusão.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("CRM.API");
                PutTokenInHeaderAuthorization(GetAccessToken(), client);
                var response = await client.DeleteAsync($"api/customer/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar cliente.");
                return StatusCode(500, "Erro interno do servidor.");
            }
        }

        private async Task<IEnumerable<CustomerViewModel>> GetCustomersAsync()
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            PutTokenInHeaderAuthorization(GetAccessToken(), client);
            var response = await client.GetAsync("api/customer");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<CustomerViewModel>>();
        }

        private async Task<CustomerViewModel> GetCustomerByIdAsync(Guid id)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            PutTokenInHeaderAuthorization(GetAccessToken(), client);
            var response = await client.GetAsync($"api/customer/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<CustomerViewModel>();
        }
    }
}