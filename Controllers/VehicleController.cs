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
        [HttpGet("get-all-vehicles")]
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
            return createdVehicle == null ?  BadRequest(createdVehicle) : Ok(createdVehicle);
        }
        #endregion

        #region Getting full record of vehicle using vehicleId
        [HttpGet("get-full-record-of-vehicleId/{id}")]
        public async Task<ActionResult<VehicleResponseDto>> GetFullRecordOdVehicleById([FromRoute] string id)
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
        public async Task<ActionResult<VehicleResponseDto>> GetDocumentOfVehicleById([FromRoute] string id)
        {
            var vehicle = await _vehicleService.GetDocumentsOfVehicleById(id);
            return Ok(vehicle);
        }
        #endregion

        #region Getting only vehicle by Id
        [HttpGet("get-vehicle-by-id/{id}")]
        public async Task<ActionResult<VehicleResponseDto>> GetVehicleById([FromRoute] string id)
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
        [HttpGet("get-active-vehicle-list")]
        public async Task<ActionResult<VehicleResponseDto>> GetAssignedVehicleList()
        {
            var vehicle = await _vehicleService.GetAssignedVehicleList(VehicleStatus.Active);
            return vehicle.Success ? Ok(vehicle) : BadRequest(vehicle);
        }
        #endregion

        #region Getting only Unassigned vehicles
        [HttpGet("get-unassigned-vehicle-list")]
        public async Task<ActionResult<VehicleResponseDto>> GetUnAssignedVehicleList()
        {
            var vehicle = await _vehicleService.GetUnAssignedVehicleList(VehicleStatus.InActive);
            return vehicle.Success ? Ok(vehicle) : BadRequest(vehicle);
        }
        #endregion

        #region specfic vehicle Assigned driver list
        [HttpGet("get-assigned-drivers-for-vehicle")]
        public async Task<ActionResult<VehicleResponseDto>> GerDriverListForSpecficVehicle
            ([FromRoute] string vehicleId)
        {
            var response = await _driverService.GetAssignedDriversListForSignleVehicle(vehicleId);
            return response.Success ? Ok(response) : BadRequest(response);
        }
        #endregion

        #region Updating Vehicle
        [HttpPut("update-vehicle")]
        public async Task<ActionResult<VehicleResponseDto>> UpdateVehicle([FromBody] VehicleUpdateDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid vehicle data.");

            var updatedVehicle = await _vehicleService.UpdateVehicle(dto);
            return updatedVehicle.Success ? Ok(updatedVehicle) : BadRequest(updatedVehicle);
        }
        #endregion

        #region delete Vehicle
        [HttpDelete("delete-vehicle/{vehicleId}")]
        public async Task<ActionResult<VehicleResponseDto>> DeleteVehicle([FromRoute] string vehicleId)
        {
            if (string.IsNullOrEmpty(vehicleId))
                return BadRequest("Invalid vehicle Id or pass Id");

            var result = await _vehicleService.DeleteVehicle(vehicleId);
            return result.Success ? Ok(result) : NotFound(result);
        }
        #endregion

        #region Filtering 
        [HttpGet("filtering-vehicle")]
        public async Task<IActionResult> VehicleFilter([FromQuery] VehicleFilterDto vehicleFilterDto)
        {
            var result = await _vehicleService.GetVehicles(vehicleFilterDto);
            return result.Success ? Ok(result) : BadRequest(result);

        }
        #endregion

        #region Expiry Tracking (Registration)
        [HttpGet("get-registeration-expiry")]
        public async Task<IActionResult> GetRegisterationExpiry([FromQuery] int days = 30)
        {
            var result = await _vehicleService.GetRegisterationExpiryVehicles(days);
            return result.Success ? Ok(result) : BadRequest(result);

        }
        #endregion

        #region get-fitness-expiry
        [HttpGet("get-fitness-expiry")]
        public async Task<IActionResult> GetFittnessExpiryVehicles([FromQuery] int days = 30)
        {
            var result = await _vehicleService.GetFittnessExpiryVehicles(days);
            return result.Success ? Ok(result) : BadRequest(result);

        }
        #endregion

        #region Expiry Tracking (Fittness)
        [HttpGet("get-Insurance-expiry")]
        public async Task<IActionResult> GetInsuranceExpiryVehicles([FromQuery] int days = 30)
        {
            var result = await _vehicleService.GetInsuranceExpiryVehicles(days);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        #endregion


        #region change vehicle state 
        [HttpPut("change-vehicle-state")]
        public async Task<ActionResult<VehicleResponseDto>> ChangeVehicleStatus([FromRoute] string vehicleId, [FromQuery] VehicleStatus status)
        {
            var result = await _vehicleService.ChangeVehicleStatusAsync(vehicleId,status);
            return result.Success ? Ok(result) : BadRequest(result);

        }

        #endregion

        #region filtering by vehicle status
        [HttpGet("get-by-status")]
        public async Task<ActionResult> GetVehicleStatus([FromQuery] VehicleStatus vehicleStatus)
        {
            var result = await _vehicleService.GetVehicleStatus(vehicleStatus);
            return result.Success ? Ok(result) : BadRequest(result);

        }
        #endregion
    }
}
