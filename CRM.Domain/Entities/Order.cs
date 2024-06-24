
using System;
using System.Collections.Generic;

namespace CRM.Domain.Entities;

public sealed class Order : Entity
{
    public Guid OrderID { get; set; }
    public Nullable<Guid> OpportunityID { get; set; }
    public Nullable<Guid> QuoteID { get; set; }
    public Nullable<DateTime> OrderDate { get; set; } = DateTime.Now;
    public Nullable<decimal> TotalAmount { get; set; }

    // Navigation properties
    public Opportunity Opportunity { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
}