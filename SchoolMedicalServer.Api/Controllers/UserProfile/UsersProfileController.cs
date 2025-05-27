using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.UserProfile;
using SchoolMedicalServer.Abstractions.IServices;
using SchoolMedicalServer.Infrastructure.Services;

namespace SchoolMedicalServer.Api.Controllers.UserProfile
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
        public async Task<IActionResult> UpdateProfileAsync(Guid userId, [FromBody] UserProfileDto request)
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
    }
}


