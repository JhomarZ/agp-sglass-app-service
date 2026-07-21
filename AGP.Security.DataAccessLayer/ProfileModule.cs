using System;
using System.Collections.Generic;

namespace AGP.Security.DataAccessLayer;

public partial class ProfileModule
{
    public int Id { get; set; }

    public int? ModuleId { get; set; }

    public int? ProfileId { get; set; }
}
