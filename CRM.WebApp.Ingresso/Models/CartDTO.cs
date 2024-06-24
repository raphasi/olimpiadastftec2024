using CRM.Application.DTOs;
using System;
using System.Collections.Generic;

namespace CRM.WebApp.Ingresso.Models
{
    public class CartDto
    {
        public Guid CartID { get; set; }
        public Guid UserID { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? StatusCode { get; set; }
        public List<CartItemDTO> CartItems { get; set; }
    }
}