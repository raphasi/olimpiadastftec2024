using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Domain.Entities;

public sealed class Opportunity : Entity
{
    public Guid OpportunityID { get; set; }
    public string Name { get; set; }
    public Nullable<Guid> CustomerID { get; set; }
    public Nullable<Guid> LeadID { get; set; }
    public string Description { get; set; }
    public Nullable<decimal> EstimatedValue { get; set; }
    public Nullable<DateTime> ExpectedCloseDate { get; set; }

    // Navigation properties
    public Customer Customer { get; set; }
    public Lead Lead { get; set; }
    public ICollection<Order> Orders { get; set; } // Adicionada a coleção de Orders
    public ICollection<Quote> Quotes { get; set; }
    public ICollection<Activity> Activities { get; set; }
    public ICollection<Note> Notes { get; set; }
}