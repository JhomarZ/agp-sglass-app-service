using System;
using System.Collections.Generic;

namespace AGP.Gordon.DataAccessLayer.SAPEXPANSION;

public partial class Tarea
{
    public int Id { get; set; }

    public int? TipoTareaId { get; set; }

    public string? Responsable { get; set; }

    public int? CertificadoId { get; set; }

    public string? OrdProceso { get; set; }

    public string? Observacion { get; set; }

    public string? Imagen { get; set; }

    public byte? Conforme { get; set; }

    public byte? Activo { get; set; }

    public string? UsuarioCrea { get; set; }

    public DateTime? FechaCrea { get; set; }

    public string? UsuarioEdita { get; set; }

    public DateTime? FechaEdita { get; set; }

    public int? CurvadoId { get; set; }

    public int? ResponsableId { get; set; }

    public int? ResponsableAreaId { get; set; }

    public int? IdCompania { get; set; }
}
