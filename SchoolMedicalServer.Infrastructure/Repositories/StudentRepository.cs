using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Dtos.MedicalRegistration;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;

namespace SchoolMedicalServer.Infrastructure.Repositories
{
    public class StudentRepository(SchoolMedicalManagementContext _context) : IStudentRepository
    {
        public async Task AddAsync(Student student) => await _context.Students.AddAsync(student);

        public async Task<List<Student>> GetAllAsync() => await _context.Students.ToListAsync();

        public void UpdateStudent(Student student) => _context.Students.Update(student);

        public async Task<List<string>> GetParentsPhoneNumber() => await _context.Students
                .Where(s => s.ParentPhoneNumber != null)
                .Select(s => s.ParentPhoneNumber!)
                .Distinct()
                .ToListAsync();

        public async Task<List<Student>> GetStudentsWithParentPhoneAsync() => await _context.Students
                .Where(s => s.ParentPhoneNumber != null)
                .ToListAsync();

        public async Task<int> CountAsync() => await _context.Students.CountAsync();

        public async Task<List<Student>> GetPagedAsync(int skip, int take) => await _context.Students
                .Skip(skip)
                .Take(take)
                .AsNoTracking()
                .ToListAsync();
        public async Task<List<Student>> GetByParentIdAsync(Guid parentId)
        {
            return await _context.Students
                .Include(s => s.User)
                .Where(s => s.UserId == parentId)
                .ToListAsync();
        }

        public async Task<Student?> GetByParentIdAndStudentIdAsync(Guid parentId, Guid studentId)
        {
            return await _context.Students
                .Include(s => s.User)
                .Where(s => s.UserId == parentId && s.StudentId == studentId)
                .FirstOrDefaultAsync();
        }

        public async Task<MedicalRegistrationStudentResponse?> GetStudentInfoAsync(Guid? studentId)
        {
            return await _context.Students
                .Where(s => s.StudentId == studentId)
                .Select(s => new MedicalRegistrationStudentResponse
                {
                    StudentId = s.StudentId,
                    StudentFullName = s.FullName
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<Student?> GetStudentByIdAsync(Guid? studentId)
        {
            return await _context.Students
                .Include(s => s.User)
                .FirstOrDefaultAsync(s => s.StudentId == studentId);
        }

        public async Task<Guid?> GetParentUserIdAsync(Guid? studentId)
        {
            return await _context.Students
                .Where(s => s.StudentId == studentId)
                .Select(s => s.UserId)
                .FirstOrDefaultAsync();
        }

        public async Task<Student?> FindByStudentCodeAsync(string studentCode)
        {
            return await _context.Students.FirstOrDefaultAsync(s => s.StudentCode == studentCode);
        }
        public async Task<string> GenerateStudentCodeAsync()
        {
            int currentYear = DateTime.Now.Year % 100;
            string prefix = $"SV{currentYear:D2}";

            var lastStudent = await _context.Students
                .Where(s => s.StudentCode!.StartsWith(prefix))
                .OrderByDescending(s => s.StudentCode)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastStudent != null)
            {
                string lastCode = lastStudent.StudentCode!;
                string numberPart = lastCode.Substring(4);
                nextNumber = int.Parse(numberPart) + 1;
            }

            return $"{prefix}{nextNumber:D5}";
        }

    }
}

