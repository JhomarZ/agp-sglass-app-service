using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class Norma
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public byte? Active { get; set; }

    public string? Link { get; set; }
}
