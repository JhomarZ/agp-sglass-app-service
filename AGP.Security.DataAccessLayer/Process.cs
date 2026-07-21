using System;
using System.Collections.Generic;

namespace AGP.Security.DataAccessLayer;

public partial class Process
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool? Active { get; set; }

    public string? Compania { get; set; }

    public string? Center { get; set; }

    public int? AreaId { get; set; }
}
