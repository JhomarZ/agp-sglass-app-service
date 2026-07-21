using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AGP.Snowden.DataAccessLayer;

[ Table("VWImputadosSap", Schema = "Warehouse")]
public partial class VwimputadosSap
{
    public long Id { get; set; }

    public string? NroPosicionDc { get; set; }

    public string? TipoImputacion { get; set; }

    public string? Centro { get; set; }

    public string? NumeroMaterial { get; set; }

    public string? DescripcionMaterial { get; set; }

    public string? CntPedido { get; set; }

    public string? Umb { get; set; }

    public string? Solicitante { get; set; }

    public string? Responsable { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public string? HoraRegistrada { get; set; }

    public string? NroCuentaProveedor { get; set; }

    public string? ResponsableErnamEkko { get; set; }

    public string? NroDocumentoComercial { get; set; }

    public string? GrupoArticulos { get; set; }

    public string? FechaInicio { get; set; }

    public string? FechaFin { get; set; }

    public string? DocumentoCompra { get; set; }

    public string? Mblnr { get; set; }

    public string? NomCompleto { get; set; }

    public int? Bultos { get; set; }

    public int? shippingStatusId { get; set; }

    public string? Status { get; set; }

    public string? StatusCode { get; set; }
    

    public string? SolicitanteNombre { get; set; }

    public int? PackingListId { get; set; }

    public string? ResponsableIngresoFullName { get; set; }

    [NotMapped]
    public string? StatusDescription { get; set; }

    [NotMapped]
    public byte[]? QR { get; set; }

    [NotMapped]
    public PackingListItem? PackinListItem { get; set; }

    [NotMapped]
    public List<ImputadoStatusHistory>? StatusHistory { get; set; }
}
