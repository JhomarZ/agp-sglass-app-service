using AGP.Snowden.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGP.Snowden.ServiceLayer.Warehouse
{
    public class ValidationPlanService
    {
        public async Task<ValidationPlan> GetById(int? Id)
        {
            ValidationPlan validationPlan = new ValidationPlan();
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    validationPlan = await db.ValidationPlans.FindAsync(Id);

                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return validationPlan;
        }

        public async Task<List<ValidationPlan>> GetAll(string Plant = "PE02")
        {
            List<ValidationPlan> list = new List<ValidationPlan>();
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    list = await db.ValidationPlans.Where(x => x.Center == Plant).ToListAsync();

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
