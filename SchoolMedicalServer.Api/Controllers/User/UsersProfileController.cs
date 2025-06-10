using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.User;
using SchoolMedicalServer.Abstractions.IServices;
using SchoolMedicalServer.Infrastructure.Services;

namespace SchoolMedicalServer.Api.Controllers.User
{
    [Route("api/user-profile")]
    [ApiController]
    public class UserProfileController(IUserProfileService userProfileService) : ControllerBase
    {

        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IActionResult> GetProfile(Guid userId)
        {
            var profile = await userProfileService.GetUserProfileByIdAsync(userId);
            if (profile == null)
            {
                return NotFound();
            }

            return Ok(profile);
        }

        [HttpPut("{userId}")]
        [Authorize]
        public async Task<IActionResult> UpdateProfileAsync(Guid userId, [FromBody] UserProfileRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await userProfileService.UpdateUserProfileAsync(userId, request);
            if (user != null)
            {
                return Ok("Update successful");
            }

            return BadRequest("Update not successful");
        }

        [HttpPut("{userId}/avatar")]
        [Authorize]
        public async Task<IActionResult> UpdateProfileImageAsync(Guid userId, [FromBody] UserProfileRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedUrl = await userProfileService.UpdateUserProfileImageAsync(userId, request);
            if (updatedUrl != null)
            {
                return Ok(updatedUrl);
            }

            return BadRequest("Update avatar not successful");
        }
    }
}


