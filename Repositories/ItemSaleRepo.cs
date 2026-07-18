using LogisticsERP.API.Data;
using LogisticsERP.API.enums;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsERP.API.Repositories
{
    public class ItemSaleRepo : IItemSaleRepo
    {
        private readonly AppDbContext _context;

        public ItemSaleRepo(AppDbContext context)
        {
            _context = context;

        }
        public async Task<List<ItemSale>> GetByItemAsync(string itemId)
        {
            return await _context.ItemSales
                .Include(x => x.Item)
                .Where(x => x.ItemId == itemId)
                .OrderByDescending(x => x.SaleDate)
                .ToListAsync();
        }


        public async Task<List<ItemSale>> GetByStatusAsync(ItemTransactionStatus status)
        {
            return await _context.ItemSales
                .Include(x => x.Item)
                .Where(x => x.Status == status)
                .OrderByDescending(x => x.SaleDate)
                .ToListAsync();
        }

        public async Task<List<ItemSale>> GetByVehicleAsync(string vehicleId)
        {
            return await _context.ItemSales
                .Include(x => x.Vehicle)
                .Where(x => x.VehicleId == vehicleId)
                .OrderByDescending(x => x.SaleDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalByMonthAsync(int year, int month)
        {
            return await _context.ItemSales
                    .Where(x => x.SaleDate.Year == year && x.SaleDate.Month == month)
                    .SumAsync(x => x.TotalAmount);
        }

        public async Task<List<ItemSale>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            return await _context.ItemSales
                    .Include(x => x.Item)
                    .Where(x => x.SaleDate >= from && x.SaleDate <= to)
                    .OrderByDescending(x => x.SaleDate)
                    .ToListAsync();
        }

        public async Task<List<ItemSale>> GetByMonthAsync(int year, int month)
        {
            return await _context.ItemSales
                .Include(x => x.Item)
                .Where(x => x.SaleDate.Year == year && x.SaleDate.Month == month)
                .OrderByDescending(x => x.SaleDate)
                .ToListAsync();

        }

    }
}
