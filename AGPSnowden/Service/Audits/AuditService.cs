using AGP.Snowden.DataAccessLayer;
using AGPSnowden.Model;
using AGPSnowden.Model.Scada;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AGPSnowden.Service.Audits
{
    public class AuditService
    {
        public async Task<List<Audit>> List(int start=0,int records=20 ,string centro="", int? auditTypeId=null,int? auditSubTypeId = null, int? productId = null,
            string shift = "", string description="")
        {
            List<Audit> audits = new List<Audit>(); 
            try
            {
                using (var context = new DbSnowdenContext())
                {
                    var query = context.Audits.OrderByDescending(x => x.Id)
                             .Where(x => x.Centro == centro).AsQueryable();
                    if (auditTypeId != null)
                    {
                        query =  query.Where(x => x.TypeId==auditTypeId );
                    }
                    if (auditSubTypeId != null)
                    {
                        query = query.Where(x => x.SubTypeId == auditSubTypeId);
                    }
                    if (shift != null && shift != "")
                    {
                        query = query.Where(x => x.Shift == shift);
                    }


                    audits =  query.Skip(start).Take(records).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return audits;
        }

        public async Task<Audit> GetOne(int id)
        {
            Audit audit = new Audit();
            try
            {
                using (var context = new DbSnowdenContext())
                {

                    audit= await context.Audits.FindAsync(id);
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return audit;
        }

        public async Task<Audit> Add(Audit audit)
        {
            try
            {
                using (var context = new DbSnowdenContext())
                {
                    audit.Active = true;
                    audit.CreatedAt= DateTime.Now;
                    await context.Audits.AddAsync(audit);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return audit;
        }

        public async Task<Audit> Edit(int id,Audit audit)
        {
            try
            {
                using (var context = new DbSnowdenContext())
                {
                    Audit currentAudit = new Audit();
                    currentAudit = await context.Audits.FindAsync(id);
                    if(currentAudit != null) throw new System.ArgumentException("Audit not exist", "");
                    currentAudit.TypeId = audit.TypeId;
                    currentAudit.SubTypeId = audit.SubTypeId;
                    currentAudit.ProductId = audit.ProductId;
                    currentAudit.UpdatedAt = DateTime.Now;
                    currentAudit.Status = audit.Status;
                    currentAudit.Observation = audit.Observation;
                    currentAudit.Shift = audit.Shift;
                    currentAudit.Validation = audit.Validation;
                    currentAudit.GeneralComment = audit.GeneralComment;
                    currentAudit.Zona = audit.Zona;
                    currentAudit.ProductionOrder = audit.ProductionOrder;
                    currentAudit.ValidationQuality = audit.ValidationQuality;
                    currentAudit.ValidationText = audit.ValidationText;
                    currentAudit.ValidationQualityText = audit.ValidationQualityText;
                    currentAudit.HasNc = audit.HasNc;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return audit;
        }

        public async Task<Audit> Delete(int Id)
        {
            Audit currentAudit = new Audit();
            try
            {
                using (var context = new DbSnowdenContext())
                {
                    currentAudit = await context.Audits.FindAsync(Id);
                    if (currentAudit != null) throw new System.ArgumentException("Audit not exist", "");
                    currentAudit.Active = false;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }
            return currentAudit;
        }

    }
}
