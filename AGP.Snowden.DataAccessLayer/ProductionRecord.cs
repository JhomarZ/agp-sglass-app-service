using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class ProductionRecord
{
    public long Id { get; set; }

    public string? Centro { get; set; }

    public int? ProcessId { get; set; }

    public int? ProductId { get; set; }

    public string? TimeInit { get; set; }

    public string? TimeEnd { get; set; }

    public string? ShiftRecord { get; set; }

    public int? QuantityOkParts { get; set; }

    public int? NumberMoldes { get; set; }

    public bool? Active { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public string? Comment { get; set; }

    public string? DateRecord { get; set; }

    public string? ProductionOrder { get; set; }

    public int? QuantityLevelB { get; set; }

    public int? PlannedQuantity { get; set; }

    public string? Campaign { get; set; }
}
