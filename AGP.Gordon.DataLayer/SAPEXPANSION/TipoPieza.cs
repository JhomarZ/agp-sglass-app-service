using System;
using System.Collections.Generic;

namespace AGP.Gordon.DataAccessLayer.SAPEXPANSION;

public partial class TipoPieza
{
    public int Id { get; set; }

    public string? Abreviatiura { get; set; }

    public string? Nombre { get; set; }

    public string? NombreIngles { get; set; }

    public byte? Activo { get; set; }
}
