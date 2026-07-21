using System;
using System.Collections.Generic;

namespace AGP.Security.DataAccessLayer;

public partial class Company
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Code { get; set; }

    public bool? Active { get; set; }
}
