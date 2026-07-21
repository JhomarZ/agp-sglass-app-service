using System;
using System.Collections.Generic;

namespace AGP.Snowden.DataAccessLayer;

public partial class TestRequestStatusHistory
{
    public int Id { get; set; }

    public long? TestRequestId { get; set; }

    public string? StatusCode { get; set; }

    public DateTime? DateStatus { get; set; }
}
