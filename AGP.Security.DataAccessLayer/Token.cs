using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace AGP.Security.DataAccessLayer;

public partial class Token
{
    public long Id { get; set; }

    public int UserId { get; set; }

    public string? AccessToken { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? AccessTokenExpirationDate { get; set; }

    public DateTime? RefreshTokenExpirationDate { get; set; }

    public virtual User User { get; set; } = null!;

    [NotMapped]
    public dynamic personal { get; set; } = null!;
}
