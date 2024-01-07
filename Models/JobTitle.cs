using System;
using System.Collections.Generic;

namespace Labb4IndividuelltDatabasprojekt.Models;

public partial class JobTitle
{
    public byte JobTitleId { get; set; }

    public string? JobTitle1 { get; set; }

    public virtual ICollection<Personnel> Personnel { get; set; } = new List<Personnel>();
}
