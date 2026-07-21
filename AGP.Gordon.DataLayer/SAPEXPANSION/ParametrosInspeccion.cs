using System;
using System.Collections.Generic;

namespace AGP.Gordon.DataAccessLayer.SAPEXPANSION;

public  class ParametrosInspeccion
{
    public ParametrosInspeccion()
    {
        InspeccionesOpticas = new HashSet<InspeccionOptica>();
    }
    public int Id { get; set; }

    public string? Parametro { get; set; }

    public string? Modulo { get; set; }

    public int? Orden { get; set; }

    public int? NroColumnas { get; set; }

    public string? ValorXdefecto { get; set; }

    public string? MinimoValor { get; set; }

    public string? MaximoValor { get; set; }

    public string? Tipo { get; set; }

    public byte? Activo { get; set; }

    public byte? Peru { get; set; }

    public byte? Colombia { get; set; }

    public byte? Brasil { get; set; }

    public byte? Calidad { get; set; }

    public byte? Curvado { get; set; }

    public byte? ColumnaDinamica { get; set; }

    public string? ParametroIngles { get; set; }

    public int? IdCompania { get; set; }

    public byte? ParametroDefault { get; set; }

    public string? Simbolo { get; set; }

    public byte? Requerido { get; set; }

    public byte? Curvo { get; set; }

    public byte? Plano { get; set; }

    public string? TipoEtiqueta { get; set; }

    public string? ParametroPortugues { get; set; }

    public virtual ICollection<InspeccionOptica> InspeccionesOpticas { get; set; }

    public virtual ICollection<CertificadoIfdimension> InspeccionesDimensional { get; set; }

    public virtual ICollection<CertificadoIfapariencias> InspeccionesApariencia { get; set; }

}
