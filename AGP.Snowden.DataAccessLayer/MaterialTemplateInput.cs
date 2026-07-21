using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class MaterialTemplateInput
{
    public int? MaterialTemplateId { get; set; }

    public int? MaterialId { get; set; }

    public int? InspectionPlanId { get; set; }

    public int? ValidationPlanId { get; set; }

    public int? CharacteristicId { get; set; }

    public int? CharacteristicInputId { get; set; }

    public string? NameInput { get; set; }

    public string? Type { get; set; }

    public string? List { get; set; }

    public int? Min { get; set; }

    public int? Max { get; set; }

    public string? Center { get; set; }

    public bool? Active { get; set; }

    public string? UnitMeasure { get; set; }

    public int Id { get; set; }
}
