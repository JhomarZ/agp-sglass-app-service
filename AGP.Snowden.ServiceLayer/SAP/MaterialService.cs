using AGP.Snowden.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGP.Snowden.ServiceLayer.SAP
{
    public class MaterialService
    {
        public async  Task<Material> GetMaterialByMaterialNumber(int MaterialNumber )
        {
            Material materials = new Material();
            using (var db = new DbSnowdenContext())
            {
                materials = db.Materials.Where(x => x.MaterialKey == MaterialNumber).FirstOrDefault();
                if(materials!= null)
                {
                    string materialTypeGroup = materials.MaterialGroup;
                    //materials.MaterialType .MaterialType = db.MaterialTypes.Where(x => x.MaterialTypeGroup == materialTypeGroup).FirstOrDefault();
                }    
            }
            return materials;
        }

        public List<Material> GetMaterialByDescription(string Description)
        {
            List<Material> materials = new List<Material>();
            using (var db = new DbSnowdenContext())
            {
                materials = db.Materials.Where(x => x.MaterialDescription.Contains(Description)).ToList();
            }
            return materials;
        }

    }
}
