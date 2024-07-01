using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CRM.Application.DTOs;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Domain.Interfaces;

namespace CRM.Application.Services;

public class CartService : ICartService
{
    private readonly ICartRepository _cartRepository;
    private readonly IMapper _mapper;

    public CartService(ICartRepository cartRepository, IMapper mapper)
    {
        _cartRepository = cartRepository;
        _mapper = mapper;
    }

    public async Task<CartDto> GetByIdAsync(Guid cartId)
    {
        var cart = await _cartRepository.GetByIdAsync(cartId);
        return _mapper.Map<CartDto>(cart);
    }

    public async Task<IEnumerable<CartDto>> GetAllAsync()
    {
        var carts = await _cartRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<CartDto>>(carts);
    }

    public async Task<CartDto> AddAsync(CartDto cartDto)
    {
        cartDto.CartID = Guid.NewGuid();
        var cart = _mapper.Map<Cart>(cartDto);
        await _cartRepository.AddAsync(cart);
        return cartDto;
    }

    public async Task UpdateAsync(CartDto cartDto)
    {
        var cart = _mapper.Map<Cart>(cartDto);
        await _cartRepository.UpdateAsync(cart);
    }

    public async Task DeleteAsync(Guid cartId)
    {
        await _cartRepository.DeleteAsync(cartId);
    }
}