using Azure;
using CRM.Application.DTOs;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.WebApp.Ingresso.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace CRM.WebApp.Ingresso.Controllers
{
    public class StoreController : BaseController<ProductViewModel, ProductViewModel>
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public StoreController(IHttpClientFactory httpClientFactory) : base(httpClientFactory, "cart")
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            var response = await client.GetAsync("api/product");
            response.EnsureSuccessStatusCode();

            var products = await response.Content.ReadFromJsonAsync<IEnumerable<ProductViewModel>>();
            return View(products);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            var response = await client.GetAsync($"api/event/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var eventDetails = await response.Content.ReadFromJsonAsync<EventViewModel>();

            // Buscar produtos associados ao evento
            var productsResponse = await client.GetAsync($"api/event/{id}/products");
            if (productsResponse.IsSuccessStatusCode)
            {
                var products = await productsResponse.Content.ReadFromJsonAsync<IEnumerable<ProductViewModel>>();
                eventDetails.AvailableProducts = products != null ? new List<ProductViewModel>(products) : new List<ProductViewModel>();
            }

            return View(eventDetails);
        }

        [Authorize]
        public async Task<IActionResult> AddToCart(Guid id)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            PutTokenInHeaderAuthorization(GetAccessToken(), client);
            var response = await client.GetAsync($"api/product/{id}");
            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            var product = await response.Content.ReadFromJsonAsync<ProductViewModel>();

            // Adicionar lógica para adicionar o produto ao carrinho
            var cart = HttpContext.Session.GetObjectFromJson<List<ProductViewModel>>("Cart") ?? new List<ProductViewModel>();
            cart.Add(product);
            HttpContext.Session.SetObjectAsJson("Cart", cart);

            // Verificar se já existe uma oportunidade na sessão
            var opportunityId = HttpContext.Session.GetString("opportunity_id");
            if (string.IsNullOrEmpty(opportunityId))
            {
                // Criar nova oportunidade no CRM
                var user = GetUserInfo();
                var opportunity = new OpportunityDTO
                {
                    LeadID = user.leadID,
                    Name = "Oportunidade - " + user.userName,
                    Description = "Oportunidade gerada na loja virtual",
                    CreatedOn = DateTime.Now,
                    EstimatedValue = product.Price.Value,
                    ExpectedCloseDate = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    CreatedBy = new Guid(user.id),
                    ModifiedBy = new Guid(user.id),
                    StatusCode = 1
                };

                response = await client.PostAsJsonAsync("api/opportunity", opportunity);
                response.EnsureSuccessStatusCode();
                var opp = await response.Content.ReadAsStringAsync();
                var oppDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<OpportunityDTO>(opp);

                // Armazenar o ID da oportunidade na sessão
                HttpContext.Session.SetString("opportunity_id", oppDTO.OpportunityID.ToString());
                opportunityId = oppDTO.OpportunityID.ToString();
            }

            // Criar cotação no CRM
            var quote = new QuoteDTO
            {
                OpportunityID = new Guid(opportunityId),
                ProductID = product.ProductID,
                Name = "Cotação - " + GetUserInfo().userName,
                LeadID = GetUserInfo().leadID,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                CreatedBy = new Guid(GetUserInfo().id),
                StatusCode = 1,
                ModifiedBy = new Guid(GetUserInfo().id),
                TotalPrice = product.Price.Value,
                Quantity = 1,
                Discount = 1
            };

            response = await client.PostAsJsonAsync("api/quote", quote);
            response.EnsureSuccessStatusCode();

            return RedirectToAction("Index", "Cart");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddToCartMultiProduct(List<CartUpdateModel> listProduct)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            PutTokenInHeaderAuthorization(GetAccessToken(), client);
            if (listProduct == null || !listProduct.Any())
            {
                return BadRequest("A lista de produtos está vazia.");
            }

            var cart = HttpContext.Session.GetObjectFromJson<List<ProductViewModel>>("Cart") ?? new List<ProductViewModel>();
            decimal estimatedValue = 0;

            foreach (var product in listProduct)
            {
                var productDetails = await GetProductDetailsAsync(client, product.ProductId);
                if (productDetails == null)
                {
                    return NotFound();
                }

                productDetails.Inventory = product.Quantity;
                cart.Add(productDetails);
                estimatedValue += productDetails.Price.Value;
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);

            var opportunityId = HttpContext.Session.GetString("opportunity_id");
            if (string.IsNullOrEmpty(opportunityId))
            {
                var opportunity = await CreateOpportunityAsync(client, estimatedValue);
                if (opportunity == null)
                {
                    return StatusCode(500, "Erro ao criar oportunidade.");
                }

                HttpContext.Session.SetString("opportunity_id", opportunity.OpportunityID.ToString());
                opportunityId = opportunity.OpportunityID.ToString();
            }

            foreach (var product in listProduct)
            {
                var productDetails = await GetProductDetailsAsync(client, product.ProductId);
                if (productDetails == null)
                {
                    return NotFound();
                }

                var valueQuote = productDetails.Price.Value * (decimal)product.Quantity;

                var success = await CreateQuoteAsync(client, new Guid(opportunityId), product.ProductId, product.Quantity, valueQuote);
                if (!success)
                {
                    return StatusCode(500, "Erro ao criar cotação.");
                }
            }

            return RedirectToAction("Index", "Cart");
        }

        private async Task<ProductViewModel> GetProductDetailsAsync(HttpClient client, Guid? productId)
        {
            var response = await client.GetAsync($"api/product/{productId}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<ProductViewModel>();
        }

        private async Task<OpportunityDTO> CreateOpportunityAsync(HttpClient client, decimal estimatedValue)
        {
            var user = GetUserInfo();
            var opportunity = new OpportunityDTO
            {
                LeadID = user.leadID,
                Name = "Oportunidade - " + user.userName,
                Description = "Oportunidade gerada na loja virtual",
                CreatedOn = DateTime.Now,
                EstimatedValue = estimatedValue,
                ExpectedCloseDate = DateTime.Now,
                ModifiedOn = DateTime.Now,
                CreatedBy = new Guid(user.id),
                ModifiedBy = new Guid(user.id),
                StatusCode = 1
            };

            var response = await client.PostAsJsonAsync("api/opportunity", opportunity);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var opp = await response.Content.ReadAsStringAsync();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<OpportunityDTO>(opp);
        }

        private async Task<bool> CreateQuoteAsync(HttpClient client, Guid opportunityId, Guid product, int quantity, decimal valueQuote)
        {
            var user = GetUserInfo();
            var quote = new QuoteDTO
            {
                OpportunityID = opportunityId,
                ProductID = product,
                Name = "Cotação - " + user.userName,
                LeadID = user.leadID,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                CreatedBy = new Guid(user.id),
                StatusCode = 1,
                ModifiedBy = new Guid(user.id),
                Quantity = quantity, // Ajuste aqui conforme necessário
                Discount = 1,
                TotalPrice = valueQuote
            };

            var response = await client.PostAsJsonAsync("api/quote", quote);
            return response.IsSuccessStatusCode;
        }

        [Authorize]
        public async Task<IActionResult> RemoveFromCart(Guid productId, Guid quoteID)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            PutTokenInHeaderAuthorization(GetAccessToken(), client);

            var cart = HttpContext.Session.GetObjectFromJson<List<ProductViewModel>>("Cart") ?? new List<ProductViewModel>();
            var productToRemove = cart.FirstOrDefault(p => p.ProductID == productId);

            if (productToRemove != null)
            {
                cart.Remove(productToRemove);
                HttpContext.Session.SetObjectAsJson("Cart", cart);

                var quoteUpdate = new UpdateFieldDTO
                {
                    FieldName = "StatusCode",
                    FieldValue = JsonDocument.Parse("5").RootElement
                };

                var response = await client.PatchAsync($"api/quote/{quoteID}/update-field", JsonContent.Create(quoteUpdate));
                response.EnsureSuccessStatusCode();
            }

            return RedirectToAction("Index", "Cart");
        }

        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            var cart = HttpContext.Session.GetObjectFromJson<List<ProductViewModel>>("Cart");
            var opportunityId = HttpContext.Session.GetString("opportunity_id");

            if (cart == null || !cart.Any() || string.IsNullOrEmpty(opportunityId))
            {
                return RedirectToAction("Index", "Cart");
            }

            var client = _httpClientFactory.CreateClient("CRM.API");
            PutTokenInHeaderAuthorization(GetAccessToken(), client);

            // Obter a oportunidade
            var response = await client.GetAsync($"api/opportunity/{opportunityId}");
            response.EnsureSuccessStatusCode();
            var opportunity = await response.Content.ReadFromJsonAsync<OpportunityDTO>();

            // Obter as cotações vinculadas à oportunidade
            response = await client.GetAsync($"api/quote/{opportunityId}/quotesopp");
            response.EnsureSuccessStatusCode();
            var quotes = await response.Content.ReadFromJsonAsync<IEnumerable<QuoteDTO>>();

            // Obter as informações do lead
            response = await client.GetAsync($"api/lead/{opportunity.LeadID}");
            response.EnsureSuccessStatusCode();
            var lead = await response.Content.ReadFromJsonAsync<LeadDTO>();

            // Calcular o total das cotações
            decimal totalAmount = quotes.Sum(q => q.TotalPrice);

            var checkoutViewModel = new CheckoutViewModel
            {
                CartItems = cart,
                Opportunity = opportunity,
                Quotes = quotes,
                Lead = lead,
                CartHeader = new CartHeaderDTO
                {
                    TotalAmount = totalAmount
                }
            };

            return View(checkoutViewModel);
        }

        public IActionResult OrderConfirmation()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CompleteCheckout(CheckoutViewModel model)
        {
            var client = _httpClientFactory.CreateClient("CRM.API");
            PutTokenInHeaderAuthorization(GetAccessToken(), client);
            var user = GetUserInfo();
            // Atualizar o lead para customer
            var leadUpdate = new UpdateFieldDTO
            {
                FieldName = "StatusCode",
                FieldValue = JsonDocument.Parse("4").RootElement
            };

            // lead ganha
            var response = await client.PatchAsync($"api/lead/{model.Lead.LeadID}/update-field", JsonContent.Create(leadUpdate));
            response.EnsureSuccessStatusCode();

            // Criar a ordem
            var order = new OrderDTO
            {
                OpportunityID = model.Opportunity.OpportunityID,
                OrderDate = DateTime.Now,
                TotalAmount = model.Opportunity.EstimatedValue,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                CreatedBy = new Guid(user.id),
                ModifiedBy = new Guid(user.id),
                StatusCode = 4 // Ordem ganha
            };

            response = await client.PostAsJsonAsync("api/order", order);
            response.EnsureSuccessStatusCode();
            var orderResult = await response.Content.ReadFromJsonAsync<OrderDTO>();

            // Obter as cotações vinculadas à oportunidade
            response = await client.GetAsync($"api/quote/{model.Opportunity.OpportunityID}/quotesopp");
            response.EnsureSuccessStatusCode();
            var quotes = await response.Content.ReadFromJsonAsync<IEnumerable<QuoteDTO>>();

            // Criar os itens da ordem
            foreach (var quote in quotes)
            {
                var orderItem = new OrderItemDTO
                {
                    OrderID = orderResult.OrderID,
                    ProductID = quote.ProductID,
                    Quantity = quote.Quantity,
                    TotalPrice = quote.TotalPrice,
                    StatusCode = 4,
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    CreatedBy = new Guid(user.id),
                    ModifiedBy = new Guid(user.id)
                };

                response = await client.PostAsJsonAsync("api/orderitem", orderItem);
                response.EnsureSuccessStatusCode();
            }

            // Atualizar a oportunidade e as cotações para status ganho
            // Oportunidade ganha
            var OppUpdate = new UpdateFieldDTO
            {
                FieldName = "StatusCode",
                FieldValue = JsonDocument.Parse("4").RootElement
            };

            response = await client.PatchAsync($"api/opportunity/{model.Opportunity.OpportunityID}/update-field", JsonContent.Create(OppUpdate));
            response.EnsureSuccessStatusCode();

            foreach (var quote in quotes)
            {
                // Oportunidade ganha
                var quoteUpdate = new UpdateFieldDTO
                {
                    FieldName = "StatusCode",
                    FieldValue = JsonDocument.Parse("4").RootElement
                };

                // Cotação ganha
                response = await client.PatchAsync($"api/quote/{quote.QuoteID}/update-field", JsonContent.Create(quoteUpdate));
                response.EnsureSuccessStatusCode();
            }

            var customer = new CustomerDTO
            {
                FullName = model.Lead.FirstName + " " + model.Lead.LastName,
                FirstName = model.Lead.FirstName,
                LastName = model.Lead.LastName,
                CPF = string.IsNullOrEmpty(model.CartHeader.CPF) ? "0" : model.CartHeader.CPF,
                Address1 = model.CartHeader.Address1,
                Address_Adjunct = model.CartHeader.Address_Adjunct,
                Address_City = model.CartHeader.Address_City,
                Address_Country = model.CartHeader.Address_Country,
                Address_PostalCode = model.CartHeader.Address_PostalCode,
                Address_State = model.CartHeader.Address_State,
                Telephone = model.Lead.Telephone,
                TypeLead = model.CartHeader.TypeLead,
                CNPJ = string.IsNullOrEmpty(model.CartHeader.CNPJ) ? "0" : model.CartHeader.CNPJ,
                Email = model.Lead.Email,
                StatusCode = 1,
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now,
                CreatedBy = new Guid(user.id),
                ModifiedBy = new Guid(user.id)
            };

            response = await client.PostAsJsonAsync("api/customer", customer);
            response.EnsureSuccessStatusCode();

            // Limpar o carrinho e a sessão
            HttpContext.Session.Remove("Cart");
            HttpContext.Session.Remove("opportunity_id");

            return RedirectToAction("OrderConfirmation", new { orderId = orderResult.OrderID });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateCart([FromBody] CartUpdateModel model)
        {
            if (model == null)
            {
                return BadRequest("Dados inválidos.");
            }

            var cart = HttpContext.Session.GetObjectFromJson<List<ProductViewModel>>("Cart") ?? new List<ProductViewModel>();
            var productToUpdate = cart.FirstOrDefault(p => p.ProductID == model.ProductId);

            if (productToUpdate != null)
            {
                productToUpdate.Inventory = model.Quantity;
                HttpContext.Session.SetObjectAsJson("Cart", cart);

                // Verificar se já existe uma oportunidade na sessão
                var opportunityId = HttpContext.Session.GetString("opportunity_id");
                if (!string.IsNullOrEmpty(opportunityId))
                {
                    // Atualizar a cotação no banco de dados
                    var client = _httpClientFactory.CreateClient("CRM.API");
                    PutTokenInHeaderAuthorization(GetAccessToken(), client);

                    // Obter as cotações vinculadas à oportunidade
                    var response = await client.GetAsync($"api/quote/{opportunityId}/quotesopp");
                    response.EnsureSuccessStatusCode();
                    var quotes = await response.Content.ReadFromJsonAsync<IEnumerable<QuoteDTO>>();

                    foreach (var item in quotes)
                    {
                        // Verificar se a cotação corresponde ao produto atualizado
                        if (item.ProductID == productToUpdate.ProductID)
                        {
                            var quoteUpdate = new QuoteDTO
                            {
                                QuoteID = item.QuoteID,
                                OpportunityID = item.OpportunityID,
                                LeadID = item.LeadID,
                                CreatedBy = item.CreatedBy,
                                Name = item.Name,
                                Discount = item.Discount,
                                ProductID = productToUpdate.ProductID,
                                Quantity = model.Quantity,
                                TotalPrice = productToUpdate.Price.Value * model.Quantity, // Calcula o preço total
                                ModifiedOn = DateTime.Now,
                                ModifiedBy = new Guid(GetUserInfo().id)
                            };

                            var responseQuote = await client.PutAsJsonAsync($"api/quote/{item.QuoteID}", quoteUpdate);
                            if (!responseQuote.IsSuccessStatusCode)
                            {
                                return StatusCode((int)responseQuote.StatusCode, "Erro ao atualizar a cotação.");
                            }
                        }
                    }
                }
            }

            return Ok(new { success = true });
        }
    }

    public class CartUpdateModel
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}