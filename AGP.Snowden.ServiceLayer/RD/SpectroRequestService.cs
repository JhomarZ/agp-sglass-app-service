using AGP.Gordon.CommonLayer;
using AGP.Snowden.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGP.Snowden.ServiceLayer.RD
{
    public class SpectroRequestService
    {
        
        public async Task<ResponseDataGrid> GetAll(int Page, int Rows, string Centro, string Description)
        {
            ResponseDataGrid responseDataGrid = new ResponseDataGrid();
            List<SpectroRequest> list = new List<SpectroRequest>();
            using (var db = new DbSnowdenContext())
            {
                var Query = db.SpectroRequests.Where(x => x.Active == true).AsQueryable();


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

        public async Task<SpectroRequest> GetById(int SpectroRequestId)
        {
            SpectroRequest spectroRequest = new SpectroRequest();
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    spectroRequest = await db.SpectroRequests.FindAsync(SpectroRequestId);

                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return spectroRequest;
        }
        public async Task<SpectroRequest> Add(SpectroRequest SpectroRequest)
        {

            using (var db = new DbSnowdenContext())
            {
                SpectroRequest.Active = true;
                SpectroRequest.CreatedAt = DateTime.Now;
                SpectroRequest.UpdatedAt = DateTime.Now;
                db.SpectroRequests.Add(SpectroRequest);
                db.SaveChanges();
            }
            return SpectroRequest;
        }

        public async Task<SpectroRequest> Update(int SpectroRequestId, SpectroRequest SpectroRequest)
        {
            //SpectroRequest SpectroRequest = new SpectroRequest();
            using (var db = new DbSnowdenContext())
            {
                 db.SpectroRequests.Update(SpectroRequest);
                /*.Find(MaterialId);
                CurrentMaterialTemplate.InspectionPlanId = MaterialTemplate.InspectionPlanId;
                CurrentMaterialTemplate.MaterialDescription = MaterialTemplate.MaterialDescription;
                CurrentMaterialTemplate.MaterialTypeGroup = MaterialTemplate.MaterialTypeGroup;
                CurrentMaterialTemplate.CreatedBy = MaterialTemplate.CreatedBy;
                CurrentMaterialTemplate.MaterialTypeId = MaterialTemplate.MaterialTypeId;
                CurrentMaterialTemplate.MaterialCategoryId = MaterialTemplate.MaterialCategoryId;
                CurrentMaterialTemplate.MaterialTypeCategory = MaterialTemplate.MaterialTypeCategory;
                CurrentMaterialTemplate.Active = MaterialTemplate.Active;
                CurrentMaterialTemplate.Center = MaterialTemplate.Center;
                CurrentMaterialTemplate.Tag = MaterialTemplate.Tag;
                CurrentMaterialTemplate.QtyDaysAlert = MaterialTemplate.QtyDaysAlert;
                */
                db.SaveChanges();
            }
            return SpectroRequest;
        }

        public async void Delete(int MaterialId)
        {
            SpectroRequest spectroRequest = new SpectroRequest();
            using (var db = new DbSnowdenContext())
            {
                spectroRequest = db.SpectroRequests.Find(MaterialId);
                spectroRequest.Active = false;
                db.SaveChanges();
            }
        }


        public async Task<Technology> GetTechnologyById(int? TechonologyId)
        {
            Technology technology = new Technology();
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    technology = await db.Technologies.FindAsync(TechonologyId);

                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return technology;
        }

        public async Task<MeasurementType> GetMeasurementTypeById(int? MeasurementTypeId)
        {
            MeasurementType measurementType = new MeasurementType();
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    measurementType = await db.MeasurementTypes.FindAsync(MeasurementTypeId);

                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return measurementType;
        }
    }
}
