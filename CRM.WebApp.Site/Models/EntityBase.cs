using CRM.WebApp.Site.Interface;

namespace CRM.WebApp.Site.Models
{
    public class EntityBase : IEntityBase
    {
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public string? CreatedByName { get; set; }
        public string? ModifiedByName { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? StatusCode { get; set; }
        public bool IsNew { get; set; } = true;
    }
}
