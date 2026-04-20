using AutoMapper;
using LogisticsERP.API.Data;
using LogisticsERP.API.DTOs.Vehicle;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsERP.API.Repositories
{
     public class VehicleRepo : IVehicleRepo
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public VehicleRepo(AppDbContext appDbContext, IMapper mapper)
        {
            _context = appDbContext;
            _mapper = mapper;
        }

        
        async Task<VehicleResponseDto> IVehicleRepo.GetVehicleById(string vehicleId)
        {
            var result = await _context.Vehicles
                .Include(x=> x.Drivers)
                .Include(x=> x.Documents)
                .FirstOrDefaultAsync(x => x.VehicleId == vehicleId);

            if (result == null)
                throw new Exception("Vehicle not found");
            return _mapper.Map<VehicleResponseDto>(result);
        }
    }
}
