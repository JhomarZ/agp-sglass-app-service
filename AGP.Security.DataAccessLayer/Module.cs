using System;
using System.Collections.Generic;

namespace AGP.Security.DataAccessLayer;

public partial class Module
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool? Active { get; set; }

    public bool? EmbedPbi { get; set; }

    public int? ReportPbiId { get; set; }

    public string? Controller { get; set; }

    public string? Icon { get; set; }

    public string? Description { get; set; }

    public int? Sort { get; set; }

    public bool? IsParent { get; set; }

    public int? SystemId { get; set; }

    public string? Compania { get; set; }

    public string? Centro { get; set; }

    public int? ModuleId { get; set; }
}
