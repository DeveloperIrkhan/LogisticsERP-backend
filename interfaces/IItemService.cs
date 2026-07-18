using LogisticsERP.API.DTOs.Item;
using LogisticsERP.API.enums;
using LogisticsERP.API.Models;

namespace LogisticsERP.API.interfaces
{
    public interface IItemService
    {
        //CRUD operations
        Task<ApiResponse<ItemResponseDto>> CreateAsync(ItemCreateDto dto);
        Task<ApiResponse<ItemResponseDto>> UpdateAsync(string id, ItemUpdateDto dto);
        Task<ApiResponse<ItemResponseDto>> GetByIdAsync(string id);
        Task<ApiResponse<List<ItemResponseDto>>> GetAllAsync();
        Task<ApiResponse<bool>> DeleteAsync(string id);


        //Filters
        Task<ApiResponse<List<ItemResponseDto>>> GetByCategoryAsync(ItemCategory itemCategory);
        Task<ApiResponse<List<ItemResponseDto>>> GetActiveAsync();
        Task<ApiResponse<List<ItemResponseDto>>> GetLowStockAsync();


        //Reports
        Task<ApiResponse<List<ItemStockReportDto>>> GetStockReportAsync();

    }
}
