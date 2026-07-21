using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class ProcessDefectType
{
    public int Id { get; set; }

    public int? DefectTypeId { get; set; }

    public int? ProcessId { get; set; }

    public string? Centro { get; set; }

    public int? DefectTypeId1 { get; set; }
}
