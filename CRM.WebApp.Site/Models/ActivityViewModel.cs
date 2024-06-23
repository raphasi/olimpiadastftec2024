using System;

namespace CRM.WebApp.Site.Models;

public class ActivityViewModel : EntityBase
{
    public Guid ActivityID { get; set; }
    public Guid? OpportunityID { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
}