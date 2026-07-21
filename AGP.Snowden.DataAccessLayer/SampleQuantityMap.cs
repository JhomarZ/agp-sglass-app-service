using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class SampleQuantityMap
{
    public int Id { get; set; }

    public int? Minumo { get; set; }

    public int? Maximo { get; set; }

    public int? Quantity { get; set; }

    public string? CriticalityInspection { get; set; }
}
