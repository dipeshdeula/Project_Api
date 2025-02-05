using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_Api.Dtos;
using Project_Api.Interfaces.AdminInterface;

namespace Project_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;

        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AdminDto>>> GetAllAdmin()
        {
            var admins = await _adminService.GetAllAdminAsync();
            return Ok(admins);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<AdminDto>> GetAdminById(int id)
        {
            var admin = await _adminService.GetAdminByIdAsync(id);
            if (admin == null)
            {
                return NotFound();
            }
            return Ok(admin);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> AddAdmin([FromForm] AdminDto adminDto)
        {
            await _adminService.AddAdminAsync(adminDto);
            return CreatedAtAction(nameof(GetAdminById), new { id = adminDto.Id }, adminDto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAdmin(int id, [FromForm] AdminDto adminDto)
        {
            if (id != adminDto.Id)
            {
                return BadRequest();
            }

            await _adminService.UpdateAdminAsync(adminDto);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            await _adminService.DeleteAdminAsync(id);
            return NoContent();
        }

    }
}
