using AGP.Security.DataAccessLayer;
using Azure.Core.GeoJson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AGP.Snowden.DataAccessLayer;

[Table("ImputadoStatusHistory", Schema = "Warehouse")]
public partial class ImputadoStatusHistory
{
    public long Id { get; set; }

    public int? StatusId { get; set; }

    public string? PlantSap { get; set; }

    public string? PurchaseOrder { get; set; }

    public string? NroPosition { get; set; }

    public string? MBLNR { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? Observation { get; set; }

    public int? PackingListId { get; set; }

    [NotMapped]
    public virtual ShippingStatus? ShippingStatus { get; set; }

    [NotMapped]
    public  User User { get; set; }

}
