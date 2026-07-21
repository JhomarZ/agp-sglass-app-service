using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class TestRequestParameterInspectionTmp
{
    public int Id { get; set; }

    public long TestRequestId { get; set; }

    public int TestParameterInspectionId { get; set; }

    public int? QuantityColumns { get; set; }

    public int? ParameterClassifierId { get; set; }
}
