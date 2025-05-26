using SchoolMedicalServer.Abstractions.Dtos.User;
using SchoolMedicalServer.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedicalServer.Abstractions.IServices
{
    public interface IUserService
    {
        Task<List<UserDTO?>> GetAllAsync();
        
        //Task<UserDTO?> UpdateUserAsync(Guid userId);


    }
}
