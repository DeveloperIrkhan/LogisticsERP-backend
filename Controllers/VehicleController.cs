using LogisticsERP.API.DTOs.Vehicle;
using LogisticsERP.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsERP.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly VehicleService _service;

        public VehicleController(VehicleService vehicleService)
        {
            _service = vehicleService;
        }


        [HttpGet("get-all-vehicle")]
        public async Task<ActionResult<VehicleResponseDto>> GetAllVehicle()
        {
            var vehicles = await _service.GetAllVehicles();
            if (vehicles == null)
            {
                return NotFound();
            }
            return Ok(vehicles);
        }

        [HttpPost("add-vehicle")]
        public async Task<ActionResult<VehicleResponseDto>> AddVehicle([FromBody] VehicleCreateDto vehicleCreateDto)
        {
            if (vehicleCreateDto == null)
            {
                return BadRequest("Invalid vehicle data.");
            }
            var createdVehicle = await _service.CreateVehicle(vehicleCreateDto);
            return CreatedAtAction(nameof(GetAllVehicle), new { id = createdVehicle.VehicleId }, createdVehicle);
        }


        [HttpGet("get-vehicle-by-id")]
        public async Task<ActionResult<VehicleResponseDto>> GetVehicleById(string id)
        {
            var vehicle = await _service.GetVehicleById(id);
            if (vehicle == null)
            {
                return NotFound();
            }
            return Ok(vehicle);
        }
        [HttpPut("update-vehicle")]
        public async Task<ActionResult<VehicleResponseDto>> UpdateVehicle([FromBody] VehicleUpdateDto vehicleUpdateDto)
        {
            //ArgumentNullException.ThrowIfNull(id, nameof(id));
            if (vehicleUpdateDto == null)
            {
                return BadRequest("Invalid vehicle data.");
            }
            var updatedVehicle = await _service.UpdateVehicle(vehicleUpdateDto);
            if (updatedVehicle == null)
            {
                return NotFound();
            }
            return Ok(updatedVehicle);
        }
    }
}
