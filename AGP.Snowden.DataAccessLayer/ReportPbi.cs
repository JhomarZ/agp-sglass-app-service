using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class ReportPbi
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? WorkspaceId { get; set; }

    public string? AplicationId { get; set; }

    public string? ReportId { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int SystemId { get; set; }

    public string? Compania { get; set; }

    public string? Centro { get; set; }

    public bool? Active { get; set; }
}
