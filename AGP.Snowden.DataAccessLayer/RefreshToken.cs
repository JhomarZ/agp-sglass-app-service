using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class RefreshToken
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? Token { get; set; }

    public DateTime? ExpiryDate { get; set; }
}
