using CRM.Application.DTOs;
using CRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRM.Application.DTOs;

public class EventDTO
{
    public Guid EventID { get; set; }

    public Nullable<Guid> ProductID { get; set; }

    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "O Nome deve ter entre 2 e 200 caracteres.")]
    public string Name { get; set; }

    [StringLength(1000, ErrorMessage = "A Descrição não pode exceder 1000 caracteres.")]
    public string Description { get; set; }

    public string? ImageUrl { get; set; }


    public Nullable<DateTime> EventDate { get; set; }

    [StringLength(500, ErrorMessage = "A Localização não pode exceder 500 caracteres.")]
    public string Location { get; set; }


    [Range(0.01, double.MaxValue, ErrorMessage = "O Preço do Ingresso deve ser maior que zero.")]
    public Nullable<decimal> TicketPrice { get; set; }

    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public int? StatusCode { get; set; }

    public List<Guid> SelectedProductIds { get; set; } = new List<Guid>();
    public List<ProductDTO> AvailableProducts { get; set; } = new List<ProductDTO>();
}