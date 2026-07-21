using System;
using System.Collections.Generic;

namespace AGPSnowden.Model.Scada;

public partial class SapOrder
{
    public int Id { get; set; }

    public string? Orden { get; set; }

    public string? Correlativo { get; set; }

    public string? Usuario { get; set; }

    public string? Hora { get; set; }

    public string? Fecha { get; set; }

    public string? Linea { get; set; }
}
