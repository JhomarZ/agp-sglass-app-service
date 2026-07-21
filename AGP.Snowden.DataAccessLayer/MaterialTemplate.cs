using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class MaterialTemplate
{
    public int Id { get; set; }

    public string? MaterialDescription { get; set; }

    public string? Tag { get; set; }

    public int? InspectionPlanId { get; set; }

    public int? MaterialTypeId { get; set; }

    public int? MaterialCategoryId { get; set; }

    public string? MaterialTypeGroup { get; set; }

    public string? MaterialTypeCategory { get; set; }

    public int? QtyDaysAlert { get; set; }

    public string? Center { get; set; }

    public bool? Active { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual InspectionPlan? InspectionPlan { get; set; }
}
