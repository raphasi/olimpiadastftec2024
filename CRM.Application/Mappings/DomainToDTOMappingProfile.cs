﻿using AutoMapper;
using CRM.Domain.Entities;
using CRM.Application.DTOs;

namespace CRM.Application.Mappings
{
    public class DomainToDTOMappingProfile : Profile
    {
        public DomainToDTOMappingProfile()
        {
            // Mapeamento de Quote para QuoteDTO e vice-versa
            CreateMap<Quote, QuoteDTO>().ReverseMap()
            .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.NameProduct))
            .ForMember(dest => dest.EventID, opt => opt.MapFrom(src => src.NameOpp))
            .ForMember(dest => dest.OpportunityID, opt => opt.MapFrom(scr => scr.NameProduct))
            .ForMember(dest => dest.PriceLevelID, opt => opt.MapFrom(scr => scr.NameLevel));

            // Mapeamento de Opportunity para OpportunityDTO e vice-versa
            CreateMap<Opportunity, OpportunityDTO>().ReverseMap();

            // Mapeamento de Product para ProductDTO e vice-versa
            CreateMap<Product, ProductDTO>().ReverseMap();

            // Mapeamento de PriceLevel para PriceLevelDTO e vice-versa
            CreateMap<PriceLevel, PriceLevelDTO>().ReverseMap();

            // Mapeamento de Event para EventDTO e vice-versa
            CreateMap<Event, EventDTO>().ReverseMap();

            // Mapeamento de Lead para LeadDTO e vice-versa
            CreateMap<Lead, LeadDTO>().ReverseMap();

            // Mapeamento de Customer para CustomerDTO e vice-versa
            CreateMap<Customer, CustomerDTO>().ReverseMap();

            // Mapeamento de Order para OrderDTO e vice-versa
            CreateMap<Order, OrderDTO>().ReverseMap();

            // Mapeamento de OrderItem para OrderItemDTO e vice-versa
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();

            // Mapeamento de Note para NoteDTO e vice-versa
            CreateMap<Note, NoteDTO>().ReverseMap();

            // Mapeamento de Activity para ActivityDTO e vice-versa
            CreateMap<Activity, ActivityDTO>().ReverseMap();

            // Mapeamento para Cart
            CreateMap<Cart, CartDto>()
                .ForMember(dest => dest.CartItems, opt => opt.MapFrom(src => src.CartItems))
                .ReverseMap();

            // Mapeamento para CartItem
            CreateMap<CartItem, CartItemDTO>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product))
                .ReverseMap();

            CreateMap<ProductEvent, ProductEventDTO>()
                .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.EventID, opt => opt.MapFrom(src => src.Event.Name));

            CreateMap<ProductEventDTO, ProductEvent>();
            CreateMap<ProductEventDTO, ProductEvent>();

        }
    }
}