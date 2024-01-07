using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb4IndividuelltDatabasprojekt.Models
{
    public class StudentInformation
    {
        public int StudentId { get; set; }
        public string? FirstName { get; set; }
        public string? Surname { get; set; }
        public string? SSN { get; set; }
        public string? Gender { get; set; }
        public string? PhoneNr { get; set; }
        public string? Email { get; set; }
        public string? ClassName { get; set; }
        public string? Branch { get; set; }
        public string? Mentor { get; set; }
        public string? Course { get; set; }
    }
}
