using SchoolMedicalServer.Abstractions.IRepositories;

namespace SchoolMedicalServer.Infrastructure.Repositories
{
    public class BaseRepository(SchoolMedicalManagementContext _context) : IBaseRepository
    {
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
