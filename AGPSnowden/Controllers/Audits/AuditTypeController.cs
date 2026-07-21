using AGP.Snowden.DataAccessLayer;
using AGPSnowden.Model;
using AGPSnowden.Service.Audits;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AGPSnowden.Controllers.Audits
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditTypeController : ControllerBase
    {

        private readonly AuditTypeService _AuditTypeService;
        // GET: STable
        public AuditTypeController()
        {
            _AuditTypeService = new AuditTypeService(); ;
        }

        // GET: api/<AuditTypeController>
        [HttpGet]
        public IEnumerable<AuditType> Get()
        {
            List<AuditType> list = new List<AuditType>();

            //list=_AuditTypeService.Get();

            return list;
        }

        // GET api/<AuditTypeController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<AuditTypeController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<AuditTypeController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<AuditTypeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
