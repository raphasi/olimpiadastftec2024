using CRM.Domain.Entities;
using CRM.Domain.Interfaces;
using CRM.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRM.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _productContext;

        public ProductRepository(ApplicationDbContext productContext)
        {
            _productContext = productContext;
        }

        public async Task<Product> GetProductByIdAsync(Guid productId)
        {
            return await _productContext.Set<Product>().FindAsync(productId);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productContext.Set<Product>().ToListAsync();
        }

        public async Task AddProductAsync(Product productEntity)
        {
            await _productContext.Set<Product>().AddAsync(productEntity);
            await _productContext.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product productEntity)
        {
            _productContext.Set<Product>().Update(productEntity);
            await _productContext.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Guid productId)
        {
            var product = await _productContext.Set<Product>().FindAsync(productId);
            if (product != null)
            {
                _productContext.Set<Product>().Remove(product);
                await _productContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Product>> SearchAsync(string query)
        {
            return await _productContext.Products
                .Where(c => c.Name.Contains(query))
                .ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetTop10Async()
        {
            return await _productContext.Products
                .OrderByDescending(c => c.CreatedOn)
                .Take(10)
                .ToListAsync();
        }

    }
}