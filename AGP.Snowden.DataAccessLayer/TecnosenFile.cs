using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class TecnosenFile
{
    public long Id { get; set; }

    public string? Program { get; set; }

    public string? Probes { get; set; }

    public string? Channels { get; set; }

    public string? ShapeFile { get; set; }

    public string? BatchName { get; set; }

    public string? BatchDate { get; set; }

    public int? Filas { get; set; }

    public byte? Active { get; set; }

    public string? AudUsuCrea { get; set; }

    public DateTime? AudFecCrea { get; set; }

    public string? AudUsuModi { get; set; }

    public DateTime? AudFecModi { get; set; }

    public int? ColumnsNumber { get; set; }

    public string? AudTerminal { get; set; }

    public string? AudIp { get; set; }

    public string? TecnosenName { get; set; }

    public int? CompaniaId { get; set; }
}
