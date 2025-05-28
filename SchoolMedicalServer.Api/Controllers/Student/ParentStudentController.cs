using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Student;
using SchoolMedicalServer.Abstractions.IServices;
using System.Security.Claims;

namespace SchoolMedicalServer.Api.Controllers.Student
{
    [ApiController]
    [Route("api/parents/students")]
    [Authorize (Roles = "parent")]
    public class ParentStudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public ParentStudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetAllStudents()
        {
            var parentId = GetCurrentUserId();
            var students = await _studentService.GetAllStudentsByParentIdAsync(parentId);
            if (students == null || !students.Any())
            {
                return NotFound("No students found for this parent.");
            }
            return Ok(students);
        }

        [HttpGet("{studentId}")]
        public async Task<ActionResult<StudentDTO>> GetStudent(Guid studentId)
        {
            var parentId = GetCurrentUserId();
            var student = await _studentService.GetStudentByIdForParentAsync(parentId, studentId);
            if (student == null) {
                return NotFound($"Student with ID {studentId} not found for this parent.");
            }
            return Ok(student);
        }
    }
}
