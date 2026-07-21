using AGP.Gordon.CommonLayer;
using AGP.Snowden.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGP.Snowden.ServiceLayer.Warehouse
{
    public class TrackingImputadosExtensionService
    {

        public async Task<ResponseDataGrid> GetAll(int Page, int Rows, string Centro, string Description)
        {
            ResponseDataGrid responseDataGrid = new ResponseDataGrid();
            List<TrackingImputadosExtension> list = new List<TrackingImputadosExtension>();
            using (var db = new DbSnowdenContext())
            {
                var Query = db.TrackingImputadosExtensions.Where(x => x.CentroSap == Centro).AsQueryable();


                if (Description != null && Description != "")
                {
                    Query = Query.Where(d => d.Id.ToString().ToUpper().Contains(Description.ToUpper()));
                }

                int rowsTotal = 0;
                rowsTotal = Query.Count();
                int filaInicial = Convert.ToInt32((Page - 1) * Rows);
                int totalPaginas = (rowsTotal + Rows - 1) / Rows;

                list = await Query.OrderByDescending(x => x.Id).Skip(filaInicial).Take(Rows).ToListAsync();
                responseDataGrid.rows = list;
                responseDataGrid.totalRows = rowsTotal;
                responseDataGrid.totalPages = totalPaginas;
                responseDataGrid.startRecord = filaInicial;
                responseDataGrid.finishRecord = filaInicial + Rows;
            }

            return responseDataGrid;
        }

        public async Task<TrackingImputadosExtension> GetById(int id)
        {
            TrackingImputadosExtension imputado = new TrackingImputadosExtension();
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    imputado = await db.TrackingImputadosExtensions.FindAsync(id);

                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return imputado;
        }
        public async Task<TrackingImputadosExtension> Add(TrackingImputadosExtension imputado)
        {

            using (var db = new DbSnowdenContext())
            {
                db.TrackingImputadosExtensions.Add(imputado);
                db.SaveChanges();
            }
            return imputado;
        }

        public async Task<TrackingImputadosExtension> Update(int id, TrackingImputadosExtension imputado)
        {
            TrackingImputadosExtension CurrentImputado = new TrackingImputadosExtension();
            using (var db = new DbSnowdenContext())
            {
                CurrentImputado = db.TrackingImputadosExtensions.Find(id);
                CurrentImputado.Bultos = imputado.Bultos;
                CurrentImputado.Status = imputado.Status;
                CurrentImputado.UpdatedAt = DateTime.Now;
         
                db.SaveChanges();
            }
            return CurrentImputado;
        }

        public async void Delete(int id)
        {
            TrackingImputadosExtension Imputado = new TrackingImputadosExtension();
            using (var db = new DbSnowdenContext())
            {
                Imputado = db.TrackingImputadosExtensions.Find(id);
                //Imputado.Active = false;
                db.SaveChanges();
            }
        }

        public async Task<ImputadoStatusHistory> AddImputadoHistoryStatus(ImputadoStatusHistory imputadoHistory)
        {
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    db.ImputadoStatusHistories.Add(imputadoHistory);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }
            return imputadoHistory;
        }

    }
}
