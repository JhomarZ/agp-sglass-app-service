using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class VprofileModule
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? ParentId { get; set; }

    public string? Parent { get; set; }

    public int? ProfileId { get; set; }

    public int SystemId { get; set; }
}
