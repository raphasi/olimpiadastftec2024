﻿namespace CRM.WebApp.Site.Models;

public class CustomerViewModel
{
    public Guid CustomerID { get; set; }
    public string FullName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Telephone { get; set; }
    public string Address1 { get; set; }
    public string Address_PostalCode { get; set; }
    public string Address_Country { get; set; }
    public string Address_State { get; set; }
    public string Address_City { get; set; }
    public string Address_Adjunct { get; set; }
    public int? TypeLead { get; set; }
    public string CPF { get; set; }
    public string CNPJ { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public int? StatusCode { get; set; }
}
