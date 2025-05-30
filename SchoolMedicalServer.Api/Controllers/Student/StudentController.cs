using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Dtos.Student;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.Student
{
    [Route("api/students")]
    [ApiController]
    public class StudentController(IStudentService service) : ControllerBase    
    {
        [HttpGet]
        [Authorize(Roles = "admin, manager")]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllStudents([FromQuery] PaginationRequest? paginationRequest)
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
