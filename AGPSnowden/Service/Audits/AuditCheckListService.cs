using AGP.Snowden.DataAccessLayer;
using AGPSnowden.Model;
using Microsoft.EntityFrameworkCore;

namespace AGPSnowden.Service.Audits
{
    public class AuditCheckListService
    {
        public async Task<List<AuditChecksList>> List(int auditId)
        {
            List<AuditChecksList> checklist = new List<AuditChecksList>();
            try
            {
                using (var context = new DbSnowdenContext())
                {
                    checklist = context.AuditChecksLists.Where(x => x.AuditId == auditId).OrderByDescending(x => x.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return checklist;
        }

        public async Task<AuditChecksList> Add(AuditChecksList auditCheckItem)
        {
            try
            {
                using (var context = new DbSnowdenContext())
                {
                    auditCheckItem.Active = true;
                    auditCheckItem.CreatedAt = DateTime.Now;
                    await context.AuditChecksLists.AddAsync(auditCheckItem);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return auditCheckItem;
        }

        public async Task<AuditChecksList> Edit(int Id, AuditChecksList auditCheckItem)
        {
            try
            {
                using (var context = new DbSnowdenContext())
                {
                    AuditChecksList currentCheckItem = new AuditChecksList();
                    currentCheckItem = await context.AuditChecksLists.FindAsync(Id);
                    if (currentCheckItem != null) throw new System.ArgumentException("Audit not exist", "");
                    
                    currentCheckItem.CheckId =auditCheckItem.CheckId;
                    currentCheckItem.Value = auditCheckItem.Value;  
                    currentCheckItem.ImageA =auditCheckItem.ImageA;
                    currentCheckItem.ImageB = auditCheckItem.ImageB;  
                    currentCheckItem.Observation =auditCheckItem.Observation;   
                    currentCheckItem.CheckName =auditCheckItem.CheckName;
                    currentCheckItem.UpdatedAt = DateTime.Now;
                    currentCheckItem.Tag =auditCheckItem.Tag;
                    currentCheckItem.InputType =auditCheckItem.InputType;
                    currentCheckItem.Min =auditCheckItem.Min;
                    currentCheckItem.Max =auditCheckItem.Max;
                    currentCheckItem.Options =auditCheckItem.Options;
                    currentCheckItem.ObservationSupervisor=auditCheckItem.ObservationSupervisor;
                    currentCheckItem.ObservationQuality =auditCheckItem.ObservationQuality;
                    currentCheckItem.Responsable =  auditCheckItem.Responsable;
                    currentCheckItem.Functional =auditCheckItem.Functional;
                    currentCheckItem.Safety =auditCheckItem.Safety;
                    currentCheckItem.Attachment =auditCheckItem.Attachment;

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return auditCheckItem;
        }

        public async void Delete(int Id)
        {
            try
            {
                using (var context = new DbSnowdenContext())
                {
                    AuditChecksList currentAudit = new AuditChecksList();
                    currentAudit = await context.AuditChecksLists.FindAsync(Id);
                    if (currentAudit != null) throw new System.ArgumentException("Check item audit not exist", "");
                    currentAudit.Active = false;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

        }
    }
}
