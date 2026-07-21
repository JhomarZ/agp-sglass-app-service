using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AGP.Snowden.DataAccessLayer.RRHH
{

    [Table("LoanMoneyRequestStatusHistory", Schema = "RRHH")]
    public  partial class LoanMoneyRequestStatusHistory
    {
        public int Id { get; set; }
        public int LoanMoneyRequest_Id { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Observation { get; set; }
    }
}
