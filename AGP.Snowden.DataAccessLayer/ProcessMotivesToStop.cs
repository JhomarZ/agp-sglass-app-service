using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class ProcessMotivesToStop
{
    public int Id { get; set; }

    public int? ProcessId { get; set; }

    public int? MotivesToStopId { get; set; }
}
