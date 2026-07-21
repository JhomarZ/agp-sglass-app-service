using AGP.Gordon.CommonLayer;
using AGP.Snowden.ServiceLayer.RD;
using AGPSnowden.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AGPSnowden.Controllers.RD
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestRequestController : ControllerBase
    {
        // GET: api/<TestRequestController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<TestRequestController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TestRequestController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TestRequestController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TestRequestController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }


        [HttpGet("GenerarExcel")]
        public async Task<IActionResult> GenerarExcel(string TestRequestIds)
        {
            //string plantillaRuta = "Reports/Templates/template1.xlsx";
            string plantillaRuta = "Reports/Templates/RD/Measurements.xlsm";
            string resultadoRuta = "Reports/Results/RD/";
            HelpImage _HelpImage = new HelpImage();

            TestRequestService _TestRequestService = new TestRequestService();
            Response response = new Response();
            
            try
            {

                //_HelpImage.EmptyFolder(new DirectoryInfo(resultadoRuta));

                // If directory does not exist, create it
                if (!Directory.Exists(resultadoRuta))
                {
                    Directory.CreateDirectory(resultadoRuta);
                }

                resultadoRuta = resultadoRuta  + DateTime.Now.ToString("ddmm_HHss") + ".xlsm";

                DataTable dt = new DataTable();
                dt = _TestRequestService.GetDataMeasurementPivot(TestRequestIds);

                response.Success= _TestRequestService.LoadExcelDataMeasurement(plantillaRuta, resultadoRuta,dt);


                return File(System.IO.File.ReadAllBytes(resultadoRuta), "application/octet-stream", System.IO.Path.GetFileName(resultadoRuta));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }



            return Ok();
        }


    }
}
