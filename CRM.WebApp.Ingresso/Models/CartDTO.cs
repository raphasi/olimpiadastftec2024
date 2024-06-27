using CRM.Application.DTOs;
using System;
using System.Collections.Generic;

namespace CRM.WebApp.Ingresso.Models
{
    public class CartDTO : EntityBase
    {
        public Guid CartID { get; set; }
        public Guid UserID { get; set; }
        public int? StatusCode { get; set; }
        public List<CartItemDTO> CartItems { get; set; }
    }
}