using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolMedicalServer.Abstractions.Dtos.Pagination;
using SchoolMedicalServer.Abstractions.Dtos.Student;
using SchoolMedicalServer.Abstractions.IServices;
using SchoolMedicalServer.Infrastructure.Services;

namespace SchoolMedicalServer.Api.Controllers.Student
{
    [Route("api")]
    [ApiController]
    public class StudentController(IStudentService service) : ControllerBase
    {
        [HttpGet("students")]
        [Authorize(Roles = "admin, manager, nurse")]
        public async Task<IActionResult> GetAllStudents([FromQuery] PaginationRequest? paginationRequest)
        {
            var students = await service.GetAllStudentsAsync(paginationRequest);
            if (students == null || !students.Items.Any())
            {
                return NotFound("No students found.");
            }
            return Ok(students);
        }


        [HttpPut("{studentId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateStudentInformationAsync(Guid studentId, [FromBody] StudentInformationResponse request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (studentId != request.StudentId)
            {
                return BadRequest("Student ID mismatch between route and body.");
            }

            var student = await service.UpdateStudentInformationAsync(studentId, request);
            if (student != null)
            {
                return Ok("Update successful");
            }

            return BadRequest("Update not successful");
        }
    }
}
