using System;
using System.Collections.Generic;

namespace AGP.Gordon.DataAccessLayer.SAPEXPANSION;

public partial class ParametrosInspeccionTmp
{
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
}
