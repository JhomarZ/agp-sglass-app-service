using AGP.Gordon.CommonLayer;
using AGP.Snowden.DataAccessLayer;
using AGP.Snowden.DataAccessLayer.SAP;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuestPDF;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;
using QRCoder;
using OfficeOpenXml.ConditionalFormatting.Contracts;
using System.Reflection.Metadata;
using OfficeOpenXml;

namespace AGP.Snowden.ServiceLayer.Warehouse
{
    public class TrackingImputadosService
    {
        public async Task<List<VwimputadosSap>> GetAll(int Skip=0, int Take=30, string? Descripcion = "", string? FechaInicio = "", string? Fechafin = "", string? Centro = "", string Status="" )
        {
            List<VwimputadosSap> lista = new List<VwimputadosSap>();
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    var Query = db.VwimputadoSap.Where(x => x.NumeroMaterial != "" && x.NumeroMaterial != null).AsQueryable();

                    //Query = Query.Where(d => d.DocumentoCompra.Substring(0, 3) != "457" && d.DocumentoCompra.Substring(0, 3) != "458");

                    if (Centro != null && Centro != "")
                    {
                        Query = Query.Where(d => d.Centro.Contains(Centro));
                    }
                    else
                    {
                        Query = Query.Where(d => d.Centro.Contains("PE"));
                    }

                    if (Status != null && Status != "")
                    {
                        Query = Query.Where(d => d.StatusCode == Status);
                    }


                    if (Descripcion != null && Descripcion != "")
                    {
                        Query = Query.Where(d => (d.DocumentoCompra + d.NroCuentaProveedor + d.NroDocumentoComercial + d.NumeroMaterial + d.Solicitante).Contains(Descripcion));
                    }

                    if (FechaInicio != null && FechaInicio != "" && Fechafin != null && Fechafin != "")
                    {
                        DateTime fechaInicio = Convert.ToDateTime(FechaInicio);
                        DateTime fechaFin  = Convert.ToDateTime(Fechafin);
                        Query = Query.Where(d => d.FechaCreacion >= fechaInicio && d.FechaCreacion <= fechaFin);
                    }

                    lista = await Query.OrderByDescending(x => x.FechaCreacion).ThenBy(x => x.HoraRegistrada).Skip(Skip).Take(Take).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException("Error Lista Imputado SAP", ex.Message);
            }

            return lista;
        }

