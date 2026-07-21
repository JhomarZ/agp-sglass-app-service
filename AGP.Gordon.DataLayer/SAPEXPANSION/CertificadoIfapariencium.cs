using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AGP.Gordon.DataAccessLayer.SAPEXPANSION;

public partial class CertificadoIfapariencias
{
    public long Id { get; set; }

    public long? CertificadoId { get; set; }

    public int? ParametroInspeccionId { get; set; }

    public string? Parametro { get; set; }

    public string? Valor { get; set; }

    public string? Observacion { get; set; }

    public string? UsuarioCrea { get; set; }

    public DateTime? FechaCrea { get; set; }

    public string? TipoDato { get; set; }

    public string? UsuarioEdita { get; set; }

    public DateTime? FechaEdita { get; set; }

    public byte? Terminado { get; set; }

    public byte? Requerido { get; set; }

    public byte? Activo { get; set; }

    public string? MinimoValor { get; set; }

    public string? MaximoValor { get; set; }

    public virtual ParametrosInspeccion? ParametroInspeccion { get; set; }

}
