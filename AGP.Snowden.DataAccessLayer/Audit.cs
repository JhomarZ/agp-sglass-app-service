using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class Audit
{
    public long Id { get; set; }

    public int? TypeId { get; set; }

    public int? SubTypeId { get; set; }

    public int? ProductId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? Resonsable { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public bool? Active { get; set; }

    public string? Status { get; set; }

    public string? Observation { get; set; }

    public int? CompanyId { get; set; }

    public string? Compania { get; set; }

    public string? Centro { get; set; }

    public string? Validation { get; set; }

    public string? GeneralComment { get; set; }

    public string? Zona { get; set; }

    public string? ProductionOrder { get; set; }

    public string? ValidationQuality { get; set; }

    public string? ValidationText { get; set; }

    public string? ValidationQualityText { get; set; }

    public short? HasNc { get; set; }

    public string? Shift { get; set; }
}
