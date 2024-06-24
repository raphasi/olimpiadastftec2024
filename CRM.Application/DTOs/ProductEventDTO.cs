using CRM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.DTOs;

public class ProductEventDTO
{
    public Guid Id { get; set; }
    public Guid ProductID { get; set; }
    public Product Product { get; set; }
    public Guid EventID { get; set; }
    public Event Event { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public int? StatusCode { get; set; }
}
