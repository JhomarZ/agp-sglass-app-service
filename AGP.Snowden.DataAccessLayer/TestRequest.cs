using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class TestRequest
{
    public long Id { get; set; }

    public string? Centro { get; set; }

    public int? CustomerId { get; set; }

    public string? Customer { get; set; }

    public string? Name { get; set; }

    public string? Reference { get; set; }

    public string? PartDescription { get; set; }

    public string? PartNumber { get; set; }

    public string? ProjectName { get; set; }

    public int? ProjectManagerId { get; set; }

    public int? Quantity { get; set; }

    public string? SampleDateBatch { get; set; }

    public string? StackCode { get; set; }

    public int? EssayId { get; set; }

    public string? SizeProbeta { get; set; }

    public int? QuantityCycles { get; set; }

    public decimal? Evaluation { get; set; }

    public string? Standard { get; set; }

    public string? NewTest { get; set; }

    public decimal? TotalDuration { get; set; }

    public string? Observation { get; set; }

    public byte? Active { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Status { get; set; }

    public int? NormaId { get; set; }

    public int? TechnicalId { get; set; }

    public string? StackInfo { get; set; }

    public string? TagId { get; set; }

    public decimal? AssemblyMoisture { get; set; }

    public decimal? AssemblyTemperature { get; set; }

    public string? MaterialSap { get; set; }

    public string? KeyWord { get; set; }

    public string? Link { get; set; }
}
