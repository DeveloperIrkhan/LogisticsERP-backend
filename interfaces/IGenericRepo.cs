namespace LogisticsERP.API.interfaces
{
    public interface IGenericRepo<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(string id);
        Task<T> AddAsync(T entity);
        Task Update(T entity);
        Task Delete(string Id);
    }
}
