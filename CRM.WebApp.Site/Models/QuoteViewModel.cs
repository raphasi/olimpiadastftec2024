using System;
using System.ComponentModel.DataAnnotations;

namespace CRM.WebApp.Site.Models;

public class QuoteViewModel : EntityBase
{
    public Guid QuoteID { get; set; }

    [Required(ErrorMessage = "O campo OpportunityID é obrigatório.")]
    public Guid OpportunityID { get; set; }

    public string? NameOpp { get; set; }

    [Required(ErrorMessage = "O campo ProductID é obrigatório.")]
    public Guid ProductID { get; set; }

    public string? NameProduct { get; set; }

    public Guid? PriceLevelID { get; set; }

    public string? NameLevel { get; set; }

    public Guid? EventID { get; set; }

    public string? NameEvento { get; set; }

    public Guid? CustomerID { get; set; }

    [Required(ErrorMessage = "O campo LeadID é obrigatório.")]
    public Guid? LeadID { get; set; }

    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "O Nome deve ter entre 3 e 200 caracteres.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "O campo Quantidade é obrigatório.")]
    [Range(1, int.MaxValue, ErrorMessage = "A Quantidade deve ser pelo menos 1.")]
    public int Quantity { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "O Desconto deve ser um valor positivo.")]
    public decimal Discount { get; set; }

    [Required(ErrorMessage = "O campo Preço Total é obrigatório.")]
    [Range(0, double.MaxValue, ErrorMessage = "O Preço Total deve ser um valor positivo.")]
    public decimal TotalPrice { get; set; }

    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public int? StatusCode { get; set; }

    public string? LeadName { get; set; }
    public string? CustomerName { get; set; }

}