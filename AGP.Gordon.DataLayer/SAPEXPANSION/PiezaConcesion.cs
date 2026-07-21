using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AGP.Gordon.DataAccessLayer.SAPEXPANSION;

public partial class PiezaConcesion
{
    public int Id { get; set; }

    public DateTime? Fecha { get; set; }

    public int? Tipo { get; set; }

    public string? Observacion { get; set; }

    public string? Justificacion { get; set; }

    public DateTime? FechaCrea { get; set; }

    public string? Supervisor { get; set; }

    public string? TipoDescripcion { get; set; }

    public string? Archivo { get; set; }

    public string? Color { get; set; }

    public byte? Activo { get; set; }

    public string? AutorizadoPor { get; set; }

    public long? CertificadoId { get; set; }

    public string? OrdProceso { get; set; }

    public string? UsuarioEdita { get; set; }

    public DateTime? FechaEdita { get; set; }

    public string? UsuarioCrea { get; set; }

    public decimal? Valor { get; set; }

    public string? Defecto { get; set; }

    public decimal? Tamanio { get; set; }

    public int? DefectoId { get; set; }

    public byte? Autorizado { get; set; }

    public byte? AutorizadoFecha { get; set; }

    public int? PositionX { get; set; }

    public int? PositionY { get; set; }

    public int? ZonaId { get; set; }

    public int? MotivoId { get; set; }

    public string? Riesgo { get; set; }

    public string? Mercado { get; set; }

    [NotMapped] 
    public string Zona { get; set; }

    [NotMapped]
    public Defectos DefectoMaestro { get; set; }
}
