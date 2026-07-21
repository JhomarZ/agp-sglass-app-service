using System;
using System.Collections.Generic;

namespace AGP.Gordon.DataAccessLayer.SAPEXPANSION;

public partial class Defectos
{
    public int Id { get; set; }

    public string? Defecto { get; set; }

    public string? Area { get; set; }

    public byte? Activo { get; set; }

    public string? Color { get; set; }

    public string? Grupo { get; set; }

    public int? IdCompania { get; set; }

    public int? GrupoDefectoId { get; set; }

    public string? NombreIngles { get; set; }
}
