using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Student;
using SchoolMedicalServer.Abstractions.IServices;

namespace SchoolMedicalServer.Api.Controllers.Student
{
    [ApiController]
    [Route("api")]
    public class ParentStudentController(IParentStudentService service) : ControllerBase
    {
        [HttpGet("parents/{parentId}/students")]
        [Authorize(Roles = "parent")]
        public async Task<IActionResult> GetParentStudents(Guid parentId)
        {
            var students = await service.GetParentStudentsAsync(parentId);
            if (students == null || !students.Any())
            {
                return NotFound("No students found for this parent.");
            }
            return Ok(students);
        }

        [HttpGet("parents/{parentId}/students/{studentId}")]
        [Authorize(Roles = "parent")]
        public async Task<ActionResult<StudentDto>> GetStudent(Guid parentId, Guid studentId)
        {

            var student = await service.GetParentStudentAsync(parentId, studentId);
            if (student == null) return NotFound();

            return Ok(student);
        }
    }
}
