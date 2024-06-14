using CRM.Domain.Entities;
using System;

public sealed class Note : Entity
{
    public Guid NoteID { get; set; }
    public Nullable<Guid> CustomerID { get; set; }
    public Nullable<Guid> ActivityID { get; set; }
    public Nullable<Guid> OpportunityID { get; set; }
    public string Content { get; set; }


    // Navigation properties
    public Customer Customer { get; set; }
    public Opportunity Opportunities { get; set; }
    public Activity Activities { get; set; }
}