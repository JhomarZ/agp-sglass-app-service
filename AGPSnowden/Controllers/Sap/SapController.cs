using AGP.Snowden.DataAccessLayer;
using AGP.Snowden.ServiceLayer.SAP;
using AGP.Snowden.ServiceLayer.Warehouse;
using Microsoft.AspNetCore.Mvc;

namespace AGPSnowden.Controllers.Sap
{
    [Route("api/[controller]")]
    [ApiController]
    public class SapController : ControllerBase
    {
        private readonly MaterialService _MaterialService;

        public SapController()
        {
            _MaterialService = new MaterialService();

        }

        [HttpGet("get-material/{id}")]
        public async Task<IActionResult> GetMaterial(int id)
        {
            Material material = new Material();
            material = await _MaterialService.GetMaterialByMaterialNumber(id);

            return Ok(material);
        }
    }
}
