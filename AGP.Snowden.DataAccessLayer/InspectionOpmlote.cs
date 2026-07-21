using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class InspectionOpmlote
{
    public int Id { get; set; }

    public int? InspectionOpmaterialId { get; set; }

    public string? OrderPurchase { get; set; }

    public int? MaterialKey { get; set; }

    public string? Lote { get; set; }

    public string? LevelInspection { get; set; }

    public string? DateExpiration { get; set; }

    public int? QuantityToInspect { get; set; }

    public string? Center { get; set; }

    public bool? Active { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public string? FabricationDate { get; set; }

    public string? StatusInspection { get; set; }

    public string? StoreLocation { get; set; }

    public string? HasTraslateToScrap { get; set; }

    public string? CodeScrapTraslateSap { get; set; }

    public string? HasTraslateToAproved { get; set; }

    public string? CodeConcessionTraslateSap { get; set; }

    public string? FileNameConcession { get; set; }

    public bool? SentToSap { get; set; }
}
