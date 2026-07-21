using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class ProgramaCabecera
{
    public int Id { get; set; }

    public int? ProgramaId { get; set; }

    public int? OrdenVal { get; set; }

    public string? CabeceraNombre { get; set; }

    public string? Caracteristica { get; set; }

    public string? Nominal { get; set; }

    public string? Minimo { get; set; }

    public string? Maximo { get; set; }

    public byte? Activo { get; set; }
}
