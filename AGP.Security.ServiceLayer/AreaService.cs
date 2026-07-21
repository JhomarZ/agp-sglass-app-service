using AGP.Security.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGP.Security.ServiceLayer
{
    public  class AreaService
    {
        public Area GetArea(int Id)
        {
            Area area = new Area();
            using(var dbContext= new AgpSecurityContext())
            {
                area = dbContext.Areas.Find(Id);
            }
            return area;
        }
    }
}
