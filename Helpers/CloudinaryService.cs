using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using LogisticsERP.API.DTOs.Documents;
using LogisticsERP.API.interfaces;

namespace LogisticsERP.API.Helpers
{
    public class CloudinaryService: ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IConfiguration config)
        {
            var cloudinaryAccount = new Account(
                config["CloudinarySettings:CloudName"],
                config["CloudinarySettings:ApiKey"],
                config["CloudinarySettings:ApiSecret"]
            );
            _cloudinary = new Cloudinary(cloudinaryAccount);
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
                    Folder = destinationFolderName ?? "document"
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

            await _cloudinary.DestroyAsync(deleteParams);
        }
    }
}
