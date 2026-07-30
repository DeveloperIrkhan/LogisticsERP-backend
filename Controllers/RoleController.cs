using LogisticsERP.API.DTOs.Roles;
using LogisticsERP.API.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsERP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _service;

        public RoleController(IRoleService service)
        {
            _service = service;
        }

        // Any logged-in user can view roles (needed for sign-up-adjacent screens / admin dropdowns).
        [HttpGet("get-all-roles")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpPost("add-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] RoleCreateDto dto)
        {
            if (dto == null) return BadRequest("Role data is required.");
            var result = await _service.CreateAsync(dto);
            return result.Success ? Ok(result) : BadRequest(result);
        }

    }
}
