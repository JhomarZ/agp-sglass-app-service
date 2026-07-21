using System;
using System.Collections.Generic;

namespace AGP.Security.DataAccessLayer;

public partial class User
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? LastName { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public int AreaId { get; set; }

    public int? ProfileId { get; set; }

    public bool? OfficeAcount { get; set; }

    public string? Email { get; set; }

    public string? Compania { get; set; }

    public string? Centro { get; set; }

    public DateTime? LastAcces { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? Active { get; set; }

    public string? Un { get; set; }

    public int? RoleId { get; set; }

    public int? IdHubDepartment { get; set; }

    public int? IdHubProcess { get; set; }

    public virtual Area Area { get; set; } = null!;

    public virtual Token? Token { get; set; }
}
