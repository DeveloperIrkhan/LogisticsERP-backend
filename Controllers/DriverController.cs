using LogisticsERP.API.DTOs.Drivers;
using LogisticsERP.API.DTOs.Vehicle;
using LogisticsERP.API.enums;
using LogisticsERP.API.interfaces;
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

        #region Getting all drivers
        [HttpGet("get-all-drivers")]
        public async Task<ActionResult<IEnumerable<DriverResponseDto>>> GetDrivers()
        {
            var result = await _service.GetAllDrivers();
            return result.Success ? Ok(result) : BadRequest(result);
        }
        #endregion

        #region add driver
        [HttpPost("add-driver")]
        public async Task<ActionResult<DriverResponseDto>> AddNewDriver([FromForm] DriverCreateDto driverCreateDto)
        {
            if (driverCreateDto == null) return BadRequest("Driver data is required.");

            string? PhotoUrl = "";
            string? LicenseUrl = "";
            if (driverCreateDto.Photo != null)
            {
                var uploadedImage = await _cloudinaryService.UploadImage(driverCreateDto.Photo, $"Drivers-documents/{driverCreateDto.FullName}");
                PhotoUrl = uploadedImage.FileUrl;
            }
            if (driverCreateDto.License != null)
            {
                var uploadingImage = await _cloudinaryService.UploadImage(driverCreateDto.License, $"Drivers-Avator/{driverCreateDto.FullName}");
                LicenseUrl = uploadingImage.FileUrl;
            }

            var result = await _service.CreateDriver(driverCreateDto, PhotoUrl, LicenseUrl);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        #endregion

        #region delete Driver
        [HttpDelete("delete-driver/{driverId}")]
        public async Task<ActionResult<VehicleResponseDto>> DeleteDriver([FromRoute] string driverId)
        {
            if (string.IsNullOrEmpty(driverId))
                return BadRequest("Invalid vehicle Id or pass Id");

            var result = await _service.DeleteDriver(driverId);
            return result.Success ? Ok(result) : NotFound(result);
        }
        #endregion

        #region driver Update
        [HttpPut("update-driver")]
        //done
        public async Task<ActionResult<DriverResponseDto>> UpdateDriver([FromForm] DriverUpdateDto driverUpdateDto)
        {
            if (driverUpdateDto == null)
                return BadRequest("Invalid driver data.");


            string? PhotoUrl = null;
            string? LicenseUrl = null;
            if (driverUpdateDto.Photo != null)
            {
                var uploadedImage = await _cloudinaryService.UploadImage(driverUpdateDto.Photo, $"Drivers-documents/{driverUpdateDto.FullName}");
                PhotoUrl = uploadedImage.FileUrl;
            }
            if (driverUpdateDto.License != null)
            {
                var uploadingImage = await _cloudinaryService.UploadImage(driverUpdateDto.License, $"Drivers-Avator/{driverUpdateDto.FullName}");
                LicenseUrl = uploadingImage.FileUrl;
            }

            var result = await _service.UpdateDriver(driverUpdateDto, PhotoUrl, LicenseUrl);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        #endregion

        #region Getting only driver by Id
        [HttpGet("get-driver-by-id/{id}")]
        public async Task<ActionResult<VehicleResponseDto>> GetVehicleById([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest("Driver Id is required.");
            var driver = await _service.GetDriverById(id);
            return driver == null ? NotFound() : Ok(driver);
        }
        #endregion

        #region assingment of driver to vehicle
        [HttpPost("assign-driver-to-vehicle")]
        public async Task<ActionResult<DriverResponseDto>> AssignDriverToVehicle([FromQuery] string driverId, [FromQuery] string vehicleId)
        {
            var result = await _service.AssignDriver(driverId, vehicleId);
            return result == null ? BadRequest(result) : Ok(result);
        }
        #endregion

        #region unassign-drivers using vehicle-Id
        [HttpPost("unassign-driver/{driverId}")]
        public async Task<IActionResult> UnassignDriver([FromRoute] string driverId)
        {
            var result = await _service.UnassignDriver(driverId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        #endregion

        #region drivers-list
        [HttpGet("drivers-list-for-specfic-vehicle/{vehicleId}")]
        public async Task<IActionResult> DriverList([FromRoute]string vehicleId)
        {
            var result = await _service.DriverListAssignedToSpecficVehicle(vehicleId);
            return result.Success ? Ok(result) : NotFound(result);
        }
        #endregion

        #region available-drivers

        [HttpGet("available-drivers")]
        public async Task<IActionResult> GetAvailableDrivers()
        {
            var result = await _service.GetAvailableDriversAsync();
            return result.Success ? Ok(result) : BadRequest(result);
        }
        #endregion

        
        [HttpGet("drivers-by-status")]
        public async Task<IActionResult> GetDriversByStatus([FromQuery] DriverStatus status)
        {
            var result = await _service.GetDriversByStatusAsync(status);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("is-available/{driverId}")]
        public async Task<IActionResult> IsDriverAvailable([FromRoute] string driverId)
        {
            var result = await _service.IsDriverAvailableAsync(driverId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPut("change-status/{driverId}")]
        public async Task<IActionResult> ChangeStatus([FromRoute] string driverId, [FromQuery] DriverStatus status)
        {
            var result = await _service.ChangeDriverStatusAsync(driverId, status);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        // ── DUTY TRACKING ─────────────────────────────────────────
        [HttpGet("duty-stats/{driverId}")]
        public async Task<IActionResult> GetDutyStats([FromRoute] string driverId)
        {
            var result = await _service.GetDriverDutyStatsAsync(driverId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        // ── ALERTS ────────────────────────────────────────────────
        [HttpGet("expiring-licenses")]
        public async Task<IActionResult> GetExpiringLicenses([FromQuery] int days = 30)
        {
            var result = await _service.GetExpiringLicensesAsync(days);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("expiring-cnic")]
        public async Task<IActionResult> GetExpiringCnic([FromQuery] int days = 30)
        {
            var result = await _service.GetExpiringLicensesAsync(days);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
