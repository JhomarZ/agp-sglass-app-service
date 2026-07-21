using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGP.Gordon.CommonLayer
{
    public class HelpResponse
    {
     
    }
    public class ResponseDataGrid
    {
        public int startRecord { get; set; }
        public int finishRecord { get; set; }
        public int totalPages { get; set; }
        public int totalRows { get; set; }
        public dynamic rows { get; set; }
    }
}
