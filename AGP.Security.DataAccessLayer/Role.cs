using System;
using System.Collections.Generic;

namespace AGP.Security.DataAccessLayer;

public partial class Role
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Company { get; set; }

    public string? Center { get; set; }
}
