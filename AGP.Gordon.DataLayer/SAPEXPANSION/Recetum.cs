using System;
using System.Collections.Generic;

namespace AGP.Gordon.DataAccessLayer.SAPEXPANSION;

public partial class Recetum
{
    public int Id { get; set; }

    public int? CurvadoId { get; set; }

    public string? Cabina { get; set; }

    public string? Grados { get; set; }

    public string? Operacion { get; set; }

    public string? Observacion { get; set; }

    public string? UsuarioCrea { get; set; }

    public DateTime? FechaCrea { get; set; }

    public string? UsuarioEdita { get; set; }

    public DateTime? FechaEdita { get; set; }

    public byte? Activo { get; set; }

    public string? Horno { get; set; }

    public int? CabinaId { get; set; }

    public int? HornoId { get; set; }

    public int? EstadoHerramentalId { get; set; }
}
