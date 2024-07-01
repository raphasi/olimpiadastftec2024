using CRM.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace CRM.Application.DTOs;

public class LeadDTO
{
    public Guid LeadID { get; set; }

    [Required(ErrorMessage = "O campo Nome Completo é obrigatório.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "O Nome Completo deve ter entre 3 e 200 caracteres.")]
    public string FullName { get; set; }

    [StringLength(100, MinimumLength = 2, ErrorMessage = "O Primeiro Nome deve ter entre 2 e 100 caracteres.")]
    public string? FirstName { get; set; }

    [StringLength(100, MinimumLength = 2, ErrorMessage = "O Sobrenome deve ter entre 2 e 100 caracteres.")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "O campo Email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O Email deve ser válido.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "O campo Telefone é obrigatório.")]
    [Phone(ErrorMessage = "O Telefone deve ser válido.")]
    public string Telephone { get; set; }

    public int? TypeLead { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public int? StatusCode { get; set; }
}