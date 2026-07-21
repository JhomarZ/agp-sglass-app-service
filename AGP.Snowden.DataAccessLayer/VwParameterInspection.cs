using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class VwParameterInspection
{
    public long? IndexWm { get; set; }

    public int Id { get; set; }

    public string? Centro { get; set; }

    public string? Name { get; set; }

    public string? Watermark { get; set; }

    public string? EnglishName { get; set; }

    public int? QuantityValues { get; set; }

    public string? Value { get; set; }
}
