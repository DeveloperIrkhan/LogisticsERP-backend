namespace LogisticsERP.API.DTOs.Documents
{
    public class UploadDocumentsDto
    {
        public string VehicleId { get; set; }

        public string DocumentType { get; set; }

        public List<IFormFile> Files { get; set; }
    }

    public class CloudinaryUploadResultDto
    {
        public string FileUrl { get; set; }

        public string PublicId { get; set; }
    }

    public class DocumentCreateDto
    {
        public string VehicleId { get; set; }

        public string DocumentType { get; set; }

        public string FileUrl { get; set; }

        public string? PublicId { get; set; }
    }
    public class DocumentResponseDto
    {
        public string DocumentId { get; set; }

        public string VehicleId { get; set; }

        public string DocumentType { get; set; }

        public string FileUrl { get; set; }

        public string? PublicId { get; set; }

        public DateTime UploadedAt { get; set; }
    }




}
