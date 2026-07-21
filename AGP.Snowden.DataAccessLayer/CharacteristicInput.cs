using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class CharacteristicInput
{
    public int? InspectionPlanId { get; set; }

    public int? ValidationPlanId { get; set; }

    public int? CharacteristicId { get; set; }

    public string? Name { get; set; }

    public string? Type { get; set; }

    public string? List { get; set; }

    public int? Min { get; set; }

    public int? Max { get; set; }

    public string? Center { get; set; }

    public bool? Active { get; set; }

    public string? UnitMeasure { get; set; }

    public int Id { get; set; }

    public virtual CharacteristicInspectionPlan? Characteristic { get; set; }

    public virtual InspectionPlan? InspectionPlan { get; set; }

    public virtual ValidationPlan? ValidationPlan { get; set; }
}
