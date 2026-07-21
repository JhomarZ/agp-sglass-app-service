using AGP.Snowden.DataAccessLayer;
using AGPSnowden.Model;

namespace AGPSnowden.Service.Audits
{
    public class CheckListSegmentService
    {
        public List<CheckListSegment> GetAll(string centro, int? typeId, int? subTypeId,int? productId=null)
        {
            List<CheckListSegment> list = new List<CheckListSegment>();
            using (var context = new DbSnowdenContext())
            {
                //list = context.AuditTypes.ToList();
                var query = context.CheckListSegments.OrderByDescending(x => x.Id)
                            .Where(x => x.Centro == centro && x.AuditTypeId==typeId && x.AuditSubTypeId== subTypeId).AsQueryable();

                if (productId != null)
                {
                    query = query.Where(x => x.ProductId==productId);
                }

                list = query.ToList();
            }

            return list;
        }

    }
}
