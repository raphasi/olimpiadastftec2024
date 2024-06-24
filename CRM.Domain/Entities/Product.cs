using System;

namespace CRM.Domain.Entities;

public sealed class Product : Entity
{
    public Guid ProductID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Nullable<decimal> Price { get; set; }
    public string ImageUrl { get; set; }
    public Nullable<int> Inventory { get; set; }

    public ICollection<ProductEvent> ProductEvents { get; set; }

    // Navigation properties
    public ICollection<Quote> Quotes { get; set; }
    public ICollection<Event> Events { get; set; } // Adicionada a coleção de Events
                                                   // Propriedade de navegação para OrderItems
    public ICollection<OrderItem> OrderItems { get; set; }
}