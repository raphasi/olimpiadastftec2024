using CRM.Application.DTOs;
using System;
using System.ComponentModel.DataAnnotations;

namespace CRM.Application.DTOs;

public class NoteDTO
{
    public Guid NoteID { get; set; }

    [Required(ErrorMessage = "O campo CustomerID é obrigatório.")]
    public Guid CustomerID { get; set; }

    [Required(ErrorMessage = "O campo ActivityID é obrigatório.")]
    public Guid ActivityID { get; set; }

    [Required(ErrorMessage = "O campo OpportunityID é obrigatório.")]
    public Guid OpportunityID { get; set; }

    [Required(ErrorMessage = "O campo Conteúdo é obrigatório.")]
    [StringLength(5000, ErrorMessage = "O Conteúdo não pode exceder 5000 caracteres.")]
    public string Content { get; set; }

    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public int? StatusCode { get; set; }
}