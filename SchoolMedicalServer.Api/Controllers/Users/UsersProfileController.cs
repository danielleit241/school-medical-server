using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos;
using SchoolMedicalServer.Abstractions.IServices;
using SchoolMedicalServer.Infrastructure.Services;

namespace SchoolMedicalServer.Api.Controllers.Users
{
    [Route("api/user-profile")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }
        [HttpGet("{userId}")]
        [Authorize]
        public async Task<ActionResult<UserProfileDTO>> GetUserProfileByIdAsync(Guid userId)
        {
            var profile = await _userProfileService.GetUserProfileByIdAsync(userId);
            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }


        [HttpPut("{userId}")]
        [Authorize]
        public async Task<IActionResult> UpdateUserProfileAsync(Guid userId, [FromBody] UserProfileDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userProfileService.UpdateUserProfileAsync(userId, dto);
            if (user != null) // Fix: Check if the returned user object is not null
            {
                return Ok("Update successful");
            }

            return BadRequest("Update not successful");
        }
    }
}

    
