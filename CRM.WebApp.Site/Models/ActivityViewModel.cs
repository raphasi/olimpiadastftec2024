using System;

namespace CRM.WebApp.Site.Models;

public class ActivityViewModel
{
    public Guid ActivityID { get; set; }
    public Guid? OpportunityID { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public int? StatusCode { get; set; }
}