using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Domain.Entities;
public sealed class Customer : Entity
{
    public Guid CustomerID { get; set; }
    public string FullName { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string Telephone { get; private set; }
    public string Address1 { get; set; }
    public string Address_PostalCode { get; set; }
    public string Address_Country { get; set; }
    public string Address_State { get; set; }
    public string Address_City { get; set; }
    public string Address_Adjunct { get; set; }
    public Nullable<int> TypeLead { get; private set; }
    public string CPF { get; set; }
    public string CNPJ { get; set; }

    // Navigation properties
    public ICollection<Opportunity> Opportunities { get; set; }

    // Propriedade de navegação para Notes
    public ICollection<Note> Notes { get; set; }

}