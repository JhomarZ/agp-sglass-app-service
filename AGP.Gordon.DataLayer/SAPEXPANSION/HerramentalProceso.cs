using System;
using System.Collections.Generic;

namespace AGP.Gordon.DataAccessLayer.SAPEXPANSION;

public partial class HerramentalProceso
{
    public int Id { get; set; }

    public string? OrdProceso { get; set; }

    public string? CodHerramienta { get; set; }

    public string? Herramienta { get; set; }

    public string? Ubicacion { get; set; }

    public string? Observacion { get; set; }

    public int? PiezaSapId { get; set; }
}
