using System;
using System.Collections.Generic;

namespace Labb4IndividuelltDatabasprojekt.Models;

public partial class ClassMentor
{
    public int? FkPersonnelId { get; set; }

    public int? FkClassId { get; set; }

    public virtual ClassList? FkClass { get; set; }

    public virtual Personnel? FkPersonnel { get; set; }
}
