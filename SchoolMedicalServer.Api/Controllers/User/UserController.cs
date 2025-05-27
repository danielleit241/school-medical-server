using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.User;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.User
{
    [Route("api/users")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await userService.GetAllAsync();
            if (users == null || users.Count == 0)
            {
                return NotFound("No users found.");
            }
            return Ok(users);
        }

        [HttpGet("{userId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUsers(Guid userId)
        {
            var users = await userService.GetUserAsync(userId);
            if (users == null)
            {
                return NotFound("No users found.");
            }
            return Ok(users);
        }

        [HttpPut("{userid}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateUser(Guid userid, UserDto request)
        {
            var isUpdated = await userService.UpdateUserAsync(userid, request);
            if (!isUpdated)
            {
                return NotFound();
            }
            return Ok(isUpdated);
        }
    }
}
