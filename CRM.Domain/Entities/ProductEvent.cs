using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Domain.Entities;

public class ProductEvent : Entity
{
    public Guid Id { get; set; }
    public Guid ProductID { get; set; }
    public Product Product { get; set; }
    public Guid EventID { get; set; }
    public Event Event { get; set; }
}
