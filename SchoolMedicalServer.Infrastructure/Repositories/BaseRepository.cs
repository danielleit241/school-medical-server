using SchoolMedicalServer.Abstractions.IRepositories;
using SchoolMedicalServer.Infrastructure.Data;

namespace SchoolMedicalServer.Infrastructure.Repositories
{
    public class BaseRepository(SchoolMedicalManagementContext _context) : IBaseRepository
    {
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
    }
}
