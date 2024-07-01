using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CRM.Application.DTOs;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using CRM.Domain.Interfaces;

namespace CRM.Application.Services
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IMapper _mapper;

        public CartItemService(ICartItemRepository cartItemRepository, IMapper mapper)
        {
            _cartItemRepository = cartItemRepository;
            _mapper = mapper;
        }

        public async Task<CartItemDTO> GetByIdAsync(Guid cartItemId)
        {
            var cartItem = await _cartItemRepository.GetByIdAsync(cartItemId);
            return _mapper.Map<CartItemDTO>(cartItem);
        }

        public async Task<IEnumerable<CartItemDTO>> GetAllByCartIdAsync(Guid cartId)
        {
            var cartItems = await _cartItemRepository.GetAllByCartIdAsync(cartId);
            return _mapper.Map<IEnumerable<CartItemDTO >>(cartItems);
        }

        public async Task<CartItemDTO> AddAsync(CartItemDTO cartItemDto)
        {
            cartItemDto.CartItemID = cartItemDto.CartItemID;   
            var cartItem = _mapper.Map<CartItem>(cartItemDto);
            await _cartItemRepository.AddAsync(cartItem);
            return cartItemDto;
        }

        public async Task UpdateAsync(CartItemDTO cartItemDto)
        {
            var cartItem = _mapper.Map<CartItem>(cartItemDto);
            await _cartItemRepository.UpdateAsync(cartItem);
        }

        public async Task DeleteAsync(Guid cartItemId)
        {
            await _cartItemRepository.DeleteAsync(cartItemId);
        }
    }
}