using LogisticsERP.API.Data;
using LogisticsERP.API.interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LogisticsERP.API.Repositories
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _DbSet;

        public GenericRepo(AppDbContext appDbContext)
        {
            _context = appDbContext;
            _DbSet = _context.Set<T>();
        }
        public async Task<T> AddAsync(T entity)
        {
            await _DbSet.AddAsync(entity);
            // Save changes to the database will be now in the service layer
            //await _context.SaveChangesAsync();
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _DbSet.AddRangeAsync(entities);
        }

        public async Task Delete(string Id)
        {
            var result = await _DbSet.FindAsync(Id);
            if (result == null)
            {
                throw new Exception($"Entity with Id {Id} not found.");
            }
            _DbSet.Remove(result);
            // Save changes to the database will be now in the service layer
            //await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _DbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(string id)
        {
            var result = await _DbSet.FindAsync(id);
            return result ?? throw new Exception($"Entity with Id {id} not found.");
        }

        public async Task Update(T entity)
        {
            _DbSet.Update(entity);
        }

        public async Task<IEnumerable<T>> WhereAsync (Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>()
                .Where(predicate)
                .ToListAsync();
        }
    }
}
