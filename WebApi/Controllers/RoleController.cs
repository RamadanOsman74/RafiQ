using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models.Responses;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<RolesController> _logger;

        public RolesController(IRoleService roleService, ILogger<RolesController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignRole([FromBody] RoleAssignmentDto dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.UserId) || string.IsNullOrWhiteSpace(dto.Role))
                    return BadRequest(ApiResponse.Fail("UserId and Role are required."));

                await _roleService.AddUserToRoleAsync(dto.UserId, dto.Role);

                return Ok(ApiResponse.Ok($"Role '{dto.Role}' assigned to user {dto.UserId}."));
            }
            catch (ArgumentException ex)
            {
                return NotFound(ApiResponse.Fail(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse.Fail(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while assigning role.");
                return StatusCode(500, ApiResponse.Fail("An unexpected error occurred."));
            }
        }


        [HttpGet("{role}")]
        public async Task<IActionResult> GetUsersInRole(string role)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(role))
                    return BadRequest(ApiResponse.Fail("Role name is required."));

                var users = await _roleService.GetUsersByRoleAsync(role);

                if (users == null || !users.Any())
                    return NotFound(ApiResponse.Fail($"No users found in role '{role}'"));

                return Ok(users); // You can also wrap in ApiResponse<T> if needed
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching users for role: {role}");
                return StatusCode(500, ApiResponse.Fail("An unexpected error occurred."));
            }
        }

    }
}
