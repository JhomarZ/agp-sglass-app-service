using AGP.Snowden.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGP.Snowden.ServiceLayer.Warehouse
{
    public class CharacteristicPlanService
    {
        public async Task<CharacteristicInspectionPlan> GetById(int? CharacteristicInputId)
        {
            CharacteristicInspectionPlan characteristic = new CharacteristicInspectionPlan();
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    characteristic = await db.CharacteristicInspectionPlans.FindAsync(CharacteristicInputId);

                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return characteristic;
        }

        public async Task<List<CharacteristicInspectionPlan>> GetAll(string Plant="PE02")
        {
            List<CharacteristicInspectionPlan> list = new List<CharacteristicInspectionPlan>();
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    list = await db.CharacteristicInspectionPlans.Where(x=>x.Center==Plant).ToListAsync();

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
