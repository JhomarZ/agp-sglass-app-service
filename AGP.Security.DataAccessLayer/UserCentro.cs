using System;
using System.Collections.Generic;

namespace AGP.Security.DataAccessLayer;

public partial class UserCentro
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Centro { get; set; } = null!;
}
