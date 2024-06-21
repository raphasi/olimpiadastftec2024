using System;

namespace CRM.WebApp.Site.Models;

public class EventViewModel
{
    public Guid EventID { get; set; }
    public Guid? ProductID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime? EventDate { get; set; }
    public string Location { get; set; }
    public decimal? TicketPrice { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public int? StatusCode { get; set; }
}