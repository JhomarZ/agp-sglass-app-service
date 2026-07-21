using AGP.Gordon.CommonLayer;
using AGP.Snowden.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AGP.Snowden.ServiceLayer.Warehouse
{
    public class CharacteristicInputService
    {
      
        public async Task<ResponseDataGrid> GetAll(int Page, int Rows, string Centro,int? InspectonPlanId, int? ValidationPlanId, int? CharacteristicId,bool Active,string Description)
        {
            ResponseDataGrid responseDataGrid = new ResponseDataGrid();
            List<CharacteristicInput> characteristicInput = new List<CharacteristicInput>();
            using (var db = new DbSnowdenContext())
            {
                var Query =db.CharacteristicInputs.Where(x=>x.Center== Centro ).AsQueryable();

                if (InspectonPlanId != null && InspectonPlanId>0)
                {
                    Query = Query.Where(d => d.InspectionPlanId==InspectonPlanId);
                }
                if (ValidationPlanId != null && ValidationPlanId > 0)
                {
                    Query = Query.Where(d => d.ValidationPlanId == ValidationPlanId);
                }
                if (CharacteristicId != null && CharacteristicId > 0)
                {
                    Query = Query.Where(d => d.CharacteristicId == CharacteristicId);
                }
                if (Description != null && Description !="")
                {
                    Query = Query.Where(d => d.Name.ToUpper().Contains(Description.ToUpper()));
                }

                int rowsTotal = 0;
                rowsTotal = Query.Count();
                int filaInicial = Convert.ToInt32((Page - 1) * Rows);
                int totalPaginas = (rowsTotal + Rows - 1) / Rows;

                characteristicInput = await Query.OrderByDescending(x => x.Id).Skip(filaInicial).Take(Rows).ToListAsync();
                responseDataGrid.rows = characteristicInput;
                responseDataGrid.totalRows = rowsTotal;
                responseDataGrid.totalPages = totalPaginas;
                responseDataGrid.startRecord = filaInicial;
                responseDataGrid.finishRecord = filaInicial + Rows;
            }

            return responseDataGrid;
        }

        public async Task<CharacteristicInput> GetById(int CharacteristicInputId)
        {
            CharacteristicInput characteristicInput = new CharacteristicInput();
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    characteristicInput = await db.CharacteristicInputs.FindAsync(CharacteristicInputId);

                }
            }
            catch(Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }
            
            return characteristicInput;
        }
        public async Task<CharacteristicInput> Add(CharacteristicInput CharacteristicInput)
        {
            
            using (var db = new DbSnowdenContext())
            {
                 db.CharacteristicInputs.Add(CharacteristicInput);
                db.SaveChanges();
            }
            return CharacteristicInput;
        }

        public async Task<CharacteristicInput> Update(int CharacteristicInputId,CharacteristicInput CharacteristicInput)
        {
            CharacteristicInput CurrentCharacteristicInput = new CharacteristicInput();
            using (var db = new DbSnowdenContext())
            {
                CurrentCharacteristicInput = db.CharacteristicInputs.Find(CharacteristicInputId);
                CurrentCharacteristicInput.InspectionPlanId = CharacteristicInput.InspectionPlanId;
                CurrentCharacteristicInput.ValidationPlanId = CharacteristicInput.ValidationPlanId;
                CurrentCharacteristicInput.CharacteristicId = CharacteristicInput.CharacteristicId;
                CurrentCharacteristicInput.Name = CharacteristicInput.Name;
                CurrentCharacteristicInput.Type = CharacteristicInput.Type;
                CurrentCharacteristicInput.List = CharacteristicInput.List;
                CurrentCharacteristicInput.Min = CharacteristicInput.Min;
                CurrentCharacteristicInput.Max = CharacteristicInput.Max;
                CurrentCharacteristicInput.Center = CharacteristicInput.Center;
                CurrentCharacteristicInput.Active = CharacteristicInput.Active;
                CurrentCharacteristicInput.UnitMeasure = CharacteristicInput.UnitMeasure;
                db.SaveChanges();
            }
            return CharacteristicInput;
        }

        public async void Delete(int CharacteristicInputId)
        {
            CharacteristicInput CurrentCharacteristicInput = new CharacteristicInput();
            using (var db = new DbSnowdenContext())
            {
                CurrentCharacteristicInput = db.CharacteristicInputs.Find(CharacteristicInputId);
                CurrentCharacteristicInput.Active = false;
                db.SaveChanges();
            }
        }

        public async Task<List<InspectionPlan>> GetAllInspectionPlan(string Centro = "PE02")
        {
            List<InspectionPlan> inspectionPlanList = new List<InspectionPlan>();
            using (var db = new DbSnowdenContext())
            {
                inspectionPlanList = await db.InspectionPlans.Where(x => x.Center == Centro && x.Active == true).OrderBy(x => x.Name).ToListAsync();
            }

            return inspectionPlanList;
        }
        public async Task<List<ValidationPlan>> GetAllValidationPlan(string Centro = "PE02")
        {
            List<ValidationPlan> list = new List<ValidationPlan>();
            using (var db = new DbSnowdenContext())
            {
                list = await db.ValidationPlans.Where(x => x.Center == Centro && x.Active == true).OrderBy(x => x.Name).ToListAsync();
            }

            return list;
        }

        public async Task<List<CharacteristicInspectionPlan>> GetAllCharateristics(string? Centro = "PE02")
        {
            List<CharacteristicInspectionPlan> list = new List<CharacteristicInspectionPlan>();
            using (var db = new DbSnowdenContext())
            {
                list = await db.CharacteristicInspectionPlans.Where(x => x.Center == Centro && x.Active == true).OrderBy(x=>x.Name).ToListAsync();
            }

            return list;
        }
    }
}
