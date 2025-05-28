using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolMedicalServer.Abstractions.Dtos.Student
{
    public class ParentStudentDTO
    {
       public IEnumerable<StudentDTO> Students { get; set; } = new List<StudentDTO>();
    }
}
