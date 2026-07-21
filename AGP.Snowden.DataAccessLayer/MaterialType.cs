using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class MaterialType
{
    public int Id { get; set; }

    public string? MaterialTypeGroup { get; set; }

    public string? MaterialTypeDescription { get; set; }

    public string? LevelInspectionWarehouse { get; set; }

    public string? DetailWarehouse { get; set; }

    public string? Category { get; set; }

    public string? Centro { get; set; }

    public bool? Active { get; set; }

    public string? CriticalityInspection { get; set; }

    public int? InspectionPlanId { get; set; }
}
