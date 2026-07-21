using AGP.Gordon.CommonLayer;
using AGP.Gordon.DataAccessLayer.SAPEXPANSION;
using AGP.Gordon.ServiceLayer;
using AGPSnowden.Model;
using AGPSnowden.Service.Audits;
using AGPSnowden.Service.TestPDF;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Differencing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Drawing;
using System.IO.Compression;
using System.Net;

namespace AGPSnowden.Controllers.TestPDF
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestPDFController : Controller
    {
        private readonly TestPDFService _TestPDFService;
        private readonly CertificadoIFService _CertificadoIfService;
        private readonly ClasificadorService _ClasificadorService;
        private readonly HelpImage _HelpImage ;
        // GET: STable
        public TestPDFController()
        {
            _TestPDFService = new TestPDFService();
            _CertificadoIfService= new CertificadoIFService();
            _ClasificadorService = new ClasificadorService();
            _HelpImage = new HelpImage();
        }

        [HttpGet("GenerarPDF")]
        public async Task<IActionResult> GenerarPDF(long CertificadoId,string Idioma="I")
        {
           // HelpImage _HelpImage = new HelpImage();
            Response response = new Response();
            try
            {
                string resultadoRuta = "Reports/Results/";
                string UrlImageGordon = "http://20.197.228.211:8081/Userimage/";
                string pdfname = resultadoRuta + CertificadoId.ToString()+"/";
                CertificadoIf certificado = _CertificadoIfService.GetById(CertificadoId);
                PiezaSap PIEZA = _CertificadoIfService.GetPiezaByOrden(certificado.IdCompania, certificado.OrdProceso);
                List<CertificadoIfdimension> DIMENSIONAL_RESULT =  await _CertificadoIfService.GetMedicionesDimensionales(CertificadoId);
                List<CertificadoIfapariencias> APARIENCIA_RESULT= await _CertificadoIfService.GetDatosApariencia(CertificadoId);
                List<InspeccionOptica> INSPECCIONES_OPTICAS = await _CertificadoIfService.GetInspeccionesOpticas(CertificadoId);
                List<PiezaConcesion> DEFECTOS = _CertificadoIfService.GetImagenTecnicaObservaciones(CertificadoId);

                if (APARIENCIA_RESULT.Any())
                {
                    for (int i = 0; i < APARIENCIA_RESULT.Count; i++)
                    {
                        // validamos si tiene el parametro de colo
                        if(APARIENCIA_RESULT[i].ParametroInspeccionId== 458 || APARIENCIA_RESULT[i].ParametroInspeccionId == 502)
                        {
                            if (APARIENCIA_RESULT[i].Valor != "" && APARIENCIA_RESULT[i].Valor != null)
                            {
                                Clasificadore clasificador = _ClasificadorService.GetClasificadorById(Convert.ToInt32(APARIENCIA_RESULT[i].Valor));
                                APARIENCIA_RESULT[i].Valor = (clasificador != null)?clasificador.Nombre:"";
                            }
                        }
                    }

                }  
                    


                pdfname = resultadoRuta + PIEZA.LoteLogistico + ".pdf";

                _HelpImage.EmptyFolder(new DirectoryInfo(resultadoRuta));


                resultadoRuta = resultadoRuta + CertificadoId.ToString() + "/";

                // If directory does not exist, create it
                if (!Directory.Exists(resultadoRuta))
                {
                    Directory.CreateDirectory(resultadoRuta);
                }


                foreach (InspeccionOptica ins in INSPECCIONES_OPTICAS)
                {
                    ins.UrlImage = UrlImageGordon + PIEZA.IdCompania + "/Certificado/EvaluacionOpticaSAP/" + ins.CertificadoId +"/"+ ins.CertificadoId + "_" + ins.ParametroInspeccionId + ".jpg";//ins.ImageByte=

                    ins.PathImage = resultadoRuta + ins.ParametroInspeccionId.ToString() + ".jpg";
                    if (await _HelpImage.SaveImageFromUrlAndResize(ins.UrlImage, ins.PathImage, 250, 220) == false)
                    {
                        ins.PathImage = "";
                    }
                }

                if (PIEZA != null)
                {
                    string urlImagenDefecto = UrlImageGordon + PIEZA.IdCompania + "/GraficoExterno/" + (PIEZA.IdCompania == 1006 ? PIEZA.GetPlantNameOrigen() : "") + "/" + PIEZA.CodigoImagenTecnica + ".jpg";
                    PIEZA.ImagenFt = resultadoRuta + PIEZA.CodigoImagenTecnica + ".jpg";
                    if (await _HelpImage.SaveImageFromUrlAndResize(urlImagenDefecto, PIEZA.ImagenFt,380,250) == false)
                    {
                        PIEZA.ImagenFt = "";
                    }
                    string urlPlanoStandar= UrlImageGordon + PIEZA.IdCompania + "/GraficoExterno/" + PIEZA.CodigoImagenStandar + ".jpg";
                    PIEZA.IMAGEN_PLANO_STANDAR = resultadoRuta + PIEZA.CodigoImagenStandar+ ".jpg";
                    if (await _HelpImage.SaveImageFromUrl(urlPlanoStandar, PIEZA.IMAGEN_PLANO_STANDAR) == false)
                    {
                        PIEZA.IMAGEN_PLANO_STANDAR = "";
                    }
                }

                if (DEFECTOS.Count > 0)
                {
                    string urlImagenDefecto = UrlImageGordon + PIEZA.IdCompania + "/PiezaObservacion/FTSAP/" + PIEZA.OrdProceso + ".jpg";//ins.ImageByte=
                    PIEZA.DefectoImagen = resultadoRuta + PIEZA.OrdProceso + ".jpg";
                    if (await _HelpImage.SaveImageFromUrlAndResize(urlImagenDefecto, PIEZA.DefectoImagen, 380, 250) == false)
                    {
                        PIEZA.DefectoImagen = "";
                    }
                }


                /*
                foreach (InspeccionOptica ins in INSPECCIONES_OPTICAS)
                {

                    ins.UrlImage = UrlImageGordon + PIEZA.IdCompania + "/Certificado/EvaluacionOpticaSAP/" + ins.CertificadoId + "_" + ins.ParametroInspeccionId + ".jpg";//ins.ImageByte=
                    byte[] img = await _HelpImage.ConvertImageUrlToByte(ins.UrlImage);
                    if(img!=null)    
                    ins.ImageByte = _HelpImage.ResizeImage(img, 400, 400);

                }
                if (DEFECTOS.Count > 0)
                {
                    string urlImagenDefecto = UrlImageGordon + PIEZA.IdCompania + "/PiezaObservacion/FTSAP/" + PIEZA.OrdProceso + ".jpg";//ins.ImageByte=
                    byte[] img = await _HelpImage.ConvertImageUrlToByte(urlImagenDefecto);
                    if (img != null)
                    {
                        PIEZA.DefectoImagenByte = _HelpImage.ResizeImage(img, 300, 300);
                    }
                }
                */

               

                // Enviar el archivo al cliente
                //return File(fileBytes, "application/pdf", "archivo.pdf");
                if(Idioma=="I")
                    pdfname = await _CertificadoIfService.CertificadoPDFSglassIngles(Idioma,certificado, PIEZA,DIMENSIONAL_RESULT,APARIENCIA_RESULT,INSPECCIONES_OPTICAS, DEFECTOS);
                else
                    pdfname = await _CertificadoIfService.CertificadoPDFSglassEspanol(Idioma, certificado, PIEZA, DIMENSIONAL_RESULT, APARIENCIA_RESULT, INSPECCIONES_OPTICAS, DEFECTOS);


                byte[] fileBytesPDF = HelpImage.GetFileContent(pdfname);

                return File(fileBytesPDF, "image/pdf", System.IO.Path.GetFileName(pdfname)); 

                //var filepath = Path.Combine(environment.WebRootPath, "images", "Image1.png");
                //return File(System.IO.File.ReadAllBytes(pdfname), "image/pdf", System.IO.Path.GetFileName(pdfname));

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

           

            return Ok(response);
        }


        [HttpGet("GenerarExcel")]
        public async Task<IActionResult> GenerarExcel(long CertificadoId, string Formato="00")
        {
             //string plantillaRuta = "Reports/Templates/template1.xlsx";
            string plantillaRuta = "Reports/Templates/"+Formato+".xlsx";
            string resultadoRuta = "Reports/Results/";
            HelpImage _HelpImage = new HelpImage();
            Response response = new Response();
            try
            {
                
                _HelpImage.EmptyFolder(new DirectoryInfo(resultadoRuta));

                resultadoRuta = resultadoRuta + CertificadoId.ToString()+ "/";
                // If directory does not exist, create it
                if (!Directory.Exists(resultadoRuta))
                {
                    Directory.CreateDirectory(resultadoRuta);
                }

                
                string UrlImageGordon = "http://20.197.228.211:8081/Userimage/";
                CertificadoIf CERTIFICADO = _CertificadoIfService.GetById(CertificadoId);
                PiezaSap PIEZA = _CertificadoIfService.GetPiezaByOrden(CERTIFICADO.IdCompania, CERTIFICADO.OrdProceso);
                List<CertificadoIfdimension> DIMENSIONAL_RESULT = await _CertificadoIfService.GetMedicionesDimensionales(CertificadoId);
                List<CertificadoIfapariencias> APARIENCIA_RESULT = await _CertificadoIfService.GetDatosApariencia(CertificadoId);
                List<InspeccionOptica> INSPECCIONES_OPTICAS = await _CertificadoIfService.GetInspeccionesOpticas(CertificadoId);
                List<PiezaConcesion> DEFECTOS = _CertificadoIfService.GetImagenTecnicaObservaciones(CertificadoId);

                foreach (InspeccionOptica ins in INSPECCIONES_OPTICAS)
                {
                    ins.UrlImage = UrlImageGordon + PIEZA.IdCompania + "/Certificado/EvaluacionOpticaSAP/" + ins.CertificadoId +"/"+ ins.CertificadoId + "_" + ins.ParametroInspeccionId + ".jpg";//ins.ImageByte=

                    ins.PathImage= resultadoRuta + ins.ParametroInspeccionId.ToString() + ".jpg";
                    if (await _HelpImage.SaveImageFromUrl(ins.UrlImage, ins.PathImage)==false)
                    {
                        ins.PathImage = "";
                    }
                }

                if (PIEZA!=null)
                {
                    string urlImagenDefecto = UrlImageGordon + PIEZA.IdCompania + "/GraficoExterno/"+(PIEZA.IdCompania == 1006 ?  PIEZA.GetPlantNameOrigen() : "") + "/" + PIEZA.CodigoImagenTecnica + ".jpg";
                    PIEZA.ImagenFt = resultadoRuta + PIEZA.CodigoImagenTecnica + ".jpg";
                    if (await _HelpImage.SaveImageFromUrl(urlImagenDefecto, PIEZA.ImagenFt) == false)
                    {
                        PIEZA.ImagenFt = "";
                    }
                    string urlPlanoStandar = UrlImageGordon + PIEZA.IdCompania + "/GraficoExterno/" + PIEZA.CodigoImagenStandar + ".jpg";
                    PIEZA.IMAGEN_PLANO_STANDAR = resultadoRuta + PIEZA.CodigoImagenStandar + ".jpg";
                    if (await _HelpImage.SaveImageFromUrl(urlPlanoStandar, PIEZA.IMAGEN_PLANO_STANDAR) == false)
                    {
                        PIEZA.IMAGEN_PLANO_STANDAR = "";
                    }
                }

                if (DEFECTOS.Count > 0)
                {
                    string urlImagenDefecto= UrlImageGordon + PIEZA.IdCompania + "/PiezaObservacion/FTSAP/" + PIEZA.OrdProceso+ ".jpg";//ins.ImageByte=
                    PIEZA.DefectoImagen = resultadoRuta + PIEZA.OrdProceso+ ".jpg";
                    if (await _HelpImage.SaveImageFromUrl(urlImagenDefecto, PIEZA.DefectoImagen) == false)
                    {
                        PIEZA.DefectoImagen = "";
                    }
                }


                resultadoRuta = resultadoRuta + CertificadoId + ".xlsx";

                switch (Formato)
                {
                    case "00":
                        response.Success = _CertificadoIfService.CargarExcelFormato00(plantillaRuta, resultadoRuta, CERTIFICADO, PIEZA, DIMENSIONAL_RESULT, APARIENCIA_RESULT, DEFECTOS, INSPECCIONES_OPTICAS);
                    break;
                    case "01":
                        response.Success = _CertificadoIfService.CargarExcelFormato01(plantillaRuta, resultadoRuta, CERTIFICADO, PIEZA, DIMENSIONAL_RESULT, APARIENCIA_RESULT, DEFECTOS, INSPECCIONES_OPTICAS);
                    break;
                    case "02":
                        response.Success = _CertificadoIfService.CargarExcelFormato02(plantillaRuta, resultadoRuta, CERTIFICADO, PIEZA, DIMENSIONAL_RESULT, APARIENCIA_RESULT, DEFECTOS, INSPECCIONES_OPTICAS);
                        break;
                    case "07":
                        response.Success = _CertificadoIfService.CargarExcelFormato07(plantillaRuta, resultadoRuta, CERTIFICADO, PIEZA, DIMENSIONAL_RESULT, APARIENCIA_RESULT, DEFECTOS, INSPECCIONES_OPTICAS);
                        break;
                    case "08":
                        response.Success = _CertificadoIfService.CargarExcelFormato08(plantillaRuta, resultadoRuta, CERTIFICADO, PIEZA, DIMENSIONAL_RESULT, APARIENCIA_RESULT, DEFECTOS, INSPECCIONES_OPTICAS);
                        break;
                    case "11":
                        response.Success = _CertificadoIfService.CargarExcelFormato11(plantillaRuta, resultadoRuta, CERTIFICADO, PIEZA, DIMENSIONAL_RESULT, APARIENCIA_RESULT, DEFECTOS, INSPECCIONES_OPTICAS);
                        break;
                    case "12":
                        response.Success = _CertificadoIfService.CargarExcelFormato12(plantillaRuta, resultadoRuta, CERTIFICADO, PIEZA, DIMENSIONAL_RESULT, APARIENCIA_RESULT, DEFECTOS, INSPECCIONES_OPTICAS);
                        break;
                    case "30":
                        response.Success = _CertificadoIfService.CargarExcelFormato30(plantillaRuta, resultadoRuta, CERTIFICADO, PIEZA, DIMENSIONAL_RESULT, APARIENCIA_RESULT, DEFECTOS, INSPECCIONES_OPTICAS);
                        break;
                    case "JSS":
                        response.Success = _CertificadoIfService.CargarExcelFormatoJSS(plantillaRuta, resultadoRuta, CERTIFICADO, PIEZA, DIMENSIONAL_RESULT, APARIENCIA_RESULT, DEFECTOS, INSPECCIONES_OPTICAS);
                        break;

                    default:
                        return StatusCode(500, "Formato No Existe !");

                }    

                //_CertificadoIfService.GenerarExcel(plantillaRuta, resultadoRuta, texto, imagenRuta);
                return File(System.IO.File.ReadAllBytes(resultadoRuta), "application/octet-stream", System.IO.Path.GetFileName(resultadoRuta));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }



            return Ok(response);
        }


        [HttpPost("GenerarMultiplesPDF")]
        public async Task<IActionResult> GenerarMultiplesPDF( [FromBody] GenerateMultiplePDFRequest request )
        {
            try
            {
        //        HelpImage _HelpImage = new HelpImage();
                /*
            List<long> CertificadoIds = new List<long>();

            CertificadoIds.Add(492137);
            CertificadoIds.Add(492137);*/

                /*
                if (request.CertificadoIds == null || !request.CertificadoIds.Any())
                {
                    return BadRequest("La lista de IDs no puede estar vacía");
                }*/


                var results = new List<MultiplePDFResponse>();
                string zipFileName = $"Certificados_{DateTime.Now:yyyyMMddHHmmss}.zip";
                string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

                // Crear directorio temporal
                Directory.CreateDirectory(tempPath);

                try
                {
                    // Procesar certificados en paralelo con un límite de concurrencia
                    var options = new ParallelOptions
                    {
                        MaxDegreeOfParallelism = 5 // Ajusta según necesidades
                    };

                    await Parallel.ForEachAsync(request.CertificadoIds, options, async (certificadoId, token) =>
                    {
                        var response = new MultiplePDFResponse
                        {
                            CertificadoId = certificadoId,
                            Success = false
                        };

                        try
                        {
                            string resultadoRuta = "Reports/Results/";
                            string UrlImageGordon = "http://20.197.228.211:8081/Userimage/";

                            // Obtener datos del certificado
                            CertificadoIf certificado = _CertificadoIfService.GetById(certificadoId);
                            PiezaSap PIEZA = _CertificadoIfService.GetPiezaByOrden(certificado.IdCompania, certificado.OrdProceso);
                            var DIMENSIONAL_RESULT = await _CertificadoIfService.GetMedicionesDimensionales(certificadoId);
                            var APARIENCIA_RESULT = await _CertificadoIfService.GetDatosApariencia(certificadoId);
                            var INSPECCIONES_OPTICAS = await _CertificadoIfService.GetInspeccionesOpticas(certificadoId);
                            var DEFECTOS = _CertificadoIfService.GetImagenTecnicaObservaciones(certificadoId);

                            // Procesar apariencias
                                foreach (var apariencia in APARIENCIA_RESULT)
                                {
                                    if (apariencia.ParametroInspeccionId == 458 || apariencia.ParametroInspeccionId == 502)
                                    {
                                        if (!string.IsNullOrEmpty(apariencia.Valor))
                                        {
                                            var clasificador = _ClasificadorService.GetClasificadorById(Convert.ToInt32(apariencia.Valor));
                                            apariencia.Valor = clasificador?.Nombre ?? "";
                                        }
                                    }
                                }
                          
                            // Crear directorio específico para este certificado
                            string certificadoPath = Path.Combine(tempPath, certificadoId.ToString());
                            Directory.CreateDirectory(certificadoPath);

                            // Procesar imágenes
                            foreach (var inspeccion in INSPECCIONES_OPTICAS)
                            {
                                inspeccion.UrlImage = $"{UrlImageGordon}{PIEZA.IdCompania}/Certificado/EvaluacionOpticaSAP/{inspeccion.CertificadoId}/{inspeccion.CertificadoId}_{inspeccion.ParametroInspeccionId}.jpg";
                                inspeccion.PathImage = Path.Combine(certificadoPath, $"{inspeccion.ParametroInspeccionId}.jpg");

                                if (!await _HelpImage.SaveImageFromUrlAndResize(inspeccion.UrlImage, inspeccion.PathImage, 250, 220))
                                {
                                    inspeccion.PathImage = "";
                                }
                            }

                            // Procesar imagen de la pieza
                            if (PIEZA != null)
                            {
                                await ProcessPiezaImages(PIEZA, certificadoPath, UrlImageGordon);
                            }

                            // Procesar defectos
                            if (DEFECTOS.Any())
                            {
                                await ProcessDefectosImages(PIEZA, certificadoPath, UrlImageGordon);
                            }

                            // Generar PDF
                            string pdfPath = request.Idioma == "I"
                                ? await _CertificadoIfService.CertificadoPDFSglassIngles(request.Idioma, certificado, PIEZA, DIMENSIONAL_RESULT, APARIENCIA_RESULT, INSPECCIONES_OPTICAS, DEFECTOS)
                                : await _CertificadoIfService.CertificadoPDFSglassEspanol(request.Idioma, certificado, PIEZA, DIMENSIONAL_RESULT, APARIENCIA_RESULT, INSPECCIONES_OPTICAS, DEFECTOS);

                            // Copiar PDF al directorio temporal
                            string destinationPdfPath = Path.Combine(tempPath, $"{PIEZA.LoteLogistico} - {PIEZA.CertificadoId}.pdf");
                            await CopyFileAsync(pdfPath, destinationPdfPath);
//                            File.Copy(pdfPath, destinationPdfPath, true);

                            response.Success = true;
                            response.FileName = Path.GetFileName(destinationPdfPath);
                        }
                        catch (Exception ex)
                        {
                            response.Success = false;
                            response.Error = ex.Message;
                        }
                        finally
                        {
                            results.Add(response);
                        }
                    });

                    // Crear archivo ZIP
                    string zipPath = Path.Combine(Path.GetTempPath(), zipFileName);
                    ZipFile.CreateFromDirectory(tempPath, zipPath);

                    // Leer el archivo ZIP
                    //byte[] zipBytes = await File.ReadAllBytesAsync(zipPath);
                 
                    // Por esta implementación:
                    byte[] zipBytes;
                    using (FileStream fs = new FileStream(zipPath, FileMode.Open, FileAccess.Read))
                    {
                        zipBytes = new byte[fs.Length];
                        await fs.ReadAsync(zipBytes, 0, (int)fs.Length);
                    }
                    //File.ReadAllBytes(zipPath);



                    // Limpiar archivos temporales
                    /*
                    Directory.Delete(tempPath, true);
                    File.Delete(zipPath);
                    Directory.Delete(tempPath, true);
                    await Task.Run(() => File.Delete(zipPath));
                    */

                    // Devolver el ZIP
                    return File(zipBytes, "application/zip", zipFileName);
                }
                finally
                {
                    // Asegurar limpieza en caso de error
                    if (Directory.Exists(tempPath))
                    {
                        Directory.Delete(tempPath, true);
                    }
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Error al generar múltiples PDFs");
                return StatusCode(500, "Error interno del servidor");
            }
        }

        // Método auxiliar para procesar imágenes de la pieza
        private async Task ProcessPiezaImages(PiezaSap pieza, string certificadoPath, string urlImageGordon)
        {
            string urlImagenDefecto = $"{urlImageGordon}{pieza.IdCompania}/GraficoExterno/{(pieza.IdCompania == 1006 ? pieza.GetPlantNameOrigen() : "")}/{pieza.CodigoImagenTecnica}.jpg";
            pieza.ImagenFt = Path.Combine(certificadoPath, $"{pieza.CodigoImagenTecnica}.jpg");

            if (!await _HelpImage.SaveImageFromUrlAndResize(urlImagenDefecto, pieza.ImagenFt, 380, 250))
            {
                pieza.ImagenFt = "";
            }

            string urlPlanoStandar = $"{urlImageGordon}{pieza.IdCompania}/GraficoExterno/{pieza.CodigoImagenStandar}.jpg";
            pieza.IMAGEN_PLANO_STANDAR = Path.Combine(certificadoPath, $"{pieza.CodigoImagenStandar}.jpg");

            if (!await _HelpImage.SaveImageFromUrl(urlPlanoStandar, pieza.IMAGEN_PLANO_STANDAR))
            {
                pieza.IMAGEN_PLANO_STANDAR = "";
            }
        }

        // Método auxiliar para procesar imágenes de defectos
        private async Task ProcessDefectosImages(PiezaSap pieza, string certificadoPath, string urlImageGordon)
        {
            string urlImagenDefecto = $"{urlImageGordon}{pieza.IdCompania}/PiezaObservacion/FTSAP/{pieza.OrdProceso}.jpg";
            pieza.DefectoImagen = Path.Combine(certificadoPath, $"{pieza.OrdProceso}.jpg");

            if (!await _HelpImage.SaveImageFromUrlAndResize(urlImagenDefecto, pieza.DefectoImagen, 380, 250))
            {
                pieza.DefectoImagen = "";
            }
        }

        public static async Task CopyFileAsync(string sourceFile, string destinationFile)
        {
            await using var sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read);
            await using var destinationStream = new FileStream(destinationFile, FileMode.Create, FileAccess.Write);
            await sourceStream.CopyToAsync(destinationStream);
        }


    }

    public static class FileExtensions
    {
        public static async Task CopyFileAsync(string sourceFile, string destinationFile)
        {
            await using var sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read);
            await using var destinationStream = new FileStream(destinationFile, FileMode.Create, FileAccess.Write);
            await sourceStream.CopyToAsync(destinationStream);
        }
    }





    public class MultiplePDFResponse
    {
        public string FileName { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
        public long CertificadoId { get; set; }
    }

    public class GenerateMultiplePDFRequest
    {
        public List<long> CertificadoIds { get; set; }
        public string Idioma { get; set; } = "I";
    }

}


