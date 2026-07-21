using AGP.Snowden.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGP.Snowden.ServiceLayer.Warehouse
{
    public class InspectionPlanService
    {
        public async Task<InspectionPlan> GetById(int? Id)
        {
            InspectionPlan inspectionPlan = new InspectionPlan();
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    inspectionPlan = await db.InspectionPlans.FindAsync(Id);

                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return inspectionPlan;
        }

        public async Task<List<InspectionPlan>> GetAll(string Plant = "PE02")
        {
            List<InspectionPlan> list = new List<InspectionPlan>();
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    list = await db.InspectionPlans.Where(x => x.Center == Plant).ToListAsync();

                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return list;
        }
    }
}
