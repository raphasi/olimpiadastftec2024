using System;
using System.ComponentModel.DataAnnotations;

namespace CRM.WebApp.Site.ViewModels;

public class OpportunityViewModel
{
    public Guid OpportunityID { get; set; }
    public Guid? CustomerID { get; set; }
    public Guid? LeadID { get; set; }
    public string Description { get; set; }
    public decimal? EstimatedValue { get; set; }
    public DateTime? ExpectedCloseDate { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public int? StatusCode { get; set; }
}