using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceSitoPanel.src.dtos.users;
using ServiceSitoPanel.src.interfaces;
using System.Security.Claims;

namespace ServiceSitoPanel.src.controllers
{
    [Route("api/settings")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            // Extract user ID from token claims
            var userIdClaim = User.FindFirst("id");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                 return Unauthorized(new { message = "Token inválido" });
            }

            var result = await _settingsService.ChangePassword(changePasswordDto, userId);

            if (!result.Flag) return BadRequest(result);

            return Ok(result);
        }
    }
}
