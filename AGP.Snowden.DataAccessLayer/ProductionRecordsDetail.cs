using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class ProductionRecordsDetail
{
    public long Id { get; set; }

    public string? Centro { get; set; }

    public int? ValueRecord { get; set; }

    public bool? Active { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public string? TypeRecord { get; set; }

    public long? ProductionRecordId { get; set; }

    public int? MotiveStopedId { get; set; }

    public int? DefectId { get; set; }

    public int? ProcessOriginId { get; set; }
}
