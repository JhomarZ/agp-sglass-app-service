using AGP.Gordon.CommonLayer;
using AGP.Snowden.DataAccessLayer;
using AGP.Snowden.ServiceLayer.RD;
using AGP.Snowden.ServiceLayer.Warehouse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AGPSnowden.Controllers.RD
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpectroRequestController : ControllerBase
    {
        private readonly SpectroRequestService _SpectroRequestService;
        public SpectroRequestController()
        {
            _SpectroRequestService = new SpectroRequestService();
        }

        [HttpGet("List")]
        public async Task<ResponseDataGrid> List(int Page = 0, int Rows = 20, string Centro = "PE02", string? Description = "")
        {
            ResponseDataGrid response = new ResponseDataGrid();

            response = await _SpectroRequestService.GetAll(Page, Rows, Centro, Description);

            
            if (response.rows != null)
            {
                foreach (SpectroRequest item in response.rows)
                {
                    int? pTechonologyId = item.TechnologyId;
                    int? pMeasurementTypeId = item.MeasurementTypeId;
                    item.Technology = await _SpectroRequestService.GetTechnologyById(pTechonologyId);
                    item.SideDescription = item.GetSide();
                    item.MeasurementType= await _SpectroRequestService.GetMeasurementTypeById(pMeasurementTypeId);
                    // item.MaterialCategory = _MaterialTemplateService.GetCategory(pCategoryId);
                    // item.MaterialGroup = _MaterialTemplateService.GetMaterialGroup(pMaterialType);
                }
            }

            return response;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            SpectroRequest spectroRequest = new SpectroRequest();
            //input = await _InputService.GetById(id);

            return Ok(spectroRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Add(SpectroRequest spectroRequest)
        {
            try
            {
                spectroRequest = await _SpectroRequestService.Add(spectroRequest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(spectroRequest);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, SpectroRequest spectroRequest)
        {
            try
            {
                spectroRequest = await _SpectroRequestService.Update(id, spectroRequest);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(spectroRequest);
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _SpectroRequestService.Delete(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }
    }
}
