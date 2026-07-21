using AGP.Snowden.DataAccessLayer;
using AGP.Snowden.DataAccessLayer.Azure;
using AGP.Snowden.DataAccessLayer.RRHH;
using AGP.Snowden.ServiceLayer.Azure;
using AGP.Snowden.ServiceLayer.RRHH;
using AGP.Snowden.ServiceLayer.Warehouse;
using AGPSnowden.Model;
using AGPSnowden.Service;
using AGPSnowden.Service.RRHH;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Extensions.Msal;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AGPSnowden.Controllers.RRHH
{
    [Route("api/[controller]")]
    [ApiController]
    public class LendMoneyRequestController : ControllerBase
    {
        private readonly LendMoneyRequestService _LendMoneyRequestService;
        private readonly LoanRequestFilesService _LoanRequestFilesService;
        private readonly LoanMoneyRequestStatusHistoryService _LoanMoneyRequestStatusHistoryService;
        private readonly ImageService _imageService;
        private readonly IAzureStorage _storage;

        public LendMoneyRequestController(ImageService imageService)
        {
            _LendMoneyRequestService= new LendMoneyRequestService();
            _LoanRequestFilesService = new LoanRequestFilesService();
            _LoanMoneyRequestStatusHistoryService = new LoanMoneyRequestStatusHistoryService();
            _imageService= imageService;
        }
        public class LoanMoneyRequestDto
        {
            public string? DocumentNumber { get; set; }
            public string? RequestType { get; set; }

            public string? CellNumber { get; set; }
            public decimal? Salary { get; set; }
            public decimal? AmountRequested { get; set; }
            public int? Installments { get; set; }
            public float? InstallmentAmount { get; set; }
            public string? ReasonLendRequestDescription { get; set; }
            public string? beneficiaryDocumentNumber { get; set; }

            [Required]
            public IFormFile Format { get; set; }
            [Required]
            public IFormFile Support { get; set; }
        }
    
        // GET: api/<LendMoneyRequestController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<LendMoneyRequestController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<LendMoneyRequestController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] LoanMoneyRequestDto request)
        {
            LoanMoneyRequest record = new LoanMoneyRequest();
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var formatFileType= request.Format.ContentType; // Extensión
                var formatFileExtension = Path.GetExtension(request.Format.FileName); // Extensión
                var formatFileName = request.Format.FileName;
                var supportFileType = request.Support.ContentType; // Extensión
                var supportFileExtension = Path.GetExtension(request.Support.FileName); // Extensión
                var supportFileName = request.Support.FileName;
                if (!IsValidFile(formatFileType, formatFileExtension) || !IsValidFile(supportFileType, supportFileExtension))
                {
                    throw new System.ArgumentException("El archivo de formato no es válido. Solo se permiten imágenes, PDFs o Excel.");
                    //return BadRequest("El archivo de formato no es válido. Solo se permiten imágenes, PDFs o Excel.");
                }

                record.FormatFileName=await UploadFileToAzure(request.Format, record.FormatFileName);
                // extension = Path.GetExtension(request.Format.FileName);
                record.SupportFileName=await UploadFileToAzure(request.Support, record.SupportFileName);

                // Obtener la fecha y hora actual en UTC
                var utcNow = DateTimeOffset.UtcNow;

                // Convertir a la zona horaria de Perú
                var peruTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
                var peruNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow.DateTime, peruTimeZone);

                record.DocumentNumber = request.DocumentNumber;
                record.CellNumber = request.CellNumber;
               // record.BeneficiaryDocumentNumber = request.BeneficiaryDocumentNumber;
                record.Active = true;
                record.AmountRequested = request.AmountRequested;
                record.InstallmentAmount = Convert.ToDecimal(request.InstallmentAmount);
                record.ReasonLendRequestDescription= request.ReasonLendRequestDescription;
                record.Salary= request.Salary;
                record.RequestType= request.RequestType;
                record.HasFormat = true;
                record.HasSupport = true;
               // record.FormatFileName = request.Format.FileName;
               // record.SupportFileName = request.Support.FileName;


                record.Status = "GEN";
                record.CreatedAt = peruNow;
                record.UpdatedAt = peruNow;

                record = await _LendMoneyRequestService.Add(record);

                LoanMoneyRequestStatusHistory statusHistory = new LoanMoneyRequestStatusHistory();
                statusHistory.Status = record.Status;
                statusHistory.LoanMoneyRequest_Id = record.Id;
                statusHistory.Observation = "Nueva Solicitud";
                statusHistory.CreatedAt = peruNow;
                await _LoanMoneyRequestStatusHistoryService.Add(statusHistory);


                if (record.FormatFileName != null)
                {
                    LoanRequestFile loanRequestFile = new LoanRequestFile();
                    loanRequestFile.Active = true;
                    loanRequestFile.LoanRequestId = record.Id;
                    loanRequestFile.UploadedAt = peruNow;
                    loanRequestFile.FileName = record.FormatFileName;
                    loanRequestFile.OriginalFileName = formatFileName;
                    loanRequestFile.FileType = "FORMATO";
                    loanRequestFile.ContentType = request.Format.ContentType;
                    loanRequestFile.FileSize = request.Format.Length;
                    loanRequestFile.UploadedBy = record.DocumentNumber;
                    await _LoanRequestFilesService.Add(loanRequestFile);

                }

                if (record.SupportFileName != null)
                {
                    LoanRequestFile loanRequestFile = new LoanRequestFile();
                    loanRequestFile.LoanRequestId = record.Id;
                    loanRequestFile.Active = true;
                    loanRequestFile.UploadedAt = peruNow;
                    loanRequestFile.FileName = record.SupportFileName;
                    loanRequestFile.OriginalFileName = supportFileName;
                    if(record.RequestType== "EROGACION")
                        loanRequestFile.FileType = "LISTADO EXCEL";
                    else
                       loanRequestFile.FileType = "SOPORTE";
                    loanRequestFile.ContentType = request.Support.ContentType;
                    loanRequestFile.FileSize = request.Support.Length;
                    loanRequestFile.UploadedBy = record.DocumentNumber;
                    await _LoanRequestFilesService.Add(loanRequestFile);
                }
                

                /*
                                switch (request.RequestType)
                                {
                                    case "PRESTAMO":
                                            extension = Path.GetExtension(request.Format.FileName);
                                            await UploadFileToAzure(request.Format, record.Id + "_" + "format.jpg");
                                            extension = Path.GetExtension(request.Format.FileName);
                                            await UploadFileToAzure(request.Support, record.Id + "_" + "support.jpg");
                                        break;
                                    case "GRATIFICACION":
                                            string formatBlobName = await UploadFileToAzure(request.Format, record.Id + "_" + "format.jpg");
                                            string supportBlobName = await UploadFileToAzure(request.Support, record.Id + "_" + "support.jpg");
                                        break;
                                    case "EROGACION":
                                          //  string formatBlobName = await UploadFileToAzure(request.Format, record.Id + "_" + "format.jpg");
                                           // string supportBlobName = await UploadFileToAzure(request.Support, record.Id + "_" + "support.jpg");
                                        break;
                                }
                */


            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(record);
        }

        // POST api/<LendMoneyRequestController>
        [HttpGet("GetListByDni")]
        public IActionResult GetListByDni(string documentNumber)
        {
            List<LoanMoneyRequest> list = new List<LoanMoneyRequest>();
            try
            {
                list = _LendMoneyRequestService.GetByDocumentNumber(documentNumber);

                foreach (var item in list)
                {
                    switch (item.Status)
                    {
                        case "GEN":
                            item.StatusDescription = "EN EVALUACIÓN";
                            break;
                        case "REC":
                            item.StatusDescription = "RECHAZADO";
                            break;
                        case "APR":
                            item.StatusDescription = "APROBADO";
                            break;
                    }

                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok(list);
        }

        // PUT api/<LendMoneyRequestController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<LendMoneyRequestController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private async Task<string> UploadFileToAzure(IFormFile file, string fileName)
        {
            if (file == null || file.Length == 0)
                return string.Empty;

            using (var stream = file.OpenReadStream())
            {
                return await _imageService.UploadImageAsync(stream, file.FileName, "rrhh_lend_request" + "/");
            }
        }

        private bool IsValidFile(string contentType, string extension)
        {
            // Tipos MIME permitidos
            var allowedMimeTypes = new List<string>
                {
                    "image/jpeg",
                    "image/png",
                    "application/pdf",
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                };

                        // Extensiones permitidas
                        var allowedExtensions = new List<string>
                {
                    ".jpg",
                    ".jpeg",
                    ".png",
                    ".pdf",
                    ".xlsx"
                };

            return allowedMimeTypes.Contains(contentType.ToLower()) && allowedExtensions.Contains(extension.ToLower());
        }

    }


}
