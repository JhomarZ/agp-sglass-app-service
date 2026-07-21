using System;
using System.Collections.Generic;

namespace AGPSnowden.Model.Scada;

public partial class TableScadaSap
{
    public long Id { get; set; }

    public string? Order { get; set; }

    public string? Correlative { get; set; }

    public DateTime? Date { get; set; }

    public string? IpNumber { get; set; }

    public int? Quantity { get; set; }

    public string? Status { get; set; }

    public string? Cicle { get; set; }

    public string? IdworkCenter { get; set; }

    public string? Operation { get; set; }

    public string? Workcenter { get; set; }

    public string? KeyModel { get; set; }

    public string? Type { get; set; }

    public DateTime? DateModify { get; set; }

    public int? DefectId { get; set; }

    public string? Plant { get; set; }
}
