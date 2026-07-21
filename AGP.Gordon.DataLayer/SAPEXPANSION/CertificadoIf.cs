using System;
using System.Collections.Generic;

namespace AGP.Gordon.DataAccessLayer.SAPEXPANSION;

public partial class CertificadoIf
{
    public long Id { get; set; }

    public string? OrdProceso { get; set; }

    public string? Observacion { get; set; }

    public string? Revisa { get; set; }

    public string? Autoriza { get; set; }

    public int? NroColumnas { get; set; }

    public string? ImagenOp { get; set; }

    public string? ImagenDblZonaA { get; set; }

    public string? ImagenDblZonaB { get; set; }

    public string? ImagenDistorcion { get; set; }

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

    public byte? TerminadoDimensional { get; set; }

    public byte? TerminadoApariencia { get; set; }

    public byte? TerminadoOptico { get; set; }

    public byte? TerminadoElectrico { get; set; }

    public string? TipoPieza { get; set; }
}
