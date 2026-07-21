using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AGP.Snowden.DataAccessLayer;

[Table("shippingStatus", Schema = "Warehouse")]
public partial class ShippingStatus
{
    public int Id { get; set; }

    public string? Code { get; set; }
    
    public string? Name { get; set; }

    public string? Description { get; set; }


    public virtual ICollection<PackingListItem> PackingListItems { get; } = new List<PackingListItem>();

    public virtual ICollection<PackingListStatus> PackingListStatuses { get; } = new List<PackingListStatus>();

    public virtual ICollection<PackingList> PackingLists { get; } = new List<PackingList>();
}
