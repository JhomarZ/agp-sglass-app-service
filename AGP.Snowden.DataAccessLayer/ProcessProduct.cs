using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class ProcessProduct
{
    public int Id { get; set; }

    public int? ProcessId { get; set; }

    public int? ProductId { get; set; }
}
