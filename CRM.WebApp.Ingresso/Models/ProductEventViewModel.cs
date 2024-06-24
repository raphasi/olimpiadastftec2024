using CRM.Application.DTOs;


namespace CRM.WebApp.Ingresso.Models;

public class ProductEventViewModel
{
    public Guid ProductID { get; set; }
    public ProductDTO Product { get; set; }
    public Guid EventID { get; set; }
    public EventDTO Event { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? ModifiedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public int? StatusCode { get; set; }
}
