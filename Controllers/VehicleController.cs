using AutoMapper;
using LogisticsERP.API.DTOs.Documents;
using LogisticsERP.API.DTOs.Drivers;
using LogisticsERP.API.DTOs.Vehicle;
using LogisticsERP.API.enums;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;
        private readonly IDriverService _driverService;

        public VehicleController(IVehicleService vehicleService, IDriverService driverService)
        {
            _vehicleService = vehicleService;
            _driverService = driverService;
        }

        #region Getting All Vehicals
        [HttpGet("get-all-vehicle")]
        public async Task<ActionResult<VehicleResponseDto>> GetAllVehicle()
        {
            var vehicles = await _vehicleService.GetAllVehicles();
            if (vehicles == null)
            {
                return NotFound();
            }
            return Ok(vehicles);
        }


        #endregion

        #region Adding New Vehicle
        [HttpPost("add-vehicle")]
        public async Task<ActionResult<VehicleResponseDto>> AddVehicle([FromBody] VehicleCreateDto vehicleCreateDto)
        {
            if (vehicleCreateDto == null)
            {
                return BadRequest("Invalid vehicle data.");
            }
            var createdVehicle = await _vehicleService.CreateVehicle(vehicleCreateDto);
            return CreatedAtAction(nameof(GetAllVehicle), new { id = createdVehicle.VehicleId }, createdVehicle);
        }
        #endregion

        #region Getting full record of vehicle using vehicleId
        [HttpGet("get-full-record-of-vehicleId/{id}")]
        public async Task<ActionResult<VehicleResponseDto>> GetFullRecordOdVehicleById(string id)
        {
            var vehicle = await _vehicleService.GetFullRecordByVehicleById(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return Ok(vehicle);
        }
        #endregion

        #region Getting vehicle documents of vehicle by Id
        [HttpGet("get-documents-of-vehicle/{id}")]
        public async Task<ActionResult<VehicleResponseDto>> GetDocumentOfVehicleById(string id)
        {
            var vehicle = await _vehicleService.GetDocumentOfVehicleById(id);
            return Ok(vehicle);
        }
        #endregion

        #region Getting only vehicle by Id
        [HttpGet("get-vehicle-by-id/{id}")]
        public async Task<ActionResult<VehicleResponseDto>> GetVehicleById(string id)
        {
            var vehicle = await _vehicleService.GetVehicleById(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return Ok(vehicle);
        }
        #endregion

        #region Getting only Assigned vehicles
        [HttpGet("get-assigned-vehicle-list")]
        public async Task<ActionResult<ApiResponse<VehicleResponseDto>>> GetAssignedVehicleList()
        {
            var vehicle = await _vehicleService.GetAssignedVehicleList(VehicleStatus.Active);
            if (vehicle == null)
            {
                return new ApiResponse<VehicleResponseDto>
                {
                    Success = false,
                    Message = "No assigned vehicles found."
                };
            }
            return Ok(vehicle);
        }
        #endregion

        #region Getting only Unassigned vehicles
        [HttpGet("get-unassigned-vehicle-list")]
        public async Task<ActionResult<VehicleResponseDto>> GetUnAssignedVehicleList()
        {
            var vehicle = await _vehicleService.GetUnAssignedVehicleList(VehicleStatus.InActive);
            if (vehicle == null)
            {
                return NotFound();
            }
            return Ok(vehicle);
        }
        #endregion

        #region specfic vehicle Assigned driver list
        [HttpGet("get-assigned-drivers-for-vehicle")]
        public async Task<ActionResult<VehicleResponseDto>> GerDriverListForSpecficVehicle(string vehicleId)
        {
            var response = await _driverService.GetAssignedDriversListForSignleVehicle(vehicleId);
            return Ok(response);
        }
        #endregion

        #region Updating Vehicle
        [HttpPut("update-vehicle")]
        public async Task<ActionResult<VehicleResponseDto>> UpdateVehicle([FromBody] VehicleUpdateDto dto)
        {
            //ArgumentNullException.ThrowIfNull(id, nameof(id));
            if (dto == null)
            {
                return BadRequest("Invalid vehicle data.");
            }
            var updatedVehicle = await _vehicleService.UpdateVehicle(dto);
            return updatedVehicle == null ?
                 NotFound()
                : Ok(updatedVehicle);
        }
        #endregion

        #region Filtering 
        [HttpGet("filtering-vehicle")]
        public async Task<IActionResult> VehivleFiltering([FromQuery] VehicleFilterDto vehicleFilterDto)
        {
            var result = await _vehicleService.GetVehicles(vehicleFilterDto);
            return Ok(result);
        }
        #endregion

        #region Expiry Tracking (Registration / Insurance / Fitness)
        [HttpGet("expiring")]
        public async Task<IActionResult> GetExpiry([FromQuery] int days = 30)
        {
            var result = _vehicleService.GetExpiringVehicles(days);
            return Ok(result);
        }
        #endregion

        #region unassign-drivers using vehicle-Id
        [HttpPost("unassign-driver")]
        public async Task<IActionResult> UnassignDriver(string vehicleId)
        {
            var result = await _driverService.UnassignDriver(vehicleId);
            return Ok(result);
        }
        #endregion

        #region drivers-list
        [HttpGet("drivers-list")]
        public async Task<IActionResult> DriverList(string vehicleId)
        {
            var result = await _driverService.DriverListAssignedToSpecficVehicle(vehicleId);
            return Ok(result);
        }
        #endregion

        #region change vehicle state 
        [HttpPut("change-vehicle-state")]
        public async Task<ActionResult<VehicleResponseDto>> MakeVehicleIsActive([FromQuery] string vehicleId, VehicleStatus status)
        {
            var result = await _vehicleService.ChangeVehicleStatusAsync(vehicleId,status);
            if (result == null)
            {
                return BadRequest("Failed to make vehicle is active.");
            }
            return Ok(result);
        }

        #endregion

        #region filtering by vehicle status
        [HttpGet("get-by-status")]
        public async Task<ActionResult> GetVehicleStatus([FromQuery] VehicleStatus vehicleStatus)
        {
            var response = await _vehicleService.GetVehicleStatus(vehicleStatus);
            if (response == null)
            {
                return BadRequest("Failed to retrieve vehicle status.");
            }
            return Ok(response);
        }
        #endregion
    }
}
