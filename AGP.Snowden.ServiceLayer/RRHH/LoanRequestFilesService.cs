using AGP.Snowden.DataAccessLayer;
using AGP.Snowden.DataAccessLayer.RRHH;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGP.Snowden.ServiceLayer.RRHH
{
    public class LoanRequestFilesService
    {
        public async Task<List<LoanRequestFile>> GetAll(int Skip = 0, int Take = 15, string? Description = "")
        {
            List<LoanRequestFile> lista = new List<LoanRequestFile>();
            using (var db = new DbSnowdenContext())
            {
                var Query = db.LoanRequestFiles.Where(x => x.FileName.Contains(Description)).AsQueryable();

                lista = await Query.OrderByDescending(x => x.Id).Skip(Skip).Take(Take).ToListAsync();

            }

            return lista;
        }
        public async Task<List<LoanRequestFile>> GetByLoanRequestId(int Id)
        {
            List<LoanRequestFile> personal = new List<LoanRequestFile>();
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    personal = await db.LoanRequestFiles.Where(x => x.LoanRequestId == Id).ToListAsync();

                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException("Error obteniendo personal", ex.Message);
            }

            return personal;
        }

        public async Task<LoanRequestFile> Add(LoanRequestFile RequestFile)
        {
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    await db.LoanRequestFiles.AddAsync(RequestFile);
                    db.SaveChanges();
                    return RequestFile;
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException("Error guardando archivo", ex.Message);
            }

        }

        public async Task<bool> Delete(LoanRequestFile FileId)
        {
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    db.LoanRequestFiles.Remove(FileId);
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
