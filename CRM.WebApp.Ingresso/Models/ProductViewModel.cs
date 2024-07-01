using System;
using System.ComponentModel.DataAnnotations;

namespace CRM.WebApp.Ingresso.Models;

public class ProductViewModel : EntityBase
{
    public Guid ProductID { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public string? ImageUrl { get; set; }
    public int? Inventory { get; set; }
    public List<Guid>? SelectedEventIds { get; set; } = new List<Guid>();
    public List<EventViewModel>? AvailableEvents { get; set; } = new List<EventViewModel>();
}