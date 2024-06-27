using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRM.Application.DTOs;

public class ProductDTO
{
    public Guid ProductID { get; set; }

    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "O Nome deve ter entre 2 e 100 caracteres.")]
    public string Name { get; set; }

    [StringLength(1000, ErrorMessage = "A Descrição não pode exceder 1000 caracteres.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "O campo Preço é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O Preço deve ser maior que zero.")]
    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "O campo Inventário é obrigatório.")]
    [Range(0, int.MaxValue, ErrorMessage = "O Inventário deve ser um valor não negativo.")]
    public int Inventory { get; set; }

    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public int? StatusCode { get; set; }

    public List<Guid> SelectedEventIds { get; set; } = new List<Guid>();
    public List<EventDTO> AvailableEvents { get; set; } = new List<EventDTO>();

}