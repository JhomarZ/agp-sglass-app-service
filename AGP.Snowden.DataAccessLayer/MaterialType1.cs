using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class MaterialType1
{
    public int Id { get; set; }

    public string? Description { get; set; }

    public string? Center { get; set; }

    public bool? Active { get; set; }
}
