using System;

namespace CRM.Application.DTOs
{
    public class CartItemDTO
    {
        public Guid CartItemID { get; set; }
        public Guid CartID { get; set; }
        public Guid ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? StatusCode { get; set; }
        public ProductDTO Product { get; set; }
    }
}