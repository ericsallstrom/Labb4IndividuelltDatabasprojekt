using System;
using System.Collections.Generic;

namespace Labb4IndividuelltDatabasprojekt.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string? CourseName { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public virtual ICollection<CourseRegistration> CourseRegistrations { get; set; } = new List<CourseRegistration>();

    public virtual ICollection<CourseTeacher> CourseTeachers { get; set; } = new List<CourseTeacher>();
}
