using System;
using System.ComponentModel.DataAnnotations;

namespace CRM.WebApp.Site.Models;

public class ProductViewModel : EntityBase
{
    public Guid ProductID { get; set; }
    [Required(ErrorMessage = "Nome obrigátorio")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Descrição obrigátorio")]
    public string Description { get; set; }
    
    public decimal? Price { get; set; }
    [Required(ErrorMessage = "Imagem obrigátorio")]
    public string ImageUrl { get; set; }
    public int? Inventory { get; set; }
    public List<Guid> SelectedEventIds { get; set; } = new List<Guid>();
    public List<EventViewModel> AvailableEvents { get; set; } = new List<EventViewModel>();
}