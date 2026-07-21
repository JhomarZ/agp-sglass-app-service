using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AGP.Snowden.DataAccessLayer;


[Table("PackingListItem", Schema = "Warehouse")]

public partial class PackingListItem
{
    public int Id { get; set; }

    public int? PackingListId { get; set; }

    public int? ShippingStatusId { get; set; }

    public string? CentroSap { get; set; }

    public string? DocumentoCompra { get; set; }

    public string? NroPosicion { get; set; }

    public string? Mblnr { get; set; }

    public int? Bultos { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Observation { get; set; }

    public string? ImageA { get; set; }

    public string? DocumentNumberReceiver { get; set; }
    

    [NotMapped]
    public virtual ShippingStatus? ShippingStatus { get; set; }

    [NotMapped]
    public VwimputadosSap? TrackingImputadosSap { get; set; } = new VwimputadosSap();

    [NotMapped]
    public virtual Personal? PersonReceiver { get; set; }

}
