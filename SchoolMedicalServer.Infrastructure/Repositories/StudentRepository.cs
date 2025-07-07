using Microsoft.EntityFrameworkCore;
using SchoolMedicalServer.Abstractions.Dtos.MedicalRegistration;
using SchoolMedicalServer.Abstractions.Entities;
using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Infrastructure.Data;
using System.Linq.Dynamic.Core;


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

        public async Task<List<Student>> GetPagedAsync(
                string? search,
                string? sortBy,
                string? sortOrder,
                int skip,
                int take)
        {
            IQueryable<Student> query = _context.Students.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var lowerSearch = search.ToLower();
                query = query.Where(s => s.FullName.ToLower().Contains(lowerSearch));
            }

            string defaultSort = "StudentCode ascending";
            string sortString = !string.IsNullOrWhiteSpace(sortBy)
                ? $"{sortBy} {(sortOrder?.ToLower() == "desc" ? "descending" : "ascending")}"
                : defaultSort;

            query = query.OrderBy(sortString);

            return await query.Skip(skip).Take(take).ToListAsync();
        }

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
                    StudentCode = s.StudentCode,
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

            return $"{prefix}{nextNumber:D4}";
        }

        public async Task<IEnumerable<Student>> GetStudentsByGradeAsync(string? targetGrade)
        {
            return await _context.Students
                .Include(s => s.HealthProfile)
                .Where(s => s.Grade == targetGrade)
                .ToListAsync();
        }

        public async Task<Student?> GetStudentByHealthProfileId(Guid? healthProfileId)
        {
            return await _context.Students
                .Include(s => s.HealthProfile)
                .FirstOrDefaultAsync(s => s.HealthProfile!.HealthProfileId == healthProfileId);
        }

        public async Task<IEnumerable<Student>> GetByIdsAsync(List<Guid> studentIds)
        {
            return await _context.Students
                .Where(s => studentIds.Contains(s.StudentId))
                .ToListAsync();
        }

        public async Task<Student?> GetStudentByUserId(Guid? userId)
        {
            return await _context.Students.Include(s => s.User).FirstOrDefaultAsync(s => s.UserId == userId);
        }
    }
}

