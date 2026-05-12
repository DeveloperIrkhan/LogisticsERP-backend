using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LogisticsERP.API.Data;
using LogisticsERP.API.DTOs.Documents;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsERP.API.Helpers
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly AppDbContext _dbContext;

        public CloudinaryService(IConfiguration config, AppDbContext appDbContext)
        {
            var cloudinaryAccount = new Account(
                config["CloudinarySettings:CloudName"],
                config["CloudinarySettings:ApiKey"],
                config["CloudinarySettings:ApiSecret"]
            );
            _cloudinary = new Cloudinary(cloudinaryAccount);
            _dbContext = appDbContext;
        }


        public async Task<CloudinaryUploadResultDto> UploadImage(IFormFile formFile, string? destinationFolderName)
        {
            await using var stream = formFile.OpenReadStream();

            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(formFile.FileName, stream),
                Folder = destinationFolderName ?? "Profile-Images"
            };
            var result = await _cloudinary.UploadAsync(uploadParams);

            return result == null ? throw new Exception("Image upload failed") : new CloudinaryUploadResultDto
            {
                FileUrl = result.SecureUrl.ToString(),
                PublicId = result.PublicId
            };
        }


        public async Task DeleteFileAsync(string publicId)
        {
            var deleteParams = new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Raw
            };

            var result = await _cloudinary.DestroyAsync(deleteParams);

            if (result.Result != "ok")
            {
                throw new Exception($"Cloudinary delete failed: {result.Result}");
            }
        }


        public async Task DeleteFilesAsync(string publicId)
        {
            var resourceTypes = new[]
            {
               ResourceType.Image,
               ResourceType.Video,
               ResourceType.Raw
             };

            foreach (var type in resourceTypes)
            {
                var deleteParams = new DeletionParams(publicId)
                {
                    ResourceType = type
                };

                var result = await _cloudinary.DestroyAsync(deleteParams);

                if (result.Result == "ok")
                {
                    return; // deleted successfully
                }
            }

            throw new Exception("File not found in any resource type.");
        }

        public async Task<List<CloudinaryUploadResultDto>> UploadPdfDocuments(IEnumerable<IFormFile> filesfiles, string? destinationFolderName)
        {

            var results = new List<CloudinaryUploadResultDto>();
            foreach (var file in filesfiles)
            {
                var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".png", ".jpg", ".jpeg" };
                var extension = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                    throw new Exception("Invalid file type");

                if (file.Length > 5 * 1024 * 1024) // 5MB limit
                    throw new Exception("File size exceeds limit");

                await using var stream = file.OpenReadStream();

                var uploadParams = new RawUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Folder = destinationFolderName
                };
                var result = await _cloudinary.UploadAsync(uploadParams);
                results.Add(new CloudinaryUploadResultDto
                {
                    FileUrl = result.SecureUrl.ToString(),
                    PublicId = result.PublicId
                });
            }
            return results;
        }

       
        public async Task<CloudinaryUploadResultDto> UpdateFileAsync(string publicId, IFormFile newFile, string? destinationFolderName)
        {
            var document = await _dbContext.VehicleDocuments.FirstOrDefaultAsync(d => d.PublicId == publicId);

            if (document == null)
                throw new Exception("Document not found");

            var uploadResult = await _cloudinary.UploadAsync(new RawUploadParams
            {
                File = new FileDescription(newFile.FileName, newFile.OpenReadStream()),
                Folder = $"vehicle-documents/{document.VehicleId}"
            });

            if (!string.IsNullOrEmpty(uploadResult.PublicId))
            {
                await DeleteFileAsync(publicId);
            }



            document.FileUrl = uploadResult.SecureUrl.ToString();
            document.PublicId = uploadResult.PublicId;
            await _dbContext.SaveChangesAsync();
            return new CloudinaryUploadResultDto
            {
                FileUrl = uploadResult.SecureUrl.ToString(),
                PublicId = uploadResult.PublicId
            };
        }
    }
}
