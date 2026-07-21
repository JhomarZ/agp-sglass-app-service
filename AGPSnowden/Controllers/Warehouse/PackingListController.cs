using AGP.Gordon.CommonLayer;
using AGP.Snowden.DataAccessLayer;
using AGP.Snowden.DataAccessLayer.Azure;
using AGP.Snowden.DataAccessLayer.SAP;
using AGP.Snowden.ServiceLayer.Azure;
using AGP.Snowden.ServiceLayer.Warehouse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AGPSnowden.Controllers.Warehouse
{

    [Route("api/[controller]")]
    [ApiController]
    public class PackingListController : ControllerBase
    {
        private readonly TrackingImputadosService _TrackingImputadosService;
        private readonly ShippingStatusService _ShippingStatusService;
        private readonly PackingListService _PackingListService;
        private readonly ImageService _imageService;


        public PackingListController(ImageService imageService)
        {
            _TrackingImputadosService = new TrackingImputadosService();
            _ShippingStatusService = new ShippingStatusService();
            _PackingListService = new PackingListService();
            _imageService = imageService;
        }


        [HttpGet("List")]
        public async Task<List<PackingList>> List(int Skip, int Take, string? Observation, string? FechaInicio, string? Fechafin, string? Status)
        {
            List<PackingList> response = new List<PackingList>();

            
            response = await _PackingListService.GetAll(Skip, Take, Observation, FechaInicio, Fechafin, Status);
            for (int i = 0; i < response.Count; i++)
            {
                switch (response[i].Status)
                {
                    case "GEN":
                        response[i].StatusDescription = "GENERADO";
                        break;
                    case "DES":
                        response[i].StatusDescription = "DESPACHADO";
                        break;
                    case "REC":
                        response[i].StatusDescription = "RECEPCIONADO";
                        break;
                    case "ENT":
                        response[i].StatusDescription = "ENTREGADO";
                        break;
                }
                //response[i].NumeroMaterial = (response[i].NumeroMaterial.Length>9)?response[i].NumeroMaterial.Substring(9): response[i].NumeroMaterial;
                //response[i].Extension = await _TrackingImputadosService.GetTrackinImputadoExtensionByKey(response[i].Centro, response[i].DocumentoCompra, response[i].NroPosicionDC, response[i].MBLNR);
            }
            /*
                    for (int i = 0; i < response.Count; i++)
                    {
                     /   response[i].Extension = await _PackingListService.GetTrackinImputadoExtensionByKey(response[i].Centro, response[i].DocumentoCompra, response[i].NroPosicionDC, response[i].MBLNR);
                    }
            */

            return response;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            PackingList packingList = new PackingList();
            try
            {
                
                packingList = await _PackingListService.GetOne(id);

                if (packingList == null) { throw new System.ArgumentException("Packing List No exist."); }
                    List<PackingListItem> Imputados = (List<PackingListItem>)packingList.Imputados;

                packingList.StatusHistory = await _PackingListService.GetHistoryStatus(id);
                packingList.Imputados = await _PackingListService.GetAllPackingListItems(id);

          
                

                /*
                for (int i = 0; i < packingList.Imputados.Count; i++)
                {
                    packingList.Imputados[i].= await _TrackingImputadosService.GetTrackinImputadoSapByKey(packingList.Imputados[i].CentroSap, packingList.Imputados[i].DocumentoCompra, packingList.Imputados[i].NroPosicion, packingList.Imputados[i].Mblnr);
                }*/
                foreach (PackingListItem item in packingList.Imputados)
                {
                    item.TrackingImputadosSap = await _TrackingImputadosService.GetTrackinImputadoSapByKey(item.CentroSap,item.DocumentoCompra,item.NroPosicion,item.Mblnr);
                    item.ShippingStatus = await _ShippingStatusService.GetOne(item.ShippingStatusId);
                }
               
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(packingList);
        }

        [HttpPost]
        public async Task<IActionResult> Add(PackingList packingList)
        {
            try
            {
                // Obtener la fecha y hora actual en UTC
                var utcNow = DateTimeOffset.UtcNow;

                // Convertir a la zona horaria de Perú
                var peruTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
                var peruNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow.DateTime, peruTimeZone);


                packingList.CreatedAt = peruNow;
                packingList.UpdatedAt = peruNow;
                
                List<PackingListItem> Imputados = (List<PackingListItem>)packingList.Imputados;

                for (int i = 0; i < Imputados.Count; i++)
                {
                    PackingListItem item = await _PackingListService.GetOnePackingItemByKey(Imputados[i].CentroSap, Imputados[i].DocumentoCompra, Imputados[i].NroPosicion, Imputados[i].Mblnr);
                    if (item != null)
                    {
                        throw new System.ArgumentException("Imputado "+ Imputados[i].CentroSap+"-"+Imputados[i].DocumentoCompra + "-" + Imputados[i].NroPosicion + "-" + Imputados[i].Mblnr + "  ya exite en otro packing list", "");
                    }
                }

                packingList = await _PackingListService.Add(packingList);

              

                for (int i = 0; i < Imputados.Count; i++)
                {
                    //PackingListItem item = Imputados[i];
                    Imputados[i].PackingListId = packingList.Id;
                    Imputados[i].ShippingStatusId = 2;
                    Imputados[i] = await _PackingListService.AddPackingItem(Imputados[i]);
                    ImputadoStatusHistory imputadoStatusHistory = new ImputadoStatusHistory();
                    imputadoStatusHistory.StatusId = Imputados[i].ShippingStatusId;
                    imputadoStatusHistory.PlantSap = Imputados[i].CentroSap;
                    imputadoStatusHistory.PurchaseOrder = Imputados[i].DocumentoCompra;
                    imputadoStatusHistory.NroPosition = Imputados[i].NroPosicion;
                    imputadoStatusHistory.MBLNR = Imputados[i].Mblnr;
                    imputadoStatusHistory.CreatedAt= peruNow;
                    imputadoStatusHistory.CreatedBy = Imputados[i].CreatedBy;
                    imputadoStatusHistory.PackingListId = packingList.Id;
                    imputadoStatusHistory = await _PackingListService.AddImputadoStatusHistory(imputadoStatusHistory);

                    //TrackingImputadosExtension trackingImputadosExtension = new TrackingImputadosExtension();
                    TrackingImputadosExtension trackingImputadosExtension = await _TrackingImputadosService.GetTrackinImputadoExtensionByKey(Imputados[i].CentroSap, Imputados[i].DocumentoCompra, Imputados[i].NroPosicion, Imputados[i].Mblnr);
                    if(trackingImputadosExtension == null)
                    {
                        trackingImputadosExtension = new TrackingImputadosExtension();
                        trackingImputadosExtension.CentroSap = Imputados[i].CentroSap;
                        trackingImputadosExtension.NroPosicion = Imputados[i].NroPosicion;
                        trackingImputadosExtension.DocumentoCompra = Imputados[i].DocumentoCompra;
                        trackingImputadosExtension.MBLNR = Imputados[i].Mblnr;
                        trackingImputadosExtension.Bultos = 0;
                        trackingImputadosExtension.CreatedBy = Imputados[i].CreatedBy;
                        trackingImputadosExtension.CreatedAt = peruNow;
                        trackingImputadosExtension.UpdatedAt = peruNow;
                        trackingImputadosExtension.PackingListId= packingList.Id;
                        trackingImputadosExtension = await _TrackingImputadosService.Add(trackingImputadosExtension);
                    }
                    else
                    {
                        trackingImputadosExtension.UpdatedBy = Imputados[i].CreatedBy;
                        trackingImputadosExtension.PackingListId = packingList.Id;
                        trackingImputadosExtension.UpdatedAt = peruNow;
                        trackingImputadosExtension = await _TrackingImputadosService.Update(trackingImputadosExtension);
                    }

                }
                packingList.Imputados = Imputados;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(packingList);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id,PackingList input)
        {
            try
            {
                // Obtener la fecha y hora actual en UTC
                var utcNow = DateTimeOffset.UtcNow;

                // Convertir a la zona horaria de Perú
                var peruTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
                var peruNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow.DateTime, peruTimeZone);

                PackingList currentPackingList = new PackingList();

                currentPackingList = await _PackingListService.GetOne(id);

                if (currentPackingList == null) { throw new System.ArgumentException("Packing List no existe !! ");  }

                if(input.Status!= currentPackingList.Status)
                {
                    switch (input.Status)
                    {
                        case "REC":

                            foreach (PackingListItem item in input.Imputados)
                            {


                                // add history
                                ImputadoStatusHistory imputadoStatusHistory = new ImputadoStatusHistory();
                                imputadoStatusHistory.PlantSap = item.CentroSap;
                                imputadoStatusHistory.PurchaseOrder = item.DocumentoCompra;
                                imputadoStatusHistory.NroPosition = item.NroPosicion;
                                imputadoStatusHistory.MBLNR = item.Mblnr;
                                imputadoStatusHistory.StatusId = 3; //3 Entregado en planta
                                imputadoStatusHistory.PackingListId = id;
                                imputadoStatusHistory.CreatedAt = peruNow;
                                imputadoStatusHistory.CreatedBy = input.UpdatedBy;
                                await _PackingListService.AddImputadoStatusHistory(imputadoStatusHistory);

                                item.ShippingStatusId = 3;
                                item.UpdatedBy = input.UpdatedBy;
                                item.UpdatedAt = peruNow;
                                await _PackingListService.UpdatePackingItem(item);
                            }
                            break; 
                    }
                }

                input.UpdatedAt = peruNow;
                input = await _PackingListService.Update(id,input);

                


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
                PackingList packing = new PackingList();
                packing = await _PackingListService.GetOne(id);
                packing = await _PackingListService.DeletePackingList(packing);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok();
        }

        [HttpGet("GenerarEtiqueta")]
        public async Task<IActionResult> GenerarEtiqueta(int packingId)
        {
            HelpImage _HelpImage = new HelpImage();

            string response = "resultado";

            try
            {
                PackingList packing = await _PackingListService.GetOne(packingId);

                if (packing == null) { throw new System.ArgumentException("No se encontro el packing en la BD "); }

                packing.Imputados= await _PackingListService.GetAllPackingListItems(packingId);

                foreach (var item in packing.Imputados)
                {
                   item.TrackingImputadosSap = await _TrackingImputadosService.GetTrackinImputadoSapByKey(item.CentroSap, item.DocumentoCompra, item.NroPosicion, item.Mblnr);
                }

                //byte[] QR = _TrackingImputadosService.GenerarCodigoQR(DocumentoCompra+"-"+ Usuario+"-"+Area);
                string UrlQr = "https://snowdenappqas.azurewebsites.net/packing-list-detail/"+ packingId.ToString();
                byte[] QR = _TrackingImputadosService.GenerarCodigoQR(UrlQr);

                // QR = _HelpImage.ResizeImage(QR, 50, 50);
                string pdfname = _PackingListService.GenerarEtiquetaZebraPDF(QR, packing,1);

                byte[] fileBytesPDF = HelpImage.GetFileContent(pdfname);
               

                return File(fileBytesPDF, "image/pdf", System.IO.Path.GetFileName(pdfname));

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            //return response;
        }

        /** detail packing functions */
        
        [HttpPost("ConfirmarItems")]
        public async Task<IActionResult> ConfirmItems(List<PackingListItem> items)
        {
            int ShippingStatusId = 4; // 4 ->CONFIRMADO
            try
            {
                // Obtener la fecha y hora actual en UTC
                var utcNow = DateTimeOffset.UtcNow;

                // Convertir a la zona horaria de Perú
                var peruTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
                var peruNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow.DateTime, peruTimeZone);

                foreach (PackingListItem item in items)
                {
                    string fileTmpName = item.ImageA;
                    PackingListItem currentItem = await _PackingListService.GetOnePackingListItem(item.Id);

                    string folderPacking = currentItem.PackingListId.ToString();
                    string newFileName = ""; 
                    if(fileTmpName!="")
                    {
                        newFileName=currentItem.PackingListId.ToString() + "_" + currentItem.Id.ToString() + ".jpg";
                        if (await _imageService.FileExistsAsync(fileTmpName))
                        {
                            await _imageService.CopyImgeFileAsync(fileTmpName, folderPacking + "/" + newFileName);
                        }
                        item.ImageA = newFileName;
                    }
                    

                    
                    if (currentItem.ShippingStatusId != ShippingStatusId)
                    {
                        item.ShippingStatusId = ShippingStatusId;
                        await _PackingListService.UpdatePackingItem(item);

                        // Guardar imagenes
                        


                        
                        // add history
                        ImputadoStatusHistory imputadoStatusHistory = new ImputadoStatusHistory();
                        imputadoStatusHistory.PlantSap = item.CentroSap;
                        imputadoStatusHistory.PurchaseOrder = item.DocumentoCompra;
                        imputadoStatusHistory.NroPosition = item.NroPosicion;
                        imputadoStatusHistory.MBLNR = item.Mblnr;
                        imputadoStatusHistory.StatusId = ShippingStatusId; //3 Entregado en planta
                        imputadoStatusHistory.PackingListId = item.PackingListId;
                        imputadoStatusHistory.CreatedAt = peruNow;
                        imputadoStatusHistory.CreatedBy = item.UpdatedBy;
                        await _PackingListService.AddImputadoStatusHistory(imputadoStatusHistory);
                        
                    }

                    

                }

                //input = await _PackingListService.Update(id, input);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(items);
        }

        [HttpPost("RechazarItems")]
        public async Task<IActionResult> RechazarItems(List<PackingListItem> items)
        {
            int ShippingStatusId = 5; // 5 ->RECHAZADO
            try
            {
                // Obtener la fecha y hora actual en UTC
                var utcNow = DateTimeOffset.UtcNow;

                // Convertir a la zona horaria de Perú
                var peruTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
                var peruNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow.DateTime, peruTimeZone);

                foreach (PackingListItem item in items)
                {
                    item.ShippingStatusId = ShippingStatusId;
                    await _PackingListService.UpdatePackingItem(item);

                    // add history
                    ImputadoStatusHistory imputadoStatusHistory = new ImputadoStatusHistory();
                    imputadoStatusHistory.PlantSap = item.CentroSap;
                    imputadoStatusHistory.PurchaseOrder = item.DocumentoCompra;
                    imputadoStatusHistory.NroPosition = item.NroPosicion;
                    imputadoStatusHistory.MBLNR = item.Mblnr;
                    imputadoStatusHistory.StatusId = ShippingStatusId; 
                    imputadoStatusHistory.PackingListId = item.PackingListId;
                    imputadoStatusHistory.CreatedAt = peruNow;
                    imputadoStatusHistory.CreatedBy = item.UpdatedBy;
                    await _PackingListService.AddImputadoStatusHistory(imputadoStatusHistory);
                }

                //input = await _PackingListService.Update(id, input);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Ok(items);
        }


    }
}
