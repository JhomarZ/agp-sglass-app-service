using System;
using System.Collections.Generic;

namespace AGP.Gordon.DataAccessLayer.SAPEXPANSION;

public partial class PiezaBloqueadum
{
    public int Id { get; set; }

    public string? OrdenProduccion { get; set; }

    public string? Zfer { get; set; }

    public int? BloqueoId { get; set; }

    public int? CompanyId { get; set; }

    public byte? Activo { get; set; }

    public DateTime? CreadoEl { get; set; }

    public string? CreadoPor { get; set; }

    public DateTime? ActualizadoEl { get; set; }

    public string? ActualizadoPor { get; set; }
}
