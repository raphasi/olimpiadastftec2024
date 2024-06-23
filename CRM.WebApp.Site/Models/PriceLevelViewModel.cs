using System;

namespace CRM.WebApp.Site.Models;

public class PriceLevelViewModel : EntityBase
{
    public Guid PriceLevelID { get; set; }
    public string LevelName { get; set; }
    public decimal? DiscountPercentage { get; set; }
    public decimal? ValueBase { get; set; }
}