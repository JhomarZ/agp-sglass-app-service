using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class MotivesToStop
{
    public int Id { get; set; }

    public string? Centro { get; set; }

    public string? Name { get; set; }

    public int? MotivesTypeToStopId { get; set; }

    public int? MotivesTypeToStopId1 { get; set; }

    public bool? Active { get; set; }

    public string? TagAvo { get; set; }
}
