using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AGP.Snowden.DataAccessLayer;

public partial class Material
{
    [Key]
    public int? MaterialKey { get; set; }

    public string? MaterialDescription { get; set; }

    public string? Centro { get; set; }

    public int? FechaIni { get; set; }

    public int? FechaMod { get; set; }

    public string? Matgr { get; set; }

    public string? PriceUnit { get; set; }

    public string? BaseUom { get; set; }

    public string? Currency { get; set; }

    public string? Price { get; set; }

    public string? MaterialType { get; set; }

    public string? MaterialGroup { get; set; }

    public string? MrpType { get; set; }

    public string? UpdateFlag { get; set; }

    public string? MatgrpText { get; set; }
}
