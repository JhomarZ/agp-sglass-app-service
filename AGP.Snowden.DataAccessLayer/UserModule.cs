using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class UserModule
{
    public int Id { get; set; }

    public int ModuleId { get; set; }

    public int UserId { get; set; }

    public int SystemId { get; set; }
}
