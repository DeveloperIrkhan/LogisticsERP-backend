using LogisticsERP.API.Data;
using LogisticsERP.API.enums;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsERP.API.Repositories
{
    public class ItemRepo : IItemRepo
    {
        private readonly AppDbContext _context;

        public ItemRepo(AppDbContext appContext)
        {
            _context = appContext;
        }
        public async Task<List<Item>> GetActiveAsync()
        {
            return await _context.Items
                .Where(x => x.IsActive)
                .OrderBy(x => x.ItemName)
                .ToListAsync();
        }

        public async Task<List<Item>> GetByCategoryAsync(ItemCategory itemCategory)
        {
            return await _context.Items
                .Where(x => x.ItemCategory == itemCategory)
                .OrderBy(x => x.ItemName)
                .ToListAsync();
        }

        public async Task<Item?> GetByNameAsync(string itemName)
        {
            return await _context.Items
                .FirstOrDefaultAsync(x => x.ItemName.ToLower() == itemName.ToLower());
        }

        public async Task<List<Item>> GetLowStockAsync()
        {
            return await _context.Items
                .Where(x => x.IsActive && x.ReorderLevel.HasValue && x.CurrentStock <= x.ReorderLevel.Value)
                .ToListAsync();
        }
    }
}
