using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class TestParameterInspectionClassifier
{
    public int Id { get; set; }

    public int? ParameterId { get; set; }

    public string? ParameterName { get; set; }

    public int? ClassifierL1 { get; set; }

    public string? ClassifierNameL1 { get; set; }

    public int? ClassifierL2 { get; set; }

    public string? ClassifierNameL2 { get; set; }

    public int? ClassifierL3 { get; set; }

    public string? ClassifierNameL3 { get; set; }

    public string? Centro { get; set; }

    public bool? Active { get; set; }
}
