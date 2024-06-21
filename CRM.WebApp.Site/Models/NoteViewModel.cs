using System;

namespace CRM.WebApp.Site.Models;

public class NoteViewModel
{
    public Guid NoteID { get; set; }
    public Guid? CustomerID { get; set; }
    public Guid? ActivityID { get; set; }
    public Guid? OpportunityID { get; set; }
    public string Content { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public int? StatusCode { get; set; }
}