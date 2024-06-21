using System;

namespace CRM.WebApp.Site.Models;

public class PriceLevelViewModel
{
    public Guid PriceLevelID { get; set; }
    public string LevelName { get; set; }
    public decimal? DiscountPercentage { get; set; }
    public decimal? ValueBase { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public int? StatusCode { get; set; }
}