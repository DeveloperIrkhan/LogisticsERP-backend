using LogisticsERP.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LogisticsERP.API.interfaces
{
    public interface IGenericRepo<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(string id);
        Task<T> AddAsync(T entity);
        Task Update(T entity);
        Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate);

        Task Delete(string Id);
        Task AddRangeAsync(IEnumerable<T> entities);
    }
}
