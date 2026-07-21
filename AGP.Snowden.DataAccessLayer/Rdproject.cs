using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class Rdproject
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public int? TechnologyId { get; set; }

    public string? ProjectLeader { get; set; }

    public string? InternalOrder { get; set; }

    public string? SubTechnology { get; set; }

    public string? Process { get; set; }

    public string? RelevantFile { get; set; }

    public string? Observation { get; set; }

    public bool? Active { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
