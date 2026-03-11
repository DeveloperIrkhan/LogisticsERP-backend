using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogisticsERP.API.Models
{
    public class VehicleDocuments
    {
        [Key]
        public string DocumentId { get; set; } = $"PRCS-DOC-{Guid.NewGuid()}";
        [ForeignKey("VehicleId")]
        public string VehicleId { get; set; }

        public Vehicle Vehicle { get; set; }

        public string DocumentType { get; set; }

        public string FileUrl { get; set; }

        public string? PublicId { get; set; }

        public DateTime UploadedAt { get; set; } = DateTime.Now;
    }
}
