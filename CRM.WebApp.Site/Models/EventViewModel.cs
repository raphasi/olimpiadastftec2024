using System;

namespace CRM.WebApp.Site.Models;

public class EventViewModel : EntityBase
{
    public Guid EventID { get; set; }
    public Guid? ProductID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime? EventDate { get; set; }
    public string Location { get; set; }
    public decimal? TicketPrice { get; set; }
    public string? ImageUrl { get; set; }
    public List<Guid> SelectedProductIds { get; set; } = new List<Guid>();
    public List<ProductViewModel> AvailableProducts { get; set; } = new List<ProductViewModel>();
}