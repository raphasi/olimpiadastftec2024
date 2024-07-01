using System;
using System.ComponentModel.DataAnnotations;

namespace CRM.WebApp.Ingresso.Models;

public class LeadViewModel : EntityBase
{
    [Required]
    public Guid LeadID { get; set; }

    [Required]
    [StringLength(200)]
    public string FullName { get; set; }

    [StringLength(100)]
    public string? FirstName { get; set; }

    [StringLength(100)]
    public string? LastName { get; set; }

    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; }

    [StringLength(20)]
    public string Telephone { get; set; }

}