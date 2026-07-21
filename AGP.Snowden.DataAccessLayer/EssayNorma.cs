using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class EssayNorma
{
    public int Id { get; set; }

    public int? EssayId { get; set; }

    public int? NormaId { get; set; }
}
