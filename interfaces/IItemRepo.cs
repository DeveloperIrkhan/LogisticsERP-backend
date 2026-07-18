using LogisticsERP.API.enums;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IItemRepo
    {
        Task<List<Item>> GetByCategoryAsync(ItemCategory itemCategory);
        Task<List<Item>> GetActiveAsync();
        Task<List<Item>> GetLowStockAsync();
        Task<Item?> GetByNameAsync(string itemName);
    }
}
