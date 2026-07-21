using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class Center
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Centre { get; set; }

    public string? Company { get; set; }

    public byte? Active { get; set; }

    public string? Continent { get; set; }
}
