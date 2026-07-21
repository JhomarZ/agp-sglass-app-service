using System;
using System.Collections.Generic;

namespace AGP.Gordon.DataAccessLayer.SAPEXPANSION;

public partial class Clasificadore
{
    public int Id { get; set; }

    public int? ClasificadorPadreId { get; set; }

    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public string? Codigo { get; set; }

    public int? Orden { get; set; }

    public byte? Activo { get; set; }

    public int? IdCompania { get; set; }
}
