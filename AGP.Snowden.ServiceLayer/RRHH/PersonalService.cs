using AGP.Snowden.DataAccessLayer.SAP;
using AGP.Snowden.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;
using QRCoder;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using AGP.Snowden.DataAccessLayer;
using AGP.Security.DataAccessLayer;

namespace AGP.Snowden.ServiceLayer.RRHH
{
    public class PersonalService
    {
        public async Task<List<Personal>> GetAll(int Skip = 0, int Take = 15, string? Description = "")
        {
            List<Personal> lista = new List<Personal>();
            using (var db = new DbSnowdenContext())
            {
                var Query = db.Personal.Where(x => (x.FullName + x.Email).Contains(Description)).AsQueryable();

                lista = await Query.OrderByDescending(x => x.Id).Skip(Skip).Take(Take).ToListAsync();

            }

            return lista;
        }
        public async Task<Personal> GetOneByEmail(string Email = "")
        {
            Personal personal = new Personal();

            try
            {
                using (var db = new DbSnowdenContext())
                {
                    personal = await db.Personal.Where(x => x.Email.Contains(Email)).FirstOrDefaultAsync();

                }
            } 
            catch(Exception ex)
            {
                throw new System.ArgumentException("Error obteniendo personal", ex.Message);
            }

            return personal;
        }

        public async Task<Personal> GetOneByDocumentNumber(string DocumentNumber = "")
        {
            Personal personal = new Personal();

            try
            {
                using (var db = new DbSnowdenContext())
                {
                    personal = await db.Personal.Where(x => x.NumberDocument.Contains(DocumentNumber)).FirstOrDefaultAsync();

                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException("Error obteniendo personal", ex.Message);
            }

            return personal;
        }

        public async Task<Personal> GetOneByDocument(string DocumentNumber = "")
        {
            Personal personal = new Personal();

            try
            {
                using (var db = new DbSnowdenContext())
                {
                    personal = await db.Personal.Where(x => x.NumberDocument.Contains(DocumentNumber)).FirstOrDefaultAsync();

                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException("Error obteniendo personal", ex.Message);
            }

            return personal;
        }
    }
}
