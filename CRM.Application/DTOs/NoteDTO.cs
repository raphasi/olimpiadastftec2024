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

    // Navigation properties
    public CustomerDTO Customer { get; set; }
    public OpportunityDTO Opportunities { get; set; }
    public ActivityDTO Activities { get; set; }
}