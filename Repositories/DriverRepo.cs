using AutoMapper;
using LogisticsERP.API.Data;
using LogisticsERP.API.DTOs.Drivers;
using LogisticsERP.API.DTOs.Vehicle;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using Microsoft.EntityFrameworkCore;

namespace LogisticsERP.API.Repositories
{
    public class DriverRepo : IDriverRepo
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public DriverRepo(AppDbContext appDbContext, IMapper mapper)
        {
            _dbContext = appDbContext;
            _mapper = mapper;
        }
        public async Task<List<DriverResponseDto>> GerDriverListForSpecficVehicle(string vehicleId)
        {
            var result = await _dbContext.Drivers.Where(x => x.VehicleId == vehicleId).ToListAsync();
            return _mapper.Map<List<DriverResponseDto>>(result);
        }


        public async Task<List<DriverResponseDto>> getAllDriverAsync() {

            var drivers = await _dbContext.Drivers.ToListAsync();
            return _mapper.Map<List<DriverResponseDto>>(drivers);
        }
    } 
}
