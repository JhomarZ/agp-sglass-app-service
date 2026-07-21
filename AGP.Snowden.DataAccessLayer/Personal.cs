using AGP.Security.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace AGP.Snowden.DataAccessLayer;

[Table("Personal", Schema = "dbo")]
public partial class Personal
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string? JobTitle { get; set; }

    public string? Department { get; set; }

    public string Email { get; set; }

    public string? Manager { get; set; }

    public string? ManagerEmail { get; set; }

    public string? NumberDocument { get; set; }

    public string? PersonalType { get; set; }

    public string? Name { get; set; }
    public string? LastName1 { get; set; }
    public string? LastName2 { get; set; }
    public string? DocumentType { get; set; }
    public string? PersonalEmail { get; set; }
    public bool Active { get; set; }

}
