using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class Centro
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Code { get; set; }

    public bool? Active { get; set; }

    public int CompanyId { get; set; }
}
