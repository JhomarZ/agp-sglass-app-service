using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class CharacteristicInspectionPlan
{
    public string? Name { get; set; }

    public string? Center { get; set; }

    public bool? Active { get; set; }

    public int Id { get; set; }

    public virtual ICollection<CharacteristicInput> CharacteristicInputs { get; } = new List<CharacteristicInput>();
}
