using Azure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGP.Snowden.DataAccessLayer.RRHH
{
    [Table("ReasonLendRequest", Schema = "RRHH")]
    public class ReasonLendRequest
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string? Tag { get; set; }
        public bool Active { get; set; }
        
    }
}
