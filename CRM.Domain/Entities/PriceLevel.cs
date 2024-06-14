using System;

namespace CRM.Domain.Entities;

public sealed class PriceLevel : Entity
{
    public Guid PriceLevelID { get; set; }
    public string LevelName { get; set; }
    public Nullable<decimal> DiscountPercentage { get; set; }
    public Nullable<decimal> ValueBase { get; set; }


    // Navigation properties
    public ICollection<Quote> Quotes { get; set; }
}