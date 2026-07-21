using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class PressTissueControl
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public string? WhenDate { get; set; }

    public string? Shift { get; set; }

    public string? PressType { get; set; }

    public string? Tissue { get; set; }

    public string? Cause { get; set; }

    public int? PartsPressed { get; set; }

    public string? Photo { get; set; }

    public string? Position { get; set; }

    public string? Observation { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public bool? Active { get; set; }

    public int? LastQuantity { get; set; }

    public int? NextQuantityAlert { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public string? Center { get; set; }
}
