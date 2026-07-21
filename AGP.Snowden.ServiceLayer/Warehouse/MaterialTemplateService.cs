using AGP.Gordon.CommonLayer;
using AGP.Snowden.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGP.Snowden.ServiceLayer.Warehouse
{
    public class MaterialTemplateService
    {
        public async Task<ResponseDataGrid> GetAll(int Page, int Rows, string Centro, string Description)
        {
            ResponseDataGrid responseDataGrid = new ResponseDataGrid();
            List<MaterialTemplate> list = new List<MaterialTemplate>();
            using (var db = new DbSnowdenContext())
            {
                var Query = db.MaterialTemplates.Where(x => x.Center == Centro ).AsQueryable();

                
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

        public async Task<MaterialTemplate> GetById(int MaterialTemplateId)
        {
            MaterialTemplate materialTemplate = new MaterialTemplate();
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    materialTemplate = await db.MaterialTemplates.FindAsync(MaterialTemplateId);

                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            return materialTemplate;
        }
        public async Task<MaterialTemplate> Add(MaterialTemplate MaterialTemplate)
        {

            using (var db = new DbSnowdenContext())
            {
                db.MaterialTemplates.Add(MaterialTemplate);
                db.SaveChanges();
            }
            return MaterialTemplate;
        }

        public async Task<MaterialTemplate> Update(int MaterialId, MaterialTemplate MaterialTemplate)
        {
            MaterialTemplate CurrentMaterialTemplate = new MaterialTemplate();
            using (var db = new DbSnowdenContext())
            {
                CurrentMaterialTemplate = db.MaterialTemplates.Find(MaterialId);
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
                CurrentMaterialTemplate.QtyDaysAlert=   MaterialTemplate.QtyDaysAlert;

                db.SaveChanges();
            }
            return MaterialTemplate;
        }

        public async void Delete(int MaterialId)
        {
            MaterialTemplate CurrentMaterialTemplate = new MaterialTemplate();
            using (var db = new DbSnowdenContext())
            {
                CurrentMaterialTemplate = db.MaterialTemplates.Find(MaterialId);
                CurrentMaterialTemplate.Active = false;
                db.SaveChanges();
            }
        }

        public MaterialCategory GetCategory(int? CategoryId)
        {
            MaterialCategory Category = new MaterialCategory();
            using (var db = new DbSnowdenContext())
            {
                Category = db.MaterialCategories.Where(x=>x.Id== CategoryId).FirstOrDefault();
                
            }
            return Category;
        }
        public MaterialType GetMaterialGroup(string MaterialGroup)
        {
            MaterialType grupo = new MaterialType();
            using (var db = new DbSnowdenContext())
            {
                grupo = db.MaterialTypes.Where(x => x.MaterialTypeGroup == MaterialGroup).FirstOrDefault();

            }
            return grupo;
        }

    }

}
