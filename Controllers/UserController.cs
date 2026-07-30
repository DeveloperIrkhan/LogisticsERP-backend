using LogisticsERP.API.DTOs.User;
using LogisticsERP.API.enums;
using LogisticsERP.API.Helpers;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LogisticsERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // every endpoint here requires a logged-in user unless overridden below
    public class UserController : ControllerBase
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IUserService _service;

        public UserController(IUserService service, ICloudinaryService cloudinaryService)
        {
            _cloudinaryService = cloudinaryService;
            _service = service;
        }

        [HttpGet("get-all-users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("get-user/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("pending-approvals")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetPendingApprovals()
        {
            var result = await _service.GetPendingApprovalsAsync();
            return Ok(result);
        }

        [HttpGet("get-by-status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetByStatus([FromQuery] UserStatus status)
        {
            var result = await _service.GetByStatusAsync(status);
            return Ok(result);
        }

        [HttpPut("approve/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve([FromRoute] string id, [FromBody] ApproveUserDto dto)
        {
            if (dto == null) return BadRequest("Approval data is required.");
            var result = await _service.ApproveAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("reject/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reject([FromRoute] string id, [FromBody] RejectUserDto dto)
        {
            if (dto == null) return BadRequest("Rejection data is required.");
            var result = await _service.RejectAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("deactivate/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Deactivate([FromRoute] string id)
        {
            var result = await _service.DeactivateAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("reactivate/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reactivate([FromRoute] string id)
        {
            var result = await _service.ReactivateAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("update-role/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRole([FromRoute] string id, [FromBody] UpdateUserRoleDto dto)
        {
            if (dto == null) return BadRequest("Role data is required.");
            var result = await _service.UpdateRoleAsync(id, dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("delete-user/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var result = await _service.DeleteAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        // ── Self-service (any logged-in user) ───────────────────
        [HttpGet("my-profile")]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var result = await _service.GetByIdAsync(userId);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPut("update-my-profile")]
        public async Task<IActionResult> UpdateMyProfile([FromForm] UpdateUserProfileDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();
            if (dto == null) return BadRequest("Profile data is required.");
            string pictureUrl = "";
            if (dto.Avatar!= null)
            {
                var uploadImage = await _cloudinaryService.UploadImage(dto.Avatar,
                    $"user-profile/{dto.FullName}");
                pictureUrl = uploadImage.FileUrl;
            }
            var result = await _service.UpdateProfileAsync(userId, dto,pictureUrl);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }

}
