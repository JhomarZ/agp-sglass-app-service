using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class InspectionOpmsample
{
    public int Id { get; set; }

    public int? InspectionOpmaterialId { get; set; }

    public string? Lote { get; set; }

    public string? SampleNumber { get; set; }

    public int? MaterialKey { get; set; }

    public int? InspectionPlanId { get; set; }

    public int? ValidationPlanId { get; set; }

    public int? CharacteristicId { get; set; }

    public int? CharacteristicInputId { get; set; }

    public string? NameInput { get; set; }

    public string? Type { get; set; }

    public string? List { get; set; }

    public int? Min { get; set; }

    public int? Max { get; set; }

    public string? UnitMeasure { get; set; }

    public string? Center { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public string? OrderPurchase { get; set; }

    public string? UserValue { get; set; }

    public int? InspectionOpmloteId { get; set; }

    public int? DefectId { get; set; }

    public bool? HasObservation { get; set; }
}
