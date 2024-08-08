using AutoMapper;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Application.DTOs;
using CRM.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Options;

namespace CRM.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly string _content_url_blob;

    public ProductService(IProductRepository productRepository, IMapper mapper, IOptions<ConfigurationImagens> options)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _content_url_blob = options.Value.content_url;
    }

    public async Task<IEnumerable<ProductDTO>> GetAllAsync()
    {
        var products = await _productRepository.GetAllProductsAsync();

        foreach (var item in products)
        {
            item.ImageUrl = _content_url_blob + "/" + item.ImageUrl;
        }

        return _mapper.Map<IEnumerable<ProductDTO>>(products);
    }

    public async Task<ProductDTO> GetByIdAsync(Guid id)
    {
        var product = await _productRepository.GetProductByIdAsync(id);
        product.ImageUrl = _content_url_blob + "/" + product.ImageUrl;
        return _mapper.Map<ProductDTO>(product);
    }

    public async Task<ProductDTO> AddAsync(ProductDTO product)
    {
        product.ProductID = Guid.NewGuid();
        var productEntity = _mapper.Map<Product>(product);
        await _productRepository.AddProductAsync(productEntity);
        return product;
    }

    public async Task UpdateAsync(ProductDTO product)
    {
        var productEntity = _mapper.Map<Product>(product);
        await _productRepository.UpdateProductAsync(productEntity);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _productRepository.DeleteProductAsync(id);
    }

    public async Task<IEnumerable<ProductDTO>> SearchAsync(string query)
    {
        var opps = string.IsNullOrEmpty(query)
            ? await _productRepository.GetTop10Async()
            : await _productRepository.SearchAsync(query);
        return opps.Select(c => new ProductDTO
        {
            ProductID = c.ProductID,
            Name = c.Name
        });
    }
}