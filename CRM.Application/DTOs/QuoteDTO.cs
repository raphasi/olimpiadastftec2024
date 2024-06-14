using CRM.Application.DTOs;
using System;
using System.ComponentModel.DataAnnotations;

namespace CRM.Application.DTOs
{
    public class QuoteDTO
    {
        public Guid QuoteID { get; set; }

        [Required(ErrorMessage = "O campo OpportunityID é obrigatório.")]
        public Guid OpportunityID { get; set; }

        [Required(ErrorMessage = "O campo ProductID é obrigatório.")]
        public Guid ProductID { get; set; }

        [Required(ErrorMessage = "O campo PriceLevelID é obrigatório.")]
        public Guid PriceLevelID { get; set; }

        [Required(ErrorMessage = "O campo EventID é obrigatório.")]
        public Guid EventID { get; set; }

        [Required(ErrorMessage = "O campo Quantidade é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "A Quantidade deve ser pelo menos 1.")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "O campo Desconto é obrigatório.")]
        [Range(0, double.MaxValue, ErrorMessage = "O Desconto deve ser um valor positivo.")]
        public decimal Discount { get; set; }

        [Required(ErrorMessage = "O campo Preço Total é obrigatório.")]
        [Range(0, double.MaxValue, ErrorMessage = "O Preço Total deve ser um valor positivo.")]
        public decimal TotalPrice { get; set; }

        // Navigation properties
        public OpportunityDTO Opportunity { get; set; }
        public ProductDTO Product { get; set; }
        public PriceLevelDTO PriceLevel { get; set; }
        public EventDTO Event { get; set; }
    }
}