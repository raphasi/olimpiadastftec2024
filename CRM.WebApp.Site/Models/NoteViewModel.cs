using System;

namespace CRM.WebApp.Site.Models;

public class NoteViewModel : EntityBase
{
    public Guid NoteID { get; set; }
    public Guid? CustomerID { get; set; }
    public Guid? ActivityID { get; set; }
    public Guid? OpportunityID { get; set; }
    public string Content { get; set; }
}