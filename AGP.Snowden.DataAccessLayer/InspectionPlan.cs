using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class InspectionPlan
{
    public string? Name { get; set; }

    public string? Center { get; set; }

    public bool? Active { get; set; }

    public int Id { get; set; }

    public virtual ICollection<CharacteristicInput> CharacteristicInputs { get; } = new List<CharacteristicInput>();

    public virtual ICollection<MaterialTemplate> MaterialTemplates { get; } = new List<MaterialTemplate>();
}
