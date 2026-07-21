using AGP.Snowden.DataAccessLayer.SAP;
using AGP.Snowden.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AGP.Snowden.ServiceLayer.Warehouse
{
    public class ShippingStatusService
    {
        public async Task<List<ShippingStatus>> GetAll(string? Centro= "")
        {
            List<ShippingStatus> lista = new List<ShippingStatus>();
            using (var db = new DbSnowdenContext())
            {
                lista = await db.ShippingStatuses.ToListAsync();

            }
            return lista;
        }

        public async Task<ShippingStatus> GetOne(int? Id)
        {
            ShippingStatus Status = new ShippingStatus();
            using (var db = new DbSnowdenContext())
            {
                Status = await db.ShippingStatuses.Where(x=>x.Id==Id).FirstOrDefaultAsync();

            }
            return Status;
        }
    }
}
