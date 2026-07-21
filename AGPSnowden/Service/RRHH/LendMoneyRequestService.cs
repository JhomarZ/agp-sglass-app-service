using AGP.Snowden.DataAccessLayer;
using AGP.Snowden.DataAccessLayer.RRHH;
using System.Data.Entity;

namespace AGPSnowden.Service.RRHH
{
    public class LendMoneyRequestService
    {
        public List<LoanMoneyRequest> GetAll(int Start = 0, int Records = 10, string Centro = "PE02", string DocumentNumber = "")
        {
            List<LoanMoneyRequest> list = new List<LoanMoneyRequest>();
            using (var context = new DbSnowdenContext())
            {
                //list = context.AuditSubType.ToList();
                var query = context.LendMoneyRequest.OrderByDescending(x => x.Id).AsQueryable();

                if (DocumentNumber != "")
                {
                    query = query.Where(x => x.DocumentNumber == DocumentNumber).AsQueryable();
                }

                list = query.Skip(Start).Take(Records).ToList();
            }

            return list;
        }

        public  List<LoanMoneyRequest> GetByDocumentNumber(string DocumentNumber)
        {
            List<LoanMoneyRequest> list = new List<LoanMoneyRequest>();

            try
            {
                using (var context = new DbSnowdenContext())
                {
                    list = context.LendMoneyRequest.Where(x=>x.DocumentNumber==DocumentNumber).OrderByDescending(x=>x.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return list;
        }

        public async Task<LoanMoneyRequest> GetOne(int Id)
        {
            LoanMoneyRequest record = new LoanMoneyRequest();

            try
            {
                using (var context = new DbSnowdenContext())
                {
                    record = await context.LendMoneyRequest.FindAsync(Id);
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return record;
        }

        public async Task<LoanMoneyRequest> Add(LoanMoneyRequest record)
        {
            try
            {
                using (var context = new DbSnowdenContext())
                {
                    await context.LendMoneyRequest.AddAsync(record);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return record;
        }

        public async Task<LoanMoneyRequest> Update(LoanMoneyRequest record)
        {

            try
            {
                using (var context = new DbSnowdenContext())
                {
                    context.LendMoneyRequest.Update(record);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return record;
        }

        public async Task<LoanMoneyRequest> Delete(int id)
        {
            LoanMoneyRequest record;
            try
            {
                using (var context = new DbSnowdenContext())
                {
                    record = context.LendMoneyRequest.Find(id);
                    record.Active = false;
                    context.LendMoneyRequest.Update(record);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return record;
        }

    }

}
