namespace CRM.Domain.Entities;

public abstract class Entity
{
    public Nullable<Guid> CreatedBy { get; protected set; }
    public Nullable<Guid> ModifiedBy { get; protected set; }
    public Nullable<DateTime> CreatedOn { get; protected set; }
    public Nullable<DateTime> ModifiedOn { get; protected set; }
    public Nullable<int> StatusCode { get; protected set; }
}
