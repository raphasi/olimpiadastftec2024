using CRM.Application.DTOs;

namespace CRM.WebApp.Ingresso.Models
{
    public class CheckoutViewModel
    {
        public List<ProductViewModel>? CartItems { get; set; }
        public OpportunityDTO? Opportunity { get; set; }
        public IEnumerable<QuoteDTO>? Quotes { get; set; }
        public LeadDTO? Lead { get; set; }
        public CartHeaderDTO CartHeader { get; set; } 
    }
}