using LogisticsERP.API.DTOs.Documents;

namespace LogisticsERP.API.interfaces
{
    public interface ICloudinaryService
    {
        Task<List<CloudinaryUploadResultDto>> UploadPdfDocuments(IEnumerable<IFormFile> filesfiles, string? destinationFolderName);
        Task<CloudinaryUploadResultDto> UploadImage(IFormFile formFile, string? destinationFolderName);
        Task DeleteFileAsync(string publicId);
    }
}
