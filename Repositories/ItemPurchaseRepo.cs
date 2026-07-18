using LogisticsERP.API.Data;
using LogisticsERP.API.enums;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;

namespace LogisticsERP.API.Repositories
{
    public class ItemPurchaseRepo : IItemPurchaseRepo
    {
        private readonly AppDbContext _context;

        public ItemPurchaseRepo(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ItemPurchase>> GetByItemAsync(string itemId)
        {
            return await _context.ItemPurchases
                .Include(x => x.Item)
                .Where(x => x.ItemId == itemId)
                .OrderByDescending(x => x.PurchaseDate)
                .ToListAsync();
        }


        public async Task<List<ItemPurchase>> GetByVehicleAsync(string vehicleId)
        {
            return await _context.ItemPurchases
                .Include(x => x.Item)
                .Where(x => x.VehicleId == vehicleId)
                .OrderByDescending(x => x.PurchaseDate)
                .ToListAsync();
        }


        public async Task<List<ItemPurchase>> GetByStatusAsync(ItemTransactionStatus status)
        {
            return await _context.ItemPurchases
                .Include(x => x.Item)
                .Where(x => x.Status == status)
                .OrderByDescending(x => x.PurchaseDate)
                .ToListAsync();
        }


        public async Task<List<ItemPurchase>> GetByMonthAsync(int year, int month)
        {
            return await _context.ItemPurchases
                .Include(x => x.Item)
                .Where(x => x.PurchaseDate.Year == year && x.PurchaseDate.Month == month)
                .OrderByDescending(x => x.PurchaseDate)
                .ToListAsync();
        }


        public async Task<List<ItemPurchase>> GetByDateRangeAsync(DateTime from, DateTime to)
        {
            return await _context.ItemPurchases
                .Include(x => x.Item)
                .Where(x => x.PurchaseDate >= from && x.PurchaseDate <= to)
                .OrderByDescending(x => x.PurchaseDate)
                .ToListAsync();
        }

        public async Task<decimal> GetTotalByMonthAsync(int year, int month)
        {
            return await _context.ItemPurchases
                  .Where(x => x.PurchaseDate.Year == year && x.PurchaseDate.Month == month)
                  .SumAsync(x => x.TotalAmount);
        }
    }
}
