using AutoMapper;
using LogisticsERP.API.Data;
using LogisticsERP.API.DTOs.Documents;
using LogisticsERP.API.DTOs.Drivers;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace LogisticsERP.API.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepo<VehicleDocuments> _docRepo;
        private readonly AppDbContext _dbContext;

        public DocumentService(IMapper mapper, AppDbContext appDbContext, IGenericRepo<VehicleDocuments> genericReopDocuemnt)
        {
            _mapper = mapper;
            _docRepo = genericReopDocuemnt;
            _dbContext = appDbContext;
        }

        public Task DeleteDocument(string id)
        {
            throw new NotImplementedException();
        }

        public Task<DocumentResponseDto> GetDocumentbyId(string id)
        {
            throw new NotImplementedException();
        }

       

        public async Task<List<DocumentResponseDto>> InsertDocuments(List<DocumentCreateDto> documentCreateDto)
        {
            try
            {
                if (documentCreateDto == null || !documentCreateDto.Any())
                    throw new Exception("Documents should not be empty!");

                var newDocuments = _mapper.Map<List<VehicleDocuments>>(documentCreateDto);

                await _docRepo.AddRangeAsync(newDocuments);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<List<DocumentResponseDto>>(newDocuments);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message ?? ex.Message);
            }

        }

        public Task<DocumentResponseDto> UpdateDocument(DocumentCreateDto documentUpdateDto)
        {
            throw new NotImplementedException();
        }
    }
}
