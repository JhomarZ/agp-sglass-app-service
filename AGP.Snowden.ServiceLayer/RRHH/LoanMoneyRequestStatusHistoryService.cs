using AGP.Snowden.DataAccessLayer.RRHH;
using AGP.Snowden.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace AGP.Snowden.ServiceLayer.RRHH
{
    public class LoanMoneyRequestStatusHistoryService
    {
        public async Task<List<LoanMoneyRequestStatusHistory>> GetByLoanRequestId(int Id)
        {
            List<LoanMoneyRequestStatusHistory> list = new List<LoanMoneyRequestStatusHistory>();
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    list = await db.LoanMoneyRequestStatusHistory.Where(x => x.LoanMoneyRequest_Id == Id).ToListAsync();
                    return list;
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException("Error obteniendo personal", ex.Message);
            }
        }

        public async Task<LoanMoneyRequestStatusHistory> Add(LoanMoneyRequestStatusHistory Status)
        {
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    await db.LoanMoneyRequestStatusHistory.AddAsync(Status);
                    db.SaveChanges();
                    return Status;
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException("Error guardando archivo", ex.Message);
            }

        }

        public async Task<bool> Delete(LoanMoneyRequestStatusHistory Staatus)
        {
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    db.LoanMoneyRequestStatusHistory.Remove(Staatus);
                    await db.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException("Error obteniendo personal", ex.Message);
            }

        }

    }
}

