using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class CheckListSegment
{
    public int Id { get; set; }

    public int? AuditTypeId { get; set; }

    public string? Name { get; set; }

    public string? Tag { get; set; }

    public bool? Active { get; set; }

    public int? AuditSubTypeId { get; set; }

    public int? CompanyId { get; set; }

    public string? Compania { get; set; }

    public string? Centro { get; set; }

    public int? ProductId { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? InputType { get; set; }

    public decimal? Min { get; set; }

    public decimal? Max { get; set; }

    public string? Options { get; set; }

    public bool? Functional { get; set; }

    public bool? Safety { get; set; }

    public string? Attachment { get; set; }
}
