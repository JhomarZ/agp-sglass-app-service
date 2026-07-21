using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class ProcessDefect
{
    public int Id { get; set; }

    public int? ProcessId { get; set; }

    public int? DefectId { get; set; }

    public string? Centro { get; set; }
}
