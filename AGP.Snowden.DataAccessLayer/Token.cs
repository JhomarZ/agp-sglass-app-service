using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class Token
{
    public int Id { get; set; }

    public string? Email { get; set; }

    public string? Username { get; set; }

    public int UserId { get; set; }

    public string AccessToken { get; set; } = null!;

    public string RefreshToken { get; set; } = null!;

    public DateTime AccessTokenExpirationDate { get; set; }

    public DateTime RefreshTokenExpirationDate { get; set; }
}
