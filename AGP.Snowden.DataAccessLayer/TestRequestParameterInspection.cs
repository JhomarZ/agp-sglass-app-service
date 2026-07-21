using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class TestRequestParameterInspection
{
    public int Id { get; set; }

    public long TestRequestId { get; set; }

    public int? TestParameterInspectionId { get; set; }

    public int? QuantityColumns { get; set; }

    public int? ParameterClassifierId { get; set; }

    public string? ParameterName { get; set; }

    public string? ClassifierNameL1 { get; set; }

    public string? ClassifierNameL2 { get; set; }

    public string? ClassifierNameL3 { get; set; }

    public int? ClassifierL1 { get; set; }

    public int? ClassifierL2 { get; set; }

    public int? ClassifierL3 { get; set; }
}
