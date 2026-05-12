using LogisticsERP.API.DTOs.Drivers;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IDriverService _service;

        public DriverController(IDriverService service, ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
            _service = service;
        }


        [HttpGet("get-all-driver")]
        public async Task<ActionResult<IEnumerable<DriverResponseDto>>> GetDrivers()
        {
            var result = await _service.GetAllDrivers();
            return Ok(result);
        }

        #region adding driver
        [HttpPost("add-driver")]
        public async Task<ActionResult<DriverResponseDto>> AddNewDriver([FromForm] DriverCreateDto driverCreateDto)
        {
            if (driverCreateDto.Photo != null)
            {
                var uploadedImage = await _cloudinaryService.UploadImage(driverCreateDto.Photo, $"Driver-documents/{driverCreateDto.FullName}");
                driverCreateDto.PhotoUrl = uploadedImage.FileUrl;
            }
            if (driverCreateDto == null)
            {
                return BadRequest("Invalid driver data.");
            }
            var result = await _service.CreateDriver(driverCreateDto);
            if (result == null)
            {
                return BadRequest("Failed to create driver.");
            }
            return Ok(result);
        }

        #endregion
        
        
        #region assingment of driver to vehicle
        [HttpPost("assign-driver-to-vehicle")]
        public async Task<ActionResult<DriverResponseDto>> AssignDriverToVehicle([FromQuery] string driverId, [FromQuery] string vehicleId)
        {
            var result = await _service.AssignDriver(driverId, vehicleId);
            if (result == null)
            {
                return BadRequest("Failed to assign driver to vehicle.");
            }
            return Ok(result);
        }
        #endregion

    }
}
