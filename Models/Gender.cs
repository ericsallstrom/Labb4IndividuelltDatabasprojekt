using System;
using System.Collections.Generic;

namespace Labb4IndividuelltDatabasprojekt.Models;

public partial class Gender
{
    public byte GenderId { get; set; }

    public string? TypeOfGender { get; set; }

    public virtual ICollection<Personnel> Personnel { get; set; } = new List<Personnel>();

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
