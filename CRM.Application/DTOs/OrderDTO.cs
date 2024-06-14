using CRM.Application.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRM.Application.DTOs;

public class OrderDTO
{
    public int OrderID { get; set; }

    [Required(ErrorMessage = "O campo OpportunityID é obrigatório.")]
    public Guid OpportunityID { get; set; }

    [Required(ErrorMessage = "O campo Data do Pedido é obrigatório.")]
    public DateTime OrderDate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "O campo Valor Total é obrigatório.")]
    [Range(0, double.MaxValue, ErrorMessage = "O Valor Total deve ser um valor positivo.")]
    public decimal TotalAmount { get; set; }

    // Navigation properties
    public OpportunityDTO Opportunity { get; set; }
    public ICollection<OrderItemDTO> OrderItems { get; set; }
}