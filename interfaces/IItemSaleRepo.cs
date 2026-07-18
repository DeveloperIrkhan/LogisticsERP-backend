using LogisticsERP.API.enums;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IItemSaleRepo
    {
        Task<List<ItemSale>> GetByItemAsync(string itemId);
        Task<List<ItemSale>> GetByVehicleAsync(string vehicleId);
        Task<List<ItemSale>> GetByStatusAsync(ItemTransactionStatus status);
        Task<List<ItemSale>> GetByDateRangeAsync(DateTime from, DateTime to);
        Task<List<ItemSale>> GetByMonthAsync(int year, int month);
        Task<decimal> GetTotalByMonthAsync(int year, int month);

    }
}
