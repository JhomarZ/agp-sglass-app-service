using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class VwTestRequestMeasurementsUnpivot
{
    public long TestRequestId { get; set; }

    public string? KeyWordTr { get; set; }

    public string? TagId { get; set; }

    public string? Test { get; set; }

    public string? Name { get; set; }

    public decimal? TotalDuration { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? Status { get; set; }

    public string? Technician { get; set; }

    public string? Pmrequester { get; set; }

    public string? StackCode { get; set; }

    public string? StackInfo { get; set; }

    public string? Keyword { get; set; }

    public int? HoraNumero { get; set; }

    public int? ParameterId { get; set; }

    public string? ParameterName { get; set; }

    public string? ClassifierNameL1 { get; set; }

    public string? ClassifierNameL2 { get; set; }

    public string? ClassifierNameL3 { get; set; }

    public string? Field { get; set; }

    public string? Val { get; set; }

    public string? IndexField { get; set; }
}
