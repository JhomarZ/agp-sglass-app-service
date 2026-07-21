using System;
using System.Collections.Generic;

namespace AGP.Gordon.DataAccessLayer.SAPEXPANSION;

public partial class UsuarioCompanium
{
    public int Id { get; set; }

    public string? Usuario { get; set; }

    public string? IdCompania { get; set; }

    public string? CompaniaNombre { get; set; }

    public byte? CompaniaDefault { get; set; }

    public byte? CompaniaOrigen { get; set; }
}
