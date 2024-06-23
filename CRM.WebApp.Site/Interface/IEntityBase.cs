namespace CRM.WebApp.Site.Interface;
public interface IEntityBase
{
    DateTime? CreatedOn { get; set; }
    Guid? CreatedBy { get; set; }
    DateTime? ModifiedOn { get; set; }
    Guid? ModifiedBy { get; set; }
    int? StatusCode { get; set; }
}
