using CRM.WebApp.Ingresso.Interface;

namespace CRM.WebApp.Ingresso.Models
{
    public class EntityBase : IEntityBase
    {
        public Guid? CreatedBy { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? StatusCode { get; set; }
        public bool? IsNew { get; set; } = true;
    }
}
