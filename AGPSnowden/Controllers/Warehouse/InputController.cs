using AGP.Gordon.CommonLayer;
using AGP.Snowden.DataAccessLayer;
using AGP.Snowden.ServiceLayer.Warehouse;
using AGPSnowden.Model;
using AGPSnowden.Service;
using AGPSnowden.Service.Audits;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;


namespace AGPSnowden.Controllers.Warehouse
{
    [Route("api/[controller]")]
    [ApiController]
    public class InputController : ControllerBase
    {
        private readonly CharacteristicInputService _InputService;
        private readonly InspectionPlanService _InspectionPlanService;
        private readonly ValidationPlanService _ValidationPlanService;
        private readonly CharacteristicPlanService _CharacteristicPlanService;
        
        public InputController()
        {
            _InputService = new CharacteristicInputService();
            _InspectionPlanService = new InspectionPlanService();
            _ValidationPlanService = new ValidationPlanService();
            _CharacteristicPlanService = new CharacteristicPlanService();

        }


        [HttpGet("List")]
        public async Task<ResponseDataGrid> List(int Page = 0, int Rows = 20, string centro = "PE02", int inspectionPlanId = 0, int validationPlanId = 0, int characteristicId = 0,
            bool Active=true, string? description = "")
        {
            ResponseDataGrid response = new ResponseDataGrid();

            response = await _InputService.GetAll(Page, Rows, centro, inspectionPlanId, validationPlanId, characteristicId, Active, description);

            if (response.rows != null)
            {
                foreach (CharacteristicInput item in response.rows)
                {
                    int? pInspectionPlanId = item.InspectionPlanId;
                    item.InspectionPlan = await _InspectionPlanService.GetById(pInspectionPlanId);
                    int? pValidationPlanId = item.ValidationPlanId;
                    item.ValidationPlan = await _ValidationPlanService.GetById(pValidationPlanId);
                    int? pCharacteristicPlanId = item.CharacteristicId;
                    item.Characteristic = await _CharacteristicPlanService.GetById(pCharacteristicPlanId);
                }
            }

            return response;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            CharacteristicInput input = new CharacteristicInput();
            input = await _InputService.GetById(id);

            return Ok(input);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CharacteristicInput input)
        {
            try
            {
                

                input = await _InputService.Add(input);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(input);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, CharacteristicInput input)
        {
            try
            {

                input = await _InputService.Update(id, input);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(input);
        }


        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                 _InputService.Delete(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(true);
        }

        [HttpGet("InspectionPlan")]
        public async Task<List<InspectionPlan>> InspectionPlan(string Centro = "PE02")
        {
            List<InspectionPlan> lista = new List<InspectionPlan>();

            lista = await _InputService.GetAllInspectionPlan(Centro);

            return lista;
        }
        [HttpGet("ValidationPlan")]
        public async Task<List<ValidationPlan>> ValidationPlan(string Centro = "PE02")
        {
            List<ValidationPlan> lista = new List<ValidationPlan>();

            lista = await _InputService.GetAllValidationPlan(Centro);

            return lista;
        }
        [HttpGet("CharacteristicPlan")]
        public async Task<List<CharacteristicInspectionPlan>> CharacteristicPlan(string Centro = "PE02")
        {
            List<CharacteristicInspectionPlan> lista = new List<CharacteristicInspectionPlan>();

            lista = await _InputService.GetAllCharateristics(Centro);

            return lista;
        }

    }
}
