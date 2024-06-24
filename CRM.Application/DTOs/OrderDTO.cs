using CRM.Application.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CRM.Application.DTOs;

public class OrderDTO
{
    public Guid OrderID { get; set; }

    [Required(ErrorMessage = "O campo OpportunityID é obrigatório.")]
    public Guid OpportunityID { get; set; }
    public Guid? QuoteID { get; set; }

    [Required(ErrorMessage = "O campo Data do Pedido é obrigatório.")]
    public DateTime OrderDate { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "O campo Valor Total é obrigatório.")]
    [Range(0, double.MaxValue, ErrorMessage = "O Valor Total deve ser um valor positivo.")]
    public decimal TotalAmount { get; set; }

    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public int? StatusCode { get; set; }
}