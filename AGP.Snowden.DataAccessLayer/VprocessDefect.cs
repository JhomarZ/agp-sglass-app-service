using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class VprocessDefect
{
    public int Id { get; set; }

    public string? Centro { get; set; }

    public int? ProcessId { get; set; }

    public string? ProcessName { get; set; }

    public int? DefectId { get; set; }

    public string? Defect { get; set; }
}
