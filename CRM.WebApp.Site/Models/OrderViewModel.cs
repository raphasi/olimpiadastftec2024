using System;

namespace CRM.WebApp.Site.Models;

public class OrderViewModel : EntityBase
{
    public Guid OrderID { get; set; }
    public Guid? OpportunityID { get; set; }
    public DateTime? OrderDate { get; set; }
    public decimal? TotalAmount { get; set; }
}