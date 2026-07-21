using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class TestRequestFile
{
    public int Id { get; set; }

    public long? TestRequestId { get; set; }

    public int? TestParameterInspectionId { get; set; }

    public string? TestParameterName { get; set; }

    public string? File1 { get; set; }

    public string? File2 { get; set; }

    public string? File3 { get; set; }

    public string? File4 { get; set; }

    public string? File5 { get; set; }

    public string? File6 { get; set; }

    public string? File8 { get; set; }

    public string? File9 { get; set; }

    public string? File10 { get; set; }

    public string? TypeFile { get; set; }

    public byte? Active { get; set; }
}
