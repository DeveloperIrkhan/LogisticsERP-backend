using LogisticsERP.API.enums;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IItemPurchaseRepo
    {
        Task<List<ItemPurchase>> GetByItemAsync(string itemId);
        Task<List<ItemPurchase>> GetByVehicleAsync(string vehicleId);
        Task<List<ItemPurchase>> GetByStatusAsync(ItemTransactionStatus status);
        Task<List<ItemPurchase>> GetByDateRangeAsync(DateTime from, DateTime to);
        Task<List<ItemPurchase>> GetByMonthAsync(int year, int month);
        Task<decimal> GetTotalByMonthAsync(int year, int month);

    }
}
