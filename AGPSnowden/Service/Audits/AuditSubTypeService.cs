using AGP.Snowden.DataAccessLayer;
using AGPSnowden.Model;

namespace AGPSnowden.Service
{
    public class AuditSubTypeService
    {
        public List<AuditSubType> GetAll(string Centro, string? Description, int? auditTypeId, int Start = 0, int Records = 10)
        {
            List<AuditSubType> list = new List<AuditSubType>();
            using (var context = new DbSnowdenContext())
            {
                //list = context.AuditSubType.ToList();
                var query = context.AuditSubTypes.OrderByDescending(x => x.Id)
                            .Where(x => x.Centro == Centro).AsQueryable();

                if (auditTypeId != null)
                {
                    query = query.Where(x => x.AuditTypeId==auditTypeId);
                }

                if (Description != "" && Description != null)
                {
                    query = query.Where(x => x.Name.Contains(Description));
                }

                list = query.Skip(Start).Take(Records).ToList();
            }

            return list;
        }

        public async Task<AuditSubType> GetOne(int Id)
        {
            AuditSubType auditSubType = new AuditSubType();

            try
            {
                using (var context = new DbSnowdenContext())
                {
                    auditSubType = await context.AuditSubTypes.FindAsync(Id);
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return auditSubType;
        }

        public async Task<AuditSubType> Add(AuditSubType auditSubType)
        {
            try
            {
                using (var context = new DbSnowdenContext())
                {
                    await context.AuditSubTypes.AddAsync(auditSubType);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return auditSubType;
        }

        public async Task<AuditSubType> Update(AuditSubType auditSubType)
        {

            try
            {
                using (var context = new DbSnowdenContext())
                {
                    context.AuditSubTypes.Update(auditSubType);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return auditSubType;
        }

        public async Task<AuditSubType> Delete(int id)
        {
            AuditSubType auditSubType;
            try
            {
                using (var context = new DbSnowdenContext())
                {
                    auditSubType = context.AuditSubTypes.Find(id);
                    auditSubType.Active = false;
                    context.AuditSubTypes.Update(auditSubType);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return auditSubType;
        }

    }
}
