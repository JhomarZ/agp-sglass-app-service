using System;
using System.Collections.Generic;

namespace AGP.Security.DataAccessLayer;

public partial class Profile
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? SystemId { get; set; }

    public bool? Active { get; set; }
}
