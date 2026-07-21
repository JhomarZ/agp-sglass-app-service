using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class TestParameterInspection
{
    public int Id { get; set; }

    public string? Centro { get; set; }

    public string? Name { get; set; }

    public string? EnglishName { get; set; }

    public int? QuantityValues { get; set; }

    public string? TypeInput { get; set; }

    public decimal? Min { get; set; }

    public decimal? Max { get; set; }

    public string? OptionSegment { get; set; }

    public byte? Active { get; set; }

    public string? Watermark { get; set; }

    public byte? HasLink { get; set; }
}
