using System;
using System.Collections.Generic;

namespace AGP.Security.DataAccessLayer;

public partial class Area
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Company { get; set; }

    public string? NameEng { get; set; }

    public string? Code { get; set; }

    public string? Center { get; set; }

    public virtual ICollection<User> Users { get; } = new List<User>();
}
