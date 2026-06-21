namespace LogisticsERP.API.DTOs.Documents
{
    public class UploadDocumentsDto
    {
        public string VehicleId { get; set; } = string.Empty;

        public string DocumentType { get; set; } = string.Empty;

        public List<IFormFile> Files { get; set; } = new();
    }

    public class CloudinaryUploadResultDto
    {
        public string FileUrl { get; set; } = string.Empty;

        public string PublicId { get; set; } = string.Empty;
    }

    public class DocumentCreateDto
    {
        public string VehicleId { get; set; } = string.Empty;

        public string DocumentType { get; set; } = string.Empty;

        public string FileUrl { get; set; } = string.Empty;

        public string? PublicId { get; set; }
    }
    public class DocumentResponseDto
    {
        public string DocumentId { get; set; } = string.Empty;

        public string VehicleId { get; set; } = string.Empty;

        public string DocumentType { get; set; } = string.Empty;

        public string FileUrl { get; set; } = string.Empty;

        public string? PublicId { get; set; }

        public DateTime UploadedAt { get; set; }
    }

    public class UpdateDocumentRequest
    {
        public string PublicId { get; set; } = string.Empty;
        public IFormFile NewFile { get; set; } = default!;
        public string? DestinationFolderName { get; set; }
    }


}
