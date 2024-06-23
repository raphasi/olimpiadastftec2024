using System;
using System.ComponentModel.DataAnnotations;

namespace CRM.WebApp.Site.Models;

public class OpportunityViewModel : EntityBase
{
    public Guid OpportunityID { get; set; }
    public Guid? CustomerID { get; set; }
    public Guid? LeadID { get; set; }
    public string Description { get; set; }
    public decimal? EstimatedValue { get; set; }
    public DateTime? ExpectedCloseDate { get; set; }
}