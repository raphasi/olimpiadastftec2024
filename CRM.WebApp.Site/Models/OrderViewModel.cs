using System;

namespace CRM.WebApp.Site.Models;

public class OrderViewModel
{
    public Guid OrderID { get; set; }
    public Guid? OpportunityID { get; set; }
    public DateTime? OrderDate { get; set; }
    public decimal? TotalAmount { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public int? StatusCode { get; set; }
}