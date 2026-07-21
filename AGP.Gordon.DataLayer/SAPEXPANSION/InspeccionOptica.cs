using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AGP.Gordon.DataAccessLayer.SAPEXPANSION;

public class InspeccionOptica
{

    public long Id { get; set; }

    public long? CertificadoId { get; set; }

    public string? Parametro { get; set; }

    public int? ParametroInspeccionId { get; set; }

    public string? Observacion { get; set; }

    public byte? TieneImagen { get; set; }

    public string? UsuarioCrea { get; set; }

    public DateTime? FechaCrea { get; set; }

    public string? UsuarioEdita { get; set; }

    public DateTime? FechaEdita { get; set; }


    public virtual ParametrosInspeccion? ParametroInspeccion { get; set; }

    [NotMapped]
    public string UrlImage { get; set; }

    [NotMapped]
    public string PathImage { get; set; }

    [NotMapped]
    public byte[]? ImageByte { get; set; }


}

