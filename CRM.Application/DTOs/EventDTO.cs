using CRM.Application.DTOs;
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

    
    public Nullable<DateTime> EventDate { get; set; }

    [StringLength(500, ErrorMessage = "A Localização não pode exceder 500 caracteres.")]
    public string Location { get; set; }


    [Range(0.01, double.MaxValue, ErrorMessage = "O Preço do Ingresso deve ser maior que zero.")]
    public Nullable<decimal> TicketPrice { get; set; }

    // Navigation properties
    public ProductDTO Product { get; set; }
    public ICollection<QuoteDTO> Quotes { get; set; }
}