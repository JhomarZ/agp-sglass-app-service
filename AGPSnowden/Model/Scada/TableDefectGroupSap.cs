using System;
using System.Collections.Generic;

namespace AGPSnowden.Model.Scada;

public partial class TableDefectGroupSap
{
    public int IdCodigo { get; set; }

    public string? WorkStationPlc { get; set; }

    public string? DefectId { get; set; }

    public string? GroupDefect { get; set; }

    public string? GroupDefectDescription { get; set; }

    public string? Defect { get; set; }

    public string? DefectDescription { get; set; }
}
