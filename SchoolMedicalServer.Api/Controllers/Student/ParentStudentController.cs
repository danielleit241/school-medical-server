using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Student;
using SchoolMedicalServer.Abstractions.IServices;
using System.Security.Claims;

namespace SchoolMedicalServer.Api.Controllers.Student
{
    [ApiController]
    [Route("api/parents/students")]
    [Authorize(Roles = "Parent")]
    public class ParentStudentController : ControllerBase
    {
        private readonly IParentStudentService _parentStudentService;

        public ParentStudentController(IParentStudentService parentStudentService)
        {
            _parentStudentService = parentStudentService;
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParentStudentDto>>> GetStudents()
        {
            var userId = GetCurrentUserId();
            var students = await _parentStudentService.GetAllStudentsAsync(userId);
            return Ok(students);
        }

        [HttpGet("{studentId}")]
        public async Task<ActionResult<ParentStudentDto>> GetStudent(Guid studentId)
        {
            var userId = GetCurrentUserId();
            var student = await _parentStudentService.GetStudentByIdAsync(userId, studentId);
            if (student == null) return NotFound();
            return Ok(student);
        }
    }
}
