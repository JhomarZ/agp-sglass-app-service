using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AGP.Gordon.DataAccessLayer.SAPEXPANSION;

public partial class CertificadoIfdimension
{
    public long Id { get; set; }

    public long? CertificadoId { get; set; }

    public int? ParametroInspeccionId { get; set; }

    public string? Parametro { get; set; }

    public string? Val1 { get; set; }

    public string? Val2 { get; set; }

    public string? Val3 { get; set; }

    public string? Val4 { get; set; }

    public string? Val5 { get; set; }

    public string? Val6 { get; set; }

    public string? Val7 { get; set; }

    public string? Val8 { get; set; }

    public string? Val9 { get; set; }

    public string? Val10 { get; set; }

    public string? Val11 { get; set; }

    public string? Val12 { get; set; }

    public string? Val13 { get; set; }

    public string? Val14 { get; set; }

    public string? Val15 { get; set; }

    public string? Val16 { get; set; }

    public string? Val17 { get; set; }

    public string? Val18 { get; set; }

    public string? Val19 { get; set; }

    public string? Val20 { get; set; }

    public string? Val21 { get; set; }

    public string? Val22 { get; set; }

    public string? Val23 { get; set; }

    public string? Val24 { get; set; }

    public string? Val25 { get; set; }

    public string? UsuarioCrea { get; set; }

    public DateTime? FechaCrea { get; set; }

    public string? UsuarioEdita { get; set; }

    public DateTime? FechaEdita { get; set; }

    public byte? Terminado { get; set; }

    public byte? Requerido { get; set; }

    public int? NroColumnas { get; set; }

    public byte? Activo { get; set; }

    public int? Origen { get; set; }

    public byte? ColumnaDinamica { get; set; }

    public string? MinimoValor { get; set; }

    public string? MaximoValor { get; set; }

    public string? Modulo { get; set; }

    public byte? FueraRango { get; set; }

    public string? Observacion { get; set; }

    public byte? NoAplica { get; set; }

//    public virtual ParametrosInspeccion? parametro { get; set; }

    public virtual ParametrosInspeccion? ParametroInspeccion { get; set; }

    //    [NotMapped]
    //  public ParametrosInspeccion parametro { get; set; }
}