        public async Task<List<VwimputadosSap>> GetImputadoSapByDocumentoCompra(string DocumentoCompra)
        {
            List<VwimputadosSap> Imputado = new List<VwimputadosSap>();
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    Imputado = await db.VwimputadoSap.Where(x => x.DocumentoCompra == DocumentoCompra).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException("Error Imputado SAP", ex.Message);
            }
            return Imputado;
        }
        public async Task<VwimputadosSap> GetTrackinImputadoSapByKey(string Centro, string DocumentoCompra, string Posicion, string MBLNR)
        {
            MBLNR = (MBLNR== "undefined") ? "":MBLNR;
            VwimputadosSap Imputado = new VwimputadosSap();
            try {
                using (var db = new DbSnowdenContext())
                {
                    Imputado = await db.VwimputadoSap.Where(x => x.Centro == Centro && x.DocumentoCompra == DocumentoCompra && x.NroPosicionDc == Posicion && x.Mblnr == MBLNR).FirstAsync();
                }
            }
            catch(Exception ex)
            {
                throw new System.ArgumentException("Error Imputado SAP", ex.Message);
            }
            return Imputado;
        }

        public async Task<TrackingImputadosExtension> GetTrackinImputadoExtensionByKey(string Centro, string DocumentoCompra, string Posicion, string MBLNR)
        {
            MBLNR =(MBLNR == "undefined") ? "" : MBLNR;
            TrackingImputadosExtension Imputado = new TrackingImputadosExtension();
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    Imputado = await db.TrackingImputadosExtensions.Where(x => x.CentroSap == Centro && x.DocumentoCompra == DocumentoCompra && x.NroPosicion == Posicion && x.MBLNR == MBLNR).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException("Error Imputado SAP", ex.Message);
            }
            return Imputado;
        }

        public async Task<TrackingImputadosExtension> Add(TrackingImputadosExtension trackingImputadosExtension)
        {
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    db.TrackingImputadosExtensions.Add(trackingImputadosExtension);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException("Error al guardar en la tabla extension Imputado SAP", ex.Message);
            }
            return trackingImputadosExtension;
           
        }

        public async Task<TrackingImputadosExtension> Update(TrackingImputadosExtension trackingImputadosExtension)
        {
            List<PP_TRACKNG_IMPUT> lista = new List<PP_TRACKNG_IMPUT>();
            using (var db = new DbSnowdenContext())
            {
                db.TrackingImputadosExtensions.Update(trackingImputadosExtension);
                db.SaveChanges();
            }

            return trackingImputadosExtension;
        }

        public byte[] GenerarCodigoQR(string contenido)
        {
            var generadorQR = new QRCodeGenerator();
            var codigoQR = generadorQR.CreateQrCode(contenido, QRCodeGenerator.ECCLevel.Q);

            var mapaBits = new BitmapByteQRCode(codigoQR);
            return mapaBits.GetGraphic(20); // El número 20 es el tamaño del píxel para el código QR
        }

        public string GenerarEtiquetaZebraPDF(byte[] QR, int Bultos, int Copias, string? DocumentoCompra, string? Usuario, string? Area)
        {
            int bultos = Bultos;
            int copies = Copias;
            int totalPages = bultos * copies;

            string fileName = "Reports/Results/label.pdf";

            try
            {
              var  document = QuestPDF.Fluent.Document.Create(container =>
                {
                    // page 1 content  size 15 x 10 cm
                    for (int b = 1; b <= bultos; b++)
                    {
                        for (int c = 0; c < copies; c++)
                        {
                            container.Page(page =>
                            {
                                page.Margin(1, Unit.Centimetre);
                                page.PageColor(Colors.White);
                                page.Size(15, 10, Unit.Centimetre);


                                page.Header().Border(0).Row(row =>
                                {
                                    //FileStream fs = GetFileStrem("src/logo.jpg");
                                    FileStream fs = File.Open("src/logo.jpg", FileMode.Open);

                                    row.ConstantItem(3, Unit.Centimetre).Background(Colors.White).Border(0).AlignMiddle()
                                    .Image(fs); //.Image("logo.jpg");


                                    row.ConstantItem(10, Unit.Centimetre).Background(Colors.White).Border(0).AlignMiddle().PaddingLeft(5, Unit.Millimetre).Text(
                                        text =>
                                        {
                                            text.Span("PO: " + DocumentoCompra).FontFamily(Fonts.Arial).FontSize(17).FontColor(Colors.Black).Bold();
                                        });

                                    fs.Dispose();
                                    fs.Close();
                                });



                                page.Content().Border(0).PaddingTop(3, Unit.Millimetre).PaddingVertical(0, Unit.Millimetre)
                               .Column(column =>
                               {

                                   column.Item().AlignCenter().Row(row =>
                                   {
                                       row.ConstantItem(13, Unit.Centimetre).Background(Colors.White).Border(0).Table(table =>
                                       {


                                           table.ColumnsDefinition(columns =>
                                           {
                                               columns.ConstantColumn(6, Unit.Centimetre);
                                               columns.ConstantColumn(6, Unit.Centimetre);

                                           });

                                           table.Cell().ColumnSpan(2).PaddingTop(0, Unit.Millimetre).AlignCenter().Text(Usuario).FontFamily(Fonts.Arial).FontSize(17).Bold();
                                           table.Cell().ColumnSpan(2).PaddingTop(0, Unit.Millimetre).AlignCenter().Text(Area).FontFamily(Fonts.Arial).FontSize(17).Bold();

                                           FileStream fs = File.Open("src/logo.jpg", FileMode.Open);
                                           table.Cell().MaxWidth(5, Unit.Centimetre).Background(Colors.White).Border(0).AlignMiddle().Image(QR).FitWidth();
                                           fs.Dispose();
                                           fs.Close();
                                           table.Cell().PaddingTop(0, Unit.Millimetre).PaddingLeft(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text("BULTOS " + b.ToString() + " DE " + bultos.ToString()).FontFamily(Fonts.Arial).FontSize(18).Bold();

                                       });
                                   });
                               });
                            });
                        }

                    }
                });
                document.GeneratePdf(fileName);
            }
            catch(Exception ex)
            {
                throw new System.ArgumentException("Error PDF", ex.Message);
            }
            // instead of the standard way of generating a PDF file
            
            return fileName;
        }


        public string GenerarEtiquetaMasivoZebraPDF( int Bultos, int Copias, List<VwimputadosSap> Imputados)
        {
            int bultos = Bultos;
            int copies = Copias;
            int totalPages = bultos * copies;

            string fileName = "Reports/Results/label.pdf";

            try
            {
                var document = QuestPDF.Fluent.Document.Create(container =>
                {
                  //  foreach (var item in Imputados)
                  //  {
                        // page 1 content  size 15 x 10 cm
                        for (int b = 1; b <= bultos; b++)
                        {
                            for (int c = 0; c < copies; c++)
                            {
                                container.Page(page =>
                                {
                                    page.Margin(1, Unit.Centimetre);
                                    page.PageColor(Colors.White);
                                    page.Size(15, 10, Unit.Centimetre);


                                    page.Header().Border(0).Row(row =>
                                    {
                                        //FileStream fs = GetFileStrem("src/logo.jpg");
                                        FileStream fs = File.Open("src/logo.jpg", FileMode.Open);

                                        row.ConstantItem(3, Unit.Centimetre).Background(Colors.White).Border(0).AlignMiddle()
                                        .Image(fs); //.Image("logo.jpg");


                                        row.ConstantItem(10, Unit.Centimetre).Background(Colors.White).Border(0).AlignMiddle().PaddingLeft(5, Unit.Millimetre).Text(
                                            text =>
                                            {
                                                text.Span("PO: " + Imputados[0].DocumentoCompra).FontFamily(Fonts.Arial).FontSize(17).FontColor(Colors.Black).Bold();
                                            });

                                        fs.Dispose();
                                        fs.Close();
                                    });



                                    page.Content().Border(0).PaddingTop(3, Unit.Millimetre).PaddingVertical(0, Unit.Millimetre)
                                   .Column(column =>
                                   {

                                       column.Item().AlignCenter().Row(row =>
                                       {
                                           row.ConstantItem(13, Unit.Centimetre).Border(0).Background(Colors.White).Border(0).Table(table =>
                                           {


                                               table.ColumnsDefinition(columns =>
                                               {
                                                   columns.ConstantColumn(6, Unit.Centimetre);
                                                   columns.ConstantColumn(7, Unit.Centimetre);

                                               });

                                               table.Cell().ColumnSpan(2).PaddingTop(0, Unit.Millimetre).AlignCenter().Text(Imputados[0].Responsable).FontFamily(Fonts.Arial).FontSize(17).Bold();
                                               //table.Cell().ColumnSpan(2).PaddingTop(0, Unit.Millimetre).AlignCenter().Text(Imputados[0].Centro + "_" + Imputados[0].SolicitanteNombre).FontFamily(Fonts.Arial).FontSize(17).Bold().wrap;
                                               table.Cell().ColumnSpan(2)
                                                            //.Width(200) // Define un ancho máximo para la celda
                                                            .PaddingTop(0, Unit.Millimetre)
                                                            .AlignCenter()
                                                            .Text(text =>
                                                            {
                                                                text.Span(TruncarTexto(Imputados[0].Centro + "_" + Imputados[0].SolicitanteNombre, 28)) // Truncar texto manualmente
                                                                    .FontFamily(Fonts.Arial)
                                                                    .FontSize(17)
                                                                    .Bold();
                                                            });

                                               FileStream fs = File.Open("src/logo.jpg", FileMode.Open);
                                               table.Cell().MaxWidth(5, Unit.Centimetre).Background(Colors.White).Border(0).AlignMiddle().Image(Imputados[0].QR).FitWidth();
                                               fs.Dispose();
                                               fs.Close();

                                               // table.Cell().PaddingTop(0, Unit.Millimetre).PaddingLeft(2, Unit.Millimetre).AlignCenter().AlignMiddle().Text("BULTOS " + b.ToString() + " DE " + bultos.ToString()).FontFamily(Fonts.Arial).FontSize(18).Bold();
                                               table.Cell().Border(0).PaddingTop(0, Unit.Millimetre).PaddingLeft(0, Unit.Millimetre).AlignCenter().AlignMiddle().Text(
                                                    text =>
                                                    {
                                                        for (int i = 0; i < Imputados.Count; i++)
                                                        {
                                                            text.Line("Cnt:"+Imputados[i].CntPedido +" Pos: "+Imputados[i].NroPosicionDc +" / "+ Imputados[i].NumeroMaterial +" "+((Imputados[i].DescripcionMaterial.Length>28)?Imputados[i].DescripcionMaterial.Substring(0,28): Imputados[i].DescripcionMaterial) ).FontFamily(Fonts.Arial).FontSize(6).FontColor(Colors.Black).LineHeight(1);
                                                        }
                                                        text.Span("BULTOS " + b.ToString() + " DE " + bultos.ToString()).FontFamily(Fonts.Arial).FontSize(17).FontColor(Colors.Black).Bold();
                                                    });

                                           });
                                       });
                                   });
                                });
                            }

                        }
                   // }

                   
                });
                document.GeneratePdf(fileName);
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException("Error PDF", ex.Message);
            }
            // instead of the standard way of generating a PDF file

            return fileName;
        }
        
        // Método para truncar el texto manualmente
        string TruncarTexto(string texto, int maxLength)
        {
            if (texto.Length > maxLength)
                return texto.Substring(0, maxLength) + "...";
            return texto;
        }
    }
}