using System;
using System.Collections.Generic;

namespace AGP.Gordon.DataAccessLayer.SAPEXPANSION;

public partial class UsuariosAgp
{
    public int Id { get; set; }

    public string? Usuario { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public int? IdCompania { get; set; }

    public string? Area { get; set; }

    public byte? Activo { get; set; }
}
