using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class VbiproductControlTissue
{
    public int Id { get; set; }

    public string? Centro { get; set; }

    public string? Name { get; set; }

    public bool? Active { get; set; }

    public int CurrentTotalQuantity { get; set; }

    public int CurrentQuantity { get; set; }

    public int NextQuantityAlertTissue { get; set; }
}
