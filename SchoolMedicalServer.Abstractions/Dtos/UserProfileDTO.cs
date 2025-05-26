using SchoolMedicalServer.Abstractions.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedicalServer.Abstractions.Dtos
{
    public class UserProfileDTO
    {
     

    
        public string FullName { get; set; } = default !;

      
       


        public string Email { get; set; } = default !;

    }
}
