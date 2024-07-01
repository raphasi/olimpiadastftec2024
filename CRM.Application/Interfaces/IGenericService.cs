namespace CRM.Application.Interfaces
{
    public interface IGenericService
    {
        Task<int> GetCountAsync<T>() where T : class;
    }
}