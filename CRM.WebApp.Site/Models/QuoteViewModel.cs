using System;
using System.ComponentModel.DataAnnotations;

namespace CRM.WebApp.Site.Models;

public class QuoteViewModel : EntityBase
{
    public Guid QuoteID { get; set; }
    public Guid? OpportunityID { get; set; }
    public Guid? ProductID { get; set; }
    public Guid? PriceLevelID { get; set; }
    public Guid? EventID { get; set; }
    public Guid? CustomerID { get; set; }
    public Guid? LeadID { get; set; }
    public int? Quantity { get; set; }
    public decimal? Discount { get; set; }
    public decimal? TotalPrice { get; set; }

    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "O Nome deve ter entre 3 e 200 caracteres.")]
    public string Name { get; set; }

}