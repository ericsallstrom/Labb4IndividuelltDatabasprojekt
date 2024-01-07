using System;
using System.Collections.Generic;

namespace Labb4IndividuelltDatabasprojekt.Models;

public partial class CourseRegistration
{
    public int CourseRegId { get; set; }

    public int? FkStudentId { get; set; }

    public int? FkCourseId { get; set; }

    public virtual Course? FkCourse { get; set; }

    public virtual Student? FkStudent { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
