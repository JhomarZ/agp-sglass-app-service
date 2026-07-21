using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class Technology
{
    public int Id { get; set; }

    public string? Centro { get; set; }

    public string? NameTechnology { get; set; }

    public bool? Active { get; set; }

    public string? Code { get; set; }

    public virtual ICollection<SpectroRequest> SpectroRequests { get; } = new List<SpectroRequest>();
}
