using System;
using System.ComponentModel.DataAnnotations;

namespace CRM.WebApp.Site.Models;

public class OpportunityViewModel : EntityBase
{
    public Guid OpportunityID { get; set; }

    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "O Nome deve ter entre 3 e 200 caracteres.")]
    public string Name { get; set; }
    public Guid? CustomerID { get; set; }
    public Guid? LeadID { get; set; }
    public string Description { get; set; }
    public decimal? EstimatedValue { get; set; }
    public DateTime? ExpectedCloseDate { get; set; }
}