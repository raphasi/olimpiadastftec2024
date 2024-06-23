using System;
using System.ComponentModel.DataAnnotations;

namespace CRM.Application.DTOs;

public class ActivityDTO
{
    public Guid ActivityID { get; set; }

    [Required(ErrorMessage = "O campo OpportunityID é obrigatório.")]
    public Guid OpportunityID { get; set; }

    [Required(ErrorMessage = "O campo Tipo é obrigatório.")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "O Tipo deve ter entre 2 e 50 caracteres.")]
    public string Type { get; set; }

    [Required(ErrorMessage = "O campo Descrição é obrigatório.")]
    [StringLength(500, ErrorMessage = "A Descrição não pode exceder 500 caracteres.")]
    public string Description { get; set; }

    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public int? StatusCode { get; set; }
}