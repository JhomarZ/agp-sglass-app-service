using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class Profile
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public bool? Active { get; set; }

    public int SystemId { get; set; }
}
