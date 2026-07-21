using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class PackingListStatus
{
    public int Id { get; set; }

    public int? PackingListId { get; set; }

    public int? ShippingStatusId { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ShippingStatus? ShippingStatus { get; set; }
}
