using CRM.Application.DTOs;
using System;
using System.ComponentModel.DataAnnotations;

namespace CRM.Application.DTOs
{
    public class OrderItemDTO
    {
        public Guid OrderItemID { get; set; }

        [Required(ErrorMessage = "O campo OrderID é obrigatório.")]
        public Guid OrderID { get; set; }

        [Required(ErrorMessage = "O campo ProductID é obrigatório.")]
        public Guid ProductID { get; set; }

        [Required(ErrorMessage = "O campo QuoteID é obrigatório.")]
        public Guid QuoteID { get; set; }

        [Required(ErrorMessage = "O campo Quantidade é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "A Quantidade deve ser pelo menos 1.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "O campo Preço Unitário é obrigatório.")]
        [Range(0, double.MaxValue, ErrorMessage = "O Preço Unitário deve ser um valor positivo.")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "O campo Preço Total é obrigatório.")]
        [Range(0, double.MaxValue, ErrorMessage = "O Preço Total deve ser um valor positivo.")]
        public decimal TotalPrice { get; set; } // Calculado como Quantity * UnitPrice
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? StatusCode { get; set; }
    }
}