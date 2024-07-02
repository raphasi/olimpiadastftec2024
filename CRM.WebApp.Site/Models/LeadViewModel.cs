using System;
using System.ComponentModel.DataAnnotations;

namespace CRM.WebApp.Site.Models;

public class LeadViewModel : EntityBase
{
    [Required]
    public Guid LeadID { get; set; }

    [Required(ErrorMessage ="Nome completo obrigátorio")]
    [StringLength(200)]
    public string FullName { get; set; }
    [Required(ErrorMessage = "Primeiro Nome obrigátorio")]
    [StringLength(100)]
    public string FirstName { get; set; }
    [Required(ErrorMessage = "Último Nome obrigátorio")]
    [StringLength(100)]
    public string LastName { get; set; }
    [Required(ErrorMessage = "E-mail obrigátorio")]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; }
    [Required(ErrorMessage = "Telefone obrigátorio")]
    [StringLength(20)]
    public string Telephone { get; set; }

}