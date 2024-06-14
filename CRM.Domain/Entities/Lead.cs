using CRM.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Domain.Entities;

public sealed class Lead : Entity
{
    public Guid LeadID { get; private set; }
    public string FullName { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Telephone { get; private set; }

    // Propriedade de navegação para Opportunities
    public ICollection<Opportunity> Opportunities { get; set; }
}
