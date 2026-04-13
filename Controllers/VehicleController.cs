using AutoMapper;
using LogisticsERP.API.DTOs.Drivers;
using LogisticsERP.API.DTOs.Vehicle;
using LogisticsERP.API.interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _serviceVehicle;
        private readonly IDriverService _driverService;

        public VehicleController(IVehicleService vehicleService, IDriverService driverService)
        {
            _serviceVehicle = vehicleService;
            _driverService = driverService;
        }


        #region Getting All Vehicals

        [HttpGet("get-all-vehicle")]
        public async Task<ActionResult<DriverResponseDto>> GetAllVehicle()
        {
            var vehicles = await _serviceVehicle.GetAllVehicles();
            if (vehicles == null)
            {
                return NotFound();
            }
            return Ok(vehicles);
        }


        #endregion

        #region Adding New Vehicle
        [HttpPost("add-vehicle")]
        public async Task<ActionResult<DriverResponseDto>> AddVehicle([FromBody] VehicleCreateDto vehicleCreateDto)
        {
            if (vehicleCreateDto == null)
            {
                return BadRequest("Invalid vehicle data.");
            }
            var createdVehicle = await _serviceVehicle.CreateVehicle(vehicleCreateDto);
            return CreatedAtAction(nameof(GetAllVehicle), new { id = createdVehicle.VehicleId }, createdVehicle);
        }
        #endregion

        #region Getting vehicle by Id
        [HttpGet("get-vehicle-by-id")]
        public async Task<ActionResult<DriverResponseDto>> GetVehicleById(string id)
        {
            var vehicle = await _serviceVehicle.GetVehicleById(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return Ok(vehicle);
        }
        #endregion

        #region Updating Vehicle
        [HttpPut("update-vehicle")]
        public async Task<ActionResult<DriverResponseDto>> UpdateVehicle([FromBody] VehicleUpdateDto dto)
        {
            //ArgumentNullException.ThrowIfNull(id, nameof(id));
            if (dto == null)
            {
                return BadRequest("Invalid vehicle data.");
            }
            var updatedVehicle = await _serviceVehicle.UpdateVehicle(dto);
            return updatedVehicle ==null ? 
                 NotFound() 
                : Ok(updatedVehicle);
        }
        #endregion

        #region Expiry Tracking (Registration / Insurance / Fitness)
        [HttpGet("expiring")]
        public async Task<IActionResult> GetExpiry([FromQuery] int days=30)
        {
            var result = _serviceVehicle.GetExpiringVehicles(days);
            return Ok(result);
        }
        #endregion


        #region assign-drivers
        [HttpPost("assign-driver")]
        public async Task<IActionResult> ExpiryTracking(string vehicleId, string driverId)
        {
            var result = await _driverService.AssignDriver(vehicleId,driverId);
            return Ok(result);
        }
        #endregion

        #region assign-drivers
        [HttpGet("drivers-list")]
        public async Task<IActionResult> DriverList(string vehicleId)
        {
            var result = await _driverService.DriverListAssignedToSpecficVehicle(vehicleId);            return Ok(result);
        }
        #endregion
    }
}
