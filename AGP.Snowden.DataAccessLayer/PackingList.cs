using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AGP.Snowden.DataAccessLayer;

[Table("PackingList", Schema = "Warehouse")]
public partial class PackingList
{
    public int Id { get; set; }

    public int? ShippingStatusId { get; set; }

    public string? Code { get; set; }

    public string? Plant { get; set; }

    public string? PlantDestination { get; set; }

    public string? GuideNumber { get; set; }

    public string? Observation { get; set; }

    public bool? Active { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public string? Status { get; set; }

    [NotMapped]
    public string? StatusDescription { get; set; }
    
    [NotMapped]
    public List<PackingListItem>? Imputados { get; set; }

    [NotMapped]
    public List<PackingListStatusHistory>? StatusHistory { get; set; } = new List<PackingListStatusHistory>();

    [NotMapped]
    public List<ImputadoStatusHistory>? ImputadoHistory { get; set; }

}
