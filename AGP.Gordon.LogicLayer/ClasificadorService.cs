using AGP.Gordon.DataAccessLayer.SAPEXPANSION;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGP.Gordon.ServiceLayer
{
    public class ClasificadorService
    {
        public Clasificadore GetClasificadorById(int Id)
        {
            Clasificadore clasificadore = new Clasificadore();
            using (var db = new SapexpansionContext())
            {
                clasificadore = db.Clasificadores.Where(x => x.Id== Id).First();
            }
            return clasificadore;
        }
    }
}
