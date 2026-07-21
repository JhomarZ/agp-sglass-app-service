using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class AuditType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public bool? Active { get; set; }

    public int? CompanyId { get; set; }

    public string? Compania { get; set; }

    public string? Centro { get; set; }

    public string? Zona { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
