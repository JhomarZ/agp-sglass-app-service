using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class MotivesTypeToStop
{
    public int Id { get; set; }

    public string? Centro { get; set; }

    public string? Name { get; set; }

    public int? ProcessId { get; set; }

    public bool? Active { get; set; }

    public int? MotivesTypeToStopId { get; set; }
}
