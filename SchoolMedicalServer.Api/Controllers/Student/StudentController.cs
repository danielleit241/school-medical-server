using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.Student
{
    [Route("api")]
    [ApiController]
    public class StudentController(IStudentService service) : ControllerBase
    {
        [HttpGet("students")]
        [Authorize(Roles = "admin, manager")]
        public async Task<IActionResult> GetAllStudents([FromQuery] PaginationRequest? paginationRequest)
        {
            var students = await service.GetAllStudentsAsync(paginationRequest);
            if (students == null || !students.Items.Any())
            {
                return NotFound("No students found.");
            }
            return Ok(students);
        }
    }
}
