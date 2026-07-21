using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class ProcessOrigin
{
    public int Id { get; set; }

    public string? Centro { get; set; }

    public int? ProcessId { get; set; }

    public int? ProcessOriginId { get; set; }
}
