using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class MeasurementStatus
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }
}
