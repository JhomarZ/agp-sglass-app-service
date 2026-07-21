using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class InspectionOpmaterial
{
    public int Id { get; set; }

    public string? OrderPurchase { get; set; }

    public int? MaterialKey { get; set; }

    public string? MaterialDescription { get; set; }

    public int? SizeLote { get; set; }

    public int? QuantitySamples { get; set; }

    public string? Observation { get; set; }

    public string? Status { get; set; }

    public string? Center { get; set; }

    public bool? Active { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public int? QuantityLote { get; set; }

    public string? MaterialType { get; set; }

    public string? MaterialGroup { get; set; }

    public string? MaterialGroupText { get; set; }

    public string? LevelInspection { get; set; }

    public int? SumLoteInsp { get; set; }

    public string? ExpirationDate { get; set; }

    public string? Almacen { get; set; }

    public string? StatusQa { get; set; }

    public string? FechaInicioInsp { get; set; }

    public int? MaterialTemplateId { get; set; }
}
