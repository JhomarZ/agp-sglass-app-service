using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class TecnosenProgram
{
    public int Id { get; set; }

    public string? Program { get; set; }

    public byte? Active { get; set; }

    public int? CompaniaId { get; set; }
}
