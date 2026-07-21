using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class TypeEssay
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Centro { get; set; }

    public string? NameEnglish { get; set; }

    public byte? Active { get; set; }
}
