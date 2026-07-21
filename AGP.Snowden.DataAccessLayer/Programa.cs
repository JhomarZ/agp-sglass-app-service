using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class Programa
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public byte? Activo { get; set; }

    public string? Centro { get; set; }
}
