using System;
using System.Collections.Generic;

namespace AGP.Gordon.DataAccessLayer.SAPEXPANSION;

public partial class Curvado
{
    public int Id { get; set; }

    public string? OrdProceso { get; set; }

    public string? Observacion { get; set; }

    public string? Revisa { get; set; }

    public string? Autoriza { get; set; }

    public string? ImagenOp { get; set; }

    public DateTime? FechaTermino { get; set; }

    public byte? Termino { get; set; }

    public string? UsuarioCrea { get; set; }

    public DateTime? FechaCrea { get; set; }

    public string? UsuarioEdita { get; set; }

    public DateTime? FechaEdita { get; set; }

    public byte? Activo { get; set; }

    public string? Zfer { get; set; }

    public int? MotivoAnulacionId { get; set; }

    public int? IdCompania { get; set; }
}
