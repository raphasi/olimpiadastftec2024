using System;
using System.Diagnostics;

namespace CRM.Domain.Entities;

public sealed class Quote : Entity
{
    public Guid QuoteID { get; set; }
    public Nullable<Guid> OpportunityID { get; set; }
    public Nullable<Guid> ProductID { get; set; }
    public Nullable<Guid> PriceLevelID { get; set; }
    public Nullable<Guid> EventID { get; set; }
    public Nullable<Guid> LeadID { get; set; }
    public Nullable<Guid> CustomerID { get; set; }
    public Nullable<Guid> OrderID { get; set; }
    public Nullable<Guid> OrderItemID { get; set; }
    public Nullable<int> Quantity { get; set; }
    public Nullable<decimal> Discount { get; set; }
    public Nullable<decimal> TotalPrice { get; set; }
    public string Name { get; set; }


    //Navigation properties
    public Opportunity Opportunity { get; set; }
    public Customer Customer { get; set; }
    public Lead Lead { get; set; }
    public Product Product { get; set; }
    public PriceLevel PriceLevel { get; set; }
    public Event Event { get; set; } // Adicionada a propriedade de navegação para Event
}