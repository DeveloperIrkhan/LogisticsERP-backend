using LogisticsERP.API.Data;
using LogisticsERP.API.DTOs.Documents;
using LogisticsERP.API.Helpers;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using LogisticsERP.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogisticsERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IDocumentService _documentService;
        public DocumentController(ICloudinaryService cloudinaryService, IDocumentService documentService)
        {
            _cloudinaryService = cloudinaryService;
            _documentService = documentService;
        }

        #region creating vehicle documents

        [HttpPost("upload-documents")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UplaodDocuemtns(string vehicleId, [FromForm] List<IFormFile> files, string? DocuemntType)
        {
            if (files == null || files.Count == 0)
                return BadRequest("No files uploaded.");
            var uploadResults = await _cloudinaryService.UploadPdfDocuments(files, $"vehicle-documents/{vehicleId}");

            var documentDtos = uploadResults.Select(x => new DocumentCreateDto
            {
                VehicleId = vehicleId,
                DocumentType = DocuemntType?? "",
                FileUrl = x.FileUrl,
                PublicId = x.PublicId
            }).ToList();
            var results = await _documentService.InsertDocuments(documentDtos);
            return Ok(uploadResults);
        }
        #endregion

        #region updating vehicle documents

        [HttpPost("update-documents")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateDocument([FromForm] UpdateDocumentRequest request)
        {
            if (request.NewFile == null)
                return BadRequest("No files uploaded.");
            var uploadResults = await _cloudinaryService.UpdateFileAsync(request.PublicId, request.NewFile, $"{request.DestinationFolderName ?? "vehicle-documents"}");

            return Ok(uploadResults);
        }
        #endregion

    }
}
