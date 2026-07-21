using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AGP.Snowden.DataAccessLayer;

[Table("TrackingImputadosExtension", Schema = "Warehouse")]

public partial class TrackingImputadosExtension
{
    public long Id { get; set; }

    public string? CentroSap { get; set; }

    public string? DocumentoCompra { get; set; }

    public string? NroPosicion { get; set; }

    public string? MBLNR { get; set; }

    public int? Bultos { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public int? PackingListId { get; set; }
    
}
