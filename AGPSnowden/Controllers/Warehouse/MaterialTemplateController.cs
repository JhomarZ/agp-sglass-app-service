using AGP.Gordon.CommonLayer;
using AGP.Snowden.DataAccessLayer;
using AGP.Snowden.ServiceLayer.Warehouse;
using Microsoft.AspNetCore.Mvc;

namespace AGPSnowden.Controllers.Warehouse
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialTemplateController : ControllerBase
    {
        private readonly MaterialTemplateService _MaterialTemplateService;
        private readonly InspectionPlanService _InspectionPlanService;
        public MaterialTemplateController()
        {
            _MaterialTemplateService = new MaterialTemplateService();
            _InspectionPlanService = new InspectionPlanService();
        }

        [HttpGet("List")]
        public async Task<ResponseDataGrid> List(int Page = 0, int Rows = 20, string Centro = "PE02", string? Description = "")
        {
            ResponseDataGrid response = new ResponseDataGrid();

            response = await _MaterialTemplateService.GetAll(Page, Rows, Centro, Description);

            if (response.rows != null)
            {
                foreach (MaterialTemplate item in response.rows)
                {
                    int? pInspectionPlanId = item.InspectionPlanId;
                    int? pCategoryId = item.MaterialCategoryId;
                    string pMaterialType= item.MaterialTypeGroup;
                    item.InspectionPlan = await _InspectionPlanService.GetById(pInspectionPlanId);
                   // item.MaterialCategory = _MaterialTemplateService.GetCategory(pCategoryId);
                   // item.MaterialGroup = _MaterialTemplateService.GetMaterialGroup(pMaterialType);
                }
            }

            return response;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            CharacteristicInput input = new CharacteristicInput();
            //input = await _InputService.GetById(id);

            return Ok(input);
        }

        [HttpPost]
        public async Task<IActionResult> Add(MaterialTemplate materialTemplate)
        {
            try
            {
                materialTemplate.Active = true;
                
                MaterialTemplate materialTemplateCurrent = new MaterialTemplate();
                materialTemplateCurrent =await _MaterialTemplateService.GetById(materialTemplate.Id);
                if (materialTemplateCurrent != null) { throw new System.ArgumentException("Material ya tiene una plantilla asignada"); }

                materialTemplate = await _MaterialTemplateService.Add(materialTemplate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(materialTemplate);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, MaterialTemplate materialTemplate)
        {
            try
            {
                materialTemplate = await _MaterialTemplateService.Update(id, materialTemplate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(materialTemplate);
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _MaterialTemplateService.Delete(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }
    }
}
