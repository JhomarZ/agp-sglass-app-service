using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AGP.Snowden.DataAccessLayer;

[Table("PackingListStatusHistories", Schema = "Warehouse")] 
public partial class PackingListStatusHistory
{
    public long Id { get; set; }

    public int? PackageId { get; set; }

    public string? Status { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

  
    
}
