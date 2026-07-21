using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AGP.Snowden.DataAccessLayer;

public partial class SpectroRequest
{
    public int Id { get; set; }

    public string? RequestCode { get; set; }

    public int? NroProbetas { get; set; }

    public string? MeasurementCode { get; set; }

    public int? MeasurementId { get; set; }

    public string? Side { get; set; }

    public int? TechnologyId { get; set; }

    public DateTime? ReservationDate { get; set; }

    public int? RangeMeasurementId { get; set; }

    public string? MeasurementStatus { get; set; }

    public int? MeasurementTypeId { get; set; }

    public string? RequestStatus { get; set; }

    public string? PriorityCode { get; set; }

    public int? SamplesQuantity { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? Active { get; set; }

    [NotMapped]
    public virtual Technology? Technology { get; set; }

    [NotMapped]
    public virtual MeasurementType? MeasurementType { get; set; }


    [NotMapped]
    public string SideDescription { get; set; }


    public string GetSide()
    {
        string side = "";
        switch (this.Side)
        {
            case "CE":
                side = "CARA EXTERNA"; break;
            case "CI":
                side = "CARA INTERNA"; break;
        }
        return side;
    }


}
