using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class TestRequestStatus
{
    public int Id { get; set; }

    public string? Centro { get; set; }

    public string? Code { get; set; }

    public string? Name { get; set; }

    public byte? Active { get; set; }

    public int? Hierarchy { get; set; }
}
