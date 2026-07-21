using AGPSnowden.Model;
using AGPSnowden.Model.Scada;
using AGPSnowden.Service.Scada;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AGPSnowden.Controllers.Scada
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScadaController : ControllerBase

    {

        private readonly SapOrderService _SapOrderService;
        // GET: STable
        public ScadaController()
        {
            _SapOrderService = new SapOrderService(); ;
        }

        // GET: api/<ScadaController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ScadaController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ScadaController>
        [HttpPost]
        public async Task<Response> Post(SapOrder order)
        {
            Response response = new Response();
            try
            {
                order = await _SapOrderService.Add(order);
                response.Success = true;
                response.Message = "Se guardo exitosamente la orden " + order.Orden;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                return response;
            }

            return response;
        }

    }
}
