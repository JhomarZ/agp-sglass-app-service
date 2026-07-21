using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class VprocessProduct
{
    public int Id { get; set; }

    public string? Centro { get; set; }

    public int? ProcessId { get; set; }

    public string? ProcessName { get; set; }

    public int? ProductId { get; set; }

    public string? ProductName { get; set; }
}
