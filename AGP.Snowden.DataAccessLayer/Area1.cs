using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class Area1
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public short? Active { get; set; }

    public int? CompanyId { get; set; }

    public string? Compania { get; set; }

    public string? Centro { get; set; }

    public string? Zona { get; set; }
}
