
using System;
using System.Collections.Generic;

namespace CRM.Domain.Entities;
public sealed class Event : Entity
{
    public Guid EventID { get; set; }
    public Nullable<Guid> ProductID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Nullable<DateTime> EventDate { get; set; }
    public string Location { get; set; }
    public Nullable<decimal> TicketPrice { get; set; }
    public ICollection<ProductEvent> ProductEvents { get; set; }
    public string ImageUrl { get; set; }


    // Navigation properties
    public Product Product { get; set; }
    public ICollection<Quote> Quotes { get; set; }
}