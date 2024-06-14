using System;

namespace CRM.Domain.Entities;

public sealed class Activity : Entity
{
    public Guid ActivityID { get; set; }
    public Nullable<Guid> OpportunityID { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }

    // Navigation properties
    public Opportunity Opportunity { get; set; }

    // Propriedade de navegação para Notes
    public ICollection<Note> Notes { get; set; }
}