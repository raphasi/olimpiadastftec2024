
using System;

namespace CRM.Domain.Entities;

public sealed class OrderItem : Entity
{
    public Guid OrderItemID { get; set; }
    public Nullable<Guid> OrderID { get; set; }
    public Nullable<Guid> ProductID { get; set; }
    public Nullable<Guid> QuoteID { get; set; }
    public Nullable<int> Quantity { get; set; }
    public Nullable<decimal> UnitPrice { get; set; }
    public Nullable<decimal> TotalPrice { get; set; } // Calculado como Quantity * UnitPrice

    // Navigation properties
    public Order Order { get; set; }
    public Product Product { get; set; }
}