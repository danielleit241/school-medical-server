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
            if (parentId == Guid.Empty)
            {
                return BadRequest("Parent ID cannot be empty.");
            }
            var students = await service.GetParentStudentsAsync(parentId);
            if (students == null || !students.Any())
            {
                return NotFound("No students found for this parent.");
            }
            return Ok(students);
        }

        [HttpGet("parents/{parentId}/students/{studentId}")]
        [Authorize(Roles = "parent")]
        public async Task<ActionResult<StudentInformationResponse>> GetStudent(Guid parentId, Guid studentId)
        {
            if (parentId == Guid.Empty || studentId == Guid.Empty)
            {
                return BadRequest("Parent ID and Student ID cannot be empty.");
            }
            var student = await service.GetParentStudentAsync(parentId, studentId);
            if (student == null) return NotFound();

            return Ok(student);
        }
    }
}
