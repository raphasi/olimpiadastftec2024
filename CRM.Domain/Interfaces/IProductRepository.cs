using CRM.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Domain.Interfaces;

public interface IProductRepository
{
    Task<Product> GetProductByIdAsync(Guid productId);
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task AddProductAsync(Product productEntity);
    Task UpdateProductAsync(Product productEntity);
    Task DeleteProductAsync(Guid productId);
    Task<IEnumerable<Product>> SearchAsync(string query);
    Task<IEnumerable<Product>> GetTop10Async();
}