using System;
using System.ComponentModel.DataAnnotations;

namespace CRM.WebApp.Site.Models;

public class QuoteViewModel
{
    public Guid QuoteID { get; set; }
    public Guid? OpportunityID { get; set; }
    public Guid? ProductID { get; set; }
    public Guid? PriceLevelID { get; set; }
    public Guid? EventID { get; set; }
    public int? Quantity { get; set; }
    public decimal? Discount { get; set; }
    public decimal? TotalPrice { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public int? StatusCode { get; set; }
}