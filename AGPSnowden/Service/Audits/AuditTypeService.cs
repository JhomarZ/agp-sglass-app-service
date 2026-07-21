using AGP.Snowden.DataAccessLayer;
using AGPSnowden.Model;

namespace AGPSnowden.Service.Audits
{
    public class AuditTypeService
    {
        public List<AuditType> GetAll(string Centro, string Description, int Start = 0, int Records = 10)
        {
            List<AuditType> list = new List<AuditType>();
            using (var context = new DbSnowdenContext())
            {
                //list = context.AuditTypes.ToList();
                var query = context.AuditTypes.OrderByDescending(x => x.Id)
                            .Where(x => x.Centro == Centro).AsQueryable();

                if (Description != "" && Description != null)
                {
                    query = query.Where(x => x.Name.Contains(Description));
                }

                list = query.Skip(Start).Take(Records).ToList();
            }

            return list;
        }

        public async Task<AuditType> GetOne(int Id)
        {
            AuditType auditType = new AuditType();

            try
            {
                using (var context = new DbSnowdenContext())
                {
                    auditType = await context.AuditTypes.FindAsync(Id);
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message, "");
            }

            return auditType;
        }

        public async Task<AuditType> Add(AuditType auditType)
        {
            try
            {
                using (var context = new DbSnowdenContext())
                {
                    await context.AuditTypes.AddAsync(auditType);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message, "");
            }

            return auditType;
        }

        public async Task<AuditType> Update(AuditType auditType)
        {

            try
            {
                using (var context = new DbSnowdenContext())
                {
                    context.AuditTypes.Update(auditType);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message, "");
            }

            return auditType;
        }

        public async Task<AuditType> Delete(int id)
        {
            AuditType? auditType;
            try
            {
                using (var context = new DbSnowdenContext())
                {
                    auditType = context.AuditTypes.Find(id);
                    if (auditType == null) { throw new ArgumentException("Tipo auditoria No existe "); }
                    auditType.Active = false;
                    context.AuditTypes.Update(auditType);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message, "");
            }

            return auditType;
        }
    }
}
