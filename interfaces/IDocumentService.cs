using LogisticsERP.API.DTOs.Documents;
using LogisticsERP.API.DTOs.Drivers;

namespace LogisticsERP.API.interfaces
{
    public interface IDocumentService
    {
        Task<DocumentResponseDto> GetDocumentbyId(string id);
        Task<List<DocumentResponseDto>> InsertDocuments(List<DocumentCreateDto> documentCreateDto);
        Task DeleteDocument(string id); 
        Task<DocumentResponseDto> UpdateDocument(DocumentCreateDto documentUpdateDto);
       
    }
}
