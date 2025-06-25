namespace SchoolMedicalServer.Api.Controllers.User
{
    [Route("api/users")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        [HttpGet("free-nurses")]
        [Authorize]
        public async Task<IActionResult> GetFreeNurses()
        {
            var nurses = await userService.GetFreeNursesAsync();
            if (nurses == null)
            {
                return NotFound("No free nurses found.");
            }
            return Ok(nurses);
        }


        [HttpGet("roles/{roleName}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUsersByRoleName([FromQuery] PaginationRequest? paginationRequest, string roleName)
        {
            var users = await userService.GetUsersByRoleNamePaginationAsync(paginationRequest, roleName);
            if (users == null)
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
        public async Task<IActionResult> UpdateUser(Guid userid, UserInformation request)
        {
            var isUpdated = await userService.UpdateUserAsync(userid, request);
            if (!isUpdated)
            {
                return BadRequest();
            }
            return Ok(isUpdated);
        }

        [HttpDelete("{userid}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateStatusUser(Guid userid, [FromBody] bool status)
        {
            var isBaned = await userService.UpdateStatusUserAsync(userid, status);
            if (!isBaned)
                return BadRequest();
            return Ok(isBaned);
        }
    }
}
