using System;
using System.ComponentModel.DataAnnotations;

namespace CRM.WebApp.Site.Models;

public class LeadViewModel
{
    [Required]
    public Guid LeadID { get; set; }

    [Required]
    [StringLength(200)]
    public string FullName { get; set; }

    [StringLength(100)]
    public string FirstName { get; set; }

    [StringLength(100)]
    public string LastName { get; set; }

    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; }

    [StringLength(20)]
    public string Telephone { get; set; }

    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public int? StatusCode { get; set; }
}