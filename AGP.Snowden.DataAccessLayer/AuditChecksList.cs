using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class AuditChecksList
{
    public long Id { get; set; }

    public long? AuditId { get; set; }

    public long? CheckId { get; set; }

    public string? Value { get; set; }

    public string? ImageA { get; set; }

    public string? ImageB { get; set; }

    public string? Observation { get; set; }

    public string? CheckName { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? Active { get; set; }

    public string? Tag { get; set; }

    public string? InputType { get; set; }

    public decimal? Min { get; set; }

    public decimal? Max { get; set; }

    public string? Options { get; set; }

    public string? ObservationSupervisor { get; set; }

    public string? ObservationQuality { get; set; }

    public string? Responsable { get; set; }

    public bool? Functional { get; set; }

    public bool? Safety { get; set; }

    public string? Attachment { get; set; }
}
