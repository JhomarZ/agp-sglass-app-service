using AGP.Gordon.CommonLayer;
using AGP.Security.DataAccessLayer;
using AGP.Security.ServiceLayer;
using AGP.Snowden.DataAccessLayer;
using AGP.Snowden.DataAccessLayer.SAP;
using AGP.Snowden.ServiceLayer.RRHH;
using AGP.Snowden.ServiceLayer.Warehouse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;

namespace AGPSnowden.Controllers.Warehouse
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackingImputadosController : ControllerBase
    {
        private readonly TrackingImputadosService _TrackingImputadosService;
        private readonly ShippingStatusService _ShippingStatusService;
        private readonly PackingListService _PackingListService;
        private readonly PersonalService _PersonalService;
        private readonly AuthService _AuthService;

        public TrackingImputadosController(IConfiguration configuration)
        {
            _TrackingImputadosService = new TrackingImputadosService();
            _ShippingStatusService = new ShippingStatusService();
            _PackingListService = new PackingListService();
            _PersonalService = new PersonalService();
            _AuthService = new AuthService(configuration);
        }


        [HttpGet("List")]
        public async Task<List<VwimputadosSap>> List(int Skip, int Take , string? Descripcion , string? FechaInicio , string? Fechafin, string? Centro, string? Status )
        {
            List<VwimputadosSap>  response = new List<VwimputadosSap>();

            response = await _TrackingImputadosService.GetAll(Skip, Take, Descripcion, FechaInicio, Fechafin, Centro, Status);


         //   for (int i = 0; i < response.Count; i++)
          //  {
                
                //response[i].StatusHistory = await _PackingListService.GetHistoryStatusImputado(response[i].Centro, response[i].DocumentoCompra, response[i].NroPosicionDc, response[i].Mblnr);
                /*
                foreach (ImputadoStatusHistory item in response[i].StatusHistory)
                {
                    item.ShippingStatus= await _ShippingStatusService.GetOne(item.StatusId);
                    item.User = _AuthService.GetUserByUserName(item.CreatedBy);
                }*/
                
                /*
                response[i].PackinListItem = await _PackingListService.GetOnePackingItemB(response[i].PackingListId, response[i].Centro, response[i].DocumentoCompra, response[i].NroPosicionDc, response[i].Mblnr);
                
                if (response[i].PackinListItem != null)
                {
                    response[i].PackinListItem.PersonReceiver = await _PersonalService.GetOneByDocumentNumber(response[i].PackinListItem.DocumentNumberReceiver);
                }*/
                

                //  if(response[i].PackinListItem!=null)
                //  response[i].PackinListItem.ShippingStatus = await _ShippingStatusService.GetOne(response[i].PackinListItem.ShippingStatusId);
         //   }
            /*
            for (int i = 0; i < response.Count; i++)
            {
                response[i].NumeroMaterial = (response[i].NumeroMaterial.Length>9)?response[i].NumeroMaterial.Substring(9): response[i].NumeroMaterial;
                //response[i].Extension = await _TrackingImputadosService.GetTrackinImputadoExtensionByKey(response[i].Centro, response[i].DocumentoCompra, response[i].NroPosicionDC, response[i].MBLNR);
            }*/


            return response;
        }

        [HttpGet("GetOne")]
        public async Task<VwimputadosSap> GetOne(string Centro, string DocumentoCompra, string NroPosicionDc, string Mblnr)
        {
            VwimputadosSap response = new VwimputadosSap();

            response = await _TrackingImputadosService.GetTrackinImputadoSapByKey(Centro,DocumentoCompra,NroPosicionDc,Mblnr);

            if (response != null)
            {
                response.StatusHistory = await _PackingListService.GetHistoryStatusImputado(response.Centro, response.DocumentoCompra, response.NroPosicionDc, response.Mblnr);

                foreach (ImputadoStatusHistory item in response.StatusHistory)
                {
                    item.ShippingStatus = await _ShippingStatusService.GetOne(item.StatusId);
                    item.User = _AuthService.GetUserByUserName(item.CreatedBy);
                }

                response.PackinListItem = await _PackingListService.GetOnePackingItemB(response.PackingListId, response.Centro, response.DocumentoCompra, response.NroPosicionDc, response.Mblnr);

                if (response.PackinListItem != null)
                {
                    response.PackinListItem.PersonReceiver = await _PersonalService.GetOneByDocumentNumber(response.PackinListItem.DocumentNumberReceiver);
                }

            }

            return response;
        }


        [HttpGet("ListByDocumentoCompra")]
        public async Task<List<VwimputadosSap>> ListByDocumentoCompra(string DocumentoCompra)
        {
            List<VwimputadosSap> response = new List<VwimputadosSap>();

            response = await _TrackingImputadosService.GetImputadoSapByDocumentoCompra(DocumentoCompra);


            foreach (VwimputadosSap item in response)
            {
                if(item.PackingListId != null)
                {
                    item.PackinListItem = await _PackingListService.GetOnePackingItemB(item.PackingListId, item.Centro, item.DocumentoCompra, item.NroPosicionDc, item.Mblnr);
                    if (item.PackinListItem != null)
                        item.StatusHistory = await _PackingListService.GetHistoryStatusImputado(item.Centro, item.DocumentoCompra, item.NroPosicionDc, item.Mblnr);
                    if (item.StatusHistory != null)
                    {
                        foreach (var status in item.StatusHistory)
                        {
                            status.User = _AuthService.GetUserByUserName(status.CreatedBy);
                        }
                    }
                }
                
            }
            

            return response;
        }

        [HttpGet("GenerarEtiqueta")]
        public async Task<IActionResult> GenerarEtiqueta(string Centro, string DocumentoCompra, string NroPosicion,string MBLNR, int Bultos, int Copias, string? Usuario)
        //public async Task<IActionResult> GenerarEtiqueta(List<VwimputadosSap> Imputados)
        {
            HelpImage _HelpImage = new HelpImage();

            string response = "resultado";

            try
            {
                // Obtener la fecha y hora actual en UTC
                var utcNow = DateTimeOffset.UtcNow;

                // Convertir a la zona horaria de Perú
                var peruTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
                var peruNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow.DateTime, peruTimeZone);

                VwimputadosSap imputadoSap = await _TrackingImputadosService.GetTrackinImputadoSapByKey(Centro, DocumentoCompra, NroPosicion, MBLNR);

                if (imputadoSap == null) { throw new System.ArgumentException("No se encontro el imputado en la BD de SAP "); }

                //byte[] QR = _TrackingImputadosService.GenerarCodigoQR(DocumentoCompra+"-"+ Usuario+"-"+Area);
                string UrlQr = "https://snowdenappqas.azurewebsites.net/tracking-detail/"+DocumentoCompra;
                byte[] QR = _TrackingImputadosService.GenerarCodigoQR(UrlQr);

                // QR = _HelpImage.ResizeImage(QR, 50, 50);

                string pdfname = _TrackingImputadosService.GenerarEtiquetaZebraPDF(QR, Bultos, Copias, DocumentoCompra, imputadoSap.Responsable, imputadoSap.Centro+"_"+ imputadoSap.SolicitanteNombre);

                byte[] fileBytesPDF = HelpImage.GetFileContent(pdfname);
                TrackingImputadosExtension trackingImputadosExtension = await _TrackingImputadosService.GetTrackinImputadoExtensionByKey(imputadoSap.Centro, imputadoSap.DocumentoCompra, imputadoSap.NroPosicionDc, imputadoSap.Mblnr);
                if (trackingImputadosExtension == null)
                {
                    trackingImputadosExtension = new TrackingImputadosExtension();
                    trackingImputadosExtension.CentroSap = imputadoSap.Centro;
                    trackingImputadosExtension.NroPosicion = imputadoSap.NroPosicionDc;
                    trackingImputadosExtension.DocumentoCompra = imputadoSap.DocumentoCompra;
                    trackingImputadosExtension.MBLNR = imputadoSap.Mblnr;
                    trackingImputadosExtension.Bultos = Bultos;
                    trackingImputadosExtension.CreatedAt = peruNow;
                    trackingImputadosExtension.CreatedBy = Usuario;
                    trackingImputadosExtension = await _TrackingImputadosService.Add(trackingImputadosExtension);

                }
                
                return File(fileBytesPDF, "image/pdf", System.IO.Path.GetFileName(pdfname));

            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            //return response;
        }

        [HttpPost("GenerarEtiquetaMasivo")]
        public async Task<IActionResult> GenerarEtiqueta(ParametrosToPrintMasivo Parametros/* List<VwimputadosSap> Imputados/*,int Bultos,int Copias,string Usuario*/)
        //public async Task<IActionResult> GenerarEtiqueta(List<VwimputadosSap> Imputados)
        {
            List<VwimputadosSap> Imputados = Parametros.Imputados;
            HelpImage _HelpImage = new HelpImage();

            string response = "resultado";

            try
            {
                // Obtener la fecha y hora actual en UTC
                var utcNow = DateTimeOffset.UtcNow;

                // Convertir a la zona horaria de Perú
                var peruTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
                var peruNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow.DateTime, peruTimeZone);

                // VwimputadosSap imputadoSap = await _TrackingImputadosService.GetTrackinImputadoSapByKey(Centro, DocumentoCompra, NroPosicion, MBLNR);

                if (Imputados == null) { throw new System.ArgumentException("No se encontro el imputado en la BD de SAP "); }

                //byte[] QR = _TrackingImputadosService.GenerarCodigoQR(DocumentoCompra+"-"+ Usuario+"-"+Area);

                foreach (var item in Imputados)
                {
                    string UrlQr = "https://snowdenappqas.azurewebsites.net/tracking-detail/"+ item.DocumentoCompra;
                    item.QR = _TrackingImputadosService.GenerarCodigoQR(UrlQr);
                }


                // QR = _HelpImage.ResizeImage(QR, 50, 50);

                string pdfname = _TrackingImputadosService.GenerarEtiquetaMasivoZebraPDF(Parametros.Bultos, Parametros.Copias, Imputados);

                byte[] fileBytesPDF = HelpImage.GetFileContent(pdfname);

                foreach (var item in Imputados)
                {
                    TrackingImputadosExtension trackingImputadosExtension = await _TrackingImputadosService.GetTrackinImputadoExtensionByKey(item.Centro, item.DocumentoCompra, item.NroPosicionDc, item.Mblnr);
                    if (trackingImputadosExtension == null)
                    {
                        trackingImputadosExtension = new TrackingImputadosExtension();
                        trackingImputadosExtension.CentroSap = item.Centro;
                        trackingImputadosExtension.NroPosicion = item.NroPosicionDc;
                        trackingImputadosExtension.DocumentoCompra = item.DocumentoCompra;
                        trackingImputadosExtension.MBLNR = item.Mblnr;
                        trackingImputadosExtension.Bultos = Parametros.Bultos;
                        trackingImputadosExtension.CreatedBy = Parametros.Usuario;
                        trackingImputadosExtension.CreatedAt = peruNow;
                            trackingImputadosExtension.UpdatedAt = peruNow;
                        trackingImputadosExtension = await _TrackingImputadosService.Add(trackingImputadosExtension);

                    }
                    else
                    {
                        trackingImputadosExtension.UpdatedBy = Parametros.Usuario;
                        trackingImputadosExtension.Bultos = Parametros.Bultos;
                        trackingImputadosExtension.CreatedAt = peruNow;
                        trackingImputadosExtension.UpdatedAt = peruNow;
                        trackingImputadosExtension = await _TrackingImputadosService.Update(trackingImputadosExtension);
                    }

                }

               

                return File(fileBytesPDF, "image/pdf", System.IO.Path.GetFileName(pdfname));

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            //return response;
        }

        [HttpGet("ShippmentStatuses")]
        public async Task<List<ShippingStatus>> ShippmentStatuses(string? Centro)
        {
            List<ShippingStatus> response = new List<ShippingStatus>();

            response = await _ShippingStatusService.GetAll(Centro);

            return response;
        }

    }

    public partial class ParametrosToPrintMasivo
    {
        public List<VwimputadosSap> Imputados { get; set; }
        public int Bultos { get; set; }
        public int Copias { get; set; }
        public string Usuario { get; set; }
    }
 
}
