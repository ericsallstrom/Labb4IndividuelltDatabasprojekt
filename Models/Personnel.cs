using System;
using System.Collections.Generic;

namespace Labb4IndividuelltDatabasprojekt.Models;

public partial class Personnel
{
    public int PersonnelId { get; set; }

    public string? FirstName { get; set; }

    public string? Surname { get; set; }

    public string? Ssn { get; set; }

    public byte? FkJobTitleId { get; set; }

    public byte? FkGenderId { get; set; }
    public int? FkDepartmentId { get; set; }

    public string? PhoneNr { get; set; }

    public string? Email { get; set; }

    public DateTime EmploymentDate { get; set; }
    public decimal Salary { get; set; }

    public virtual ICollection<CourseTeacher> CourseTeachers { get; set; } = new List<CourseTeacher>();

    public virtual Gender? FkGender { get; set; }

    public virtual JobTitle? FkJobTitle { get; set; }
    public virtual Department? FkDepartment { get; set; }
}
