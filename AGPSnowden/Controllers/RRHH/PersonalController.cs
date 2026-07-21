using AGP.Snowden.DataAccessLayer;
using AGP.Snowden.ServiceLayer.RRHH;
using AGP.Snowden.ServiceLayer.Warehouse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AGPSnowden.Controllers.RRHH
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalController : ControllerBase
    {
        private readonly PersonalService _PersonalService;

        public PersonalController()
        {
            _PersonalService = new PersonalService();
        }

        [HttpGet("List")]
        public async Task<List<Personal>> List(int Skip, int Take, string? Description)
        {
            List<Personal> response = new List<Personal>();

            response = await _PersonalService.GetAll(Skip, Take, Description);
            
            return response;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string DocumentNumber="")
        {
            Personal personal = new Personal();
            try
            {
                personal = await _PersonalService.GetOneByDocumentNumber(DocumentNumber);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(personal);
        }
    }
}
