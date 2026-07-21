using AGP.Snowden.DataAccessLayer;
using AGPSnowden.Model;
using AGPSnowden.Service;
using AGPSnowden.Service.Audits;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace AGPSnowden.Controllers.Audits
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditController : ControllerBase
    {
        private readonly AuditService _AuditService;
        private readonly AuditTypeService _AuditTypeService;
        private readonly AuditSubTypeService _AuditSubTypeService;
        private readonly ProductService _ProductService;
        // GET: STable
        public AuditController()
        {
            _AuditService = new AuditService();
            _AuditTypeService = new AuditTypeService();
            _AuditSubTypeService = new AuditSubTypeService();
            _ProductService = new ProductService();
        }

        [HttpGet("PageInfo")]
        public async Task<IActionResult> PageInfo(string centro = "")
        {
            Response response = new Response();
          
            List<AuditType> types = new List<AuditType>();
            List<AuditSubType> subTypes = new List<AuditSubType>();
            List<Product> products = new List<Product>();

            types = _AuditTypeService.GetAll(centro,"",0,1000);
            subTypes = _AuditSubTypeService.GetAll(centro, "", 0, 100);
            products = _ProductService.GetAll(0, 1000, centro);
            response.Success = true;
           // response.Data["types"] =types;
           // response.Data["subtypes"] = subTypes;
           // response.Data["products"] = products;

            dynamic MyDynamic = new System.Dynamic.ExpandoObject();
            MyDynamic.types = types;
            MyDynamic.subTypes = subTypes;
            MyDynamic.products = products;

            //audits = await _AuditService.List(start, records, centro, auditTypeId, auditSubTypeId, productId, responsableId, description);


            return Ok(MyDynamic);
        }

        [HttpGet("List")]
        public async Task<List<Audit>> List(int start = 0, int records = 20, string centro = "", int? auditTypeId = null, int? auditSubTypeId = null, int? productId = null,
            string shift = "", string description = "")
        {
            List<Audit> audits = new List<Audit>();

            audits =  await _AuditService.List(start,records,centro,auditTypeId,auditSubTypeId,productId, shift, description);

            return  audits;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            Audit audit= new Audit();
            audit = await _AuditService.GetOne(id);
            
            return Ok(audit);
        }


        [HttpPost]
        public async Task<IActionResult> Add(Audit audit)
        {
            try
            {
                audit = await _AuditService.Add(audit);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(audit);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id,Audit audit)
        {
            try
            {
                audit = await _AuditService.Edit(id,audit);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(audit);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Audit audit = await _AuditService.Delete(id);

            return Ok(audit);
        }


    }
       
}
