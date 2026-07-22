// See https://aka.ms/new-console-template for more information

using AGP.Gordon.DataAccessLayer.SAPEXPANSION;
using AGP.Gordon.ServiceLayer;
using Azure;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using SkiaSharp;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;


async Task<byte[]> ConvertImageUrlToByte(string imageUrl)
{
    byte[] byteArray = null;
    try
    {
        WebClient client = new WebClient();

        Stream stream = await client.OpenReadTaskAsync(imageUrl);
        var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);

        byteArray = memoryStream.ToArray();
    }
    catch(Exception ex)
    {
        return null;
    }
    

    return byteArray;
}

string UrlImageGordon = "http://4.228.184.32:8081/Userimage/";

CertificadoIFService _CertificadoIFService = new CertificadoIFService();

long CertificadoId = 360915;
CertificadoIf certificado = _CertificadoIFService.GetById(359662);
PiezaSap pieza = _CertificadoIFService.GetPiezaByOrden(certificado.IdCompania, certificado.OrdProceso);

#region SEC-1 CABECERA CERTIFICADO INFO
string CERTIFICATE_NUMBER = CertificadoId.ToString();
string SUPPLIER = pieza.GetSuplier();
string SUPPLIER_ADDRESS = pieza.GetSuplierAddress();
string CLIENT = pieza.Cliente;
string PRODUCTION_ORDER = pieza.OrdProceso;
string COLOR = pieza.Color;
string VEHICLE = pieza.Vehiculo;
string COMPOSITION = pieza.Formula;
string THICKNESS = pieza.Espesor;
string AQL = "NOT APPLICABLE";
string SAMPLE_SIZE = "100%";
string PRODUCTION_LOT = pieza.LoteLogistico;
#endregion

#region SEC-2 IMAGEN TECNICA

string IMAGEN_TECNICA = UrlImageGordon + pieza.IdCompania+(pieza.IdCompania==1006?"/"+pieza.GetPlantNameOrigen():"") +"/GraficoExterno//"+pieza.CodigoImagenTecnica+".jpg";
string IMAGEN_PLANO_STANDAR = UrlImageGordon + pieza.IdCompania + "/GraficoExterno/"+ pieza.CodigoImagenStandar+".jpg";
Byte[] BYTE_IMAGEN_TECNICA = await ConvertImageUrlToByte(IMAGEN_TECNICA);
Byte[] BYTE_IMAGEN_PLANO_STANDAR = await ConvertImageUrlToByte(IMAGEN_PLANO_STANDAR);

#endregion

#region SEC-3 DATOS: DIMENSIONAL RESULT
List<CertificadoIfdimension> DIMENSIONAL_RESULT = new List<CertificadoIfdimension>();

DIMENSIONAL_RESULT = await _CertificadoIFService.GetMedicionesDimensionales(CertificadoId);
#endregion

#region SEC-4 DATOS: APARIENCIA RESULT
List<CertificadoIfapariencias> APARIENCIA_RESULT = new List<CertificadoIfapariencias>();

APARIENCIA_RESULT = await _CertificadoIFService.GetDatosApariencia(CertificadoId);
#endregion

#region SEC-5 OPTICAL INSPECTION
List<InspeccionOptica> INSPECCIONES_OPTICAS = new List<InspeccionOptica>();
    INSPECCIONES_OPTICAS = await _CertificadoIFService.GetInspeccionesOpticas(CertificadoId);

    foreach (InspeccionOptica ins in INSPECCIONES_OPTICAS)
    {
    //ins.ImageByte=
        ins.UrlImage = UrlImageGordon + pieza.IdCompania + "/Certificado/EvaluacionOpticaSAP/" + ins.CertificadoId + "_"+ins.ParametroInspeccionId + ".jpg";
        ins.ImageByte = await ConvertImageUrlToByte(ins.UrlImage);

    }
    
#endregion

#region SEC - FOOTER
string INSPECTOR = pieza.UsuarioCrea;
string QUALITY_ENGINEER = pieza.GetQualityEngineer();
string QUALITY_MANAGER = pieza.GetQualityManager(); 
#endregion

QuestPDF.Settings.License = LicenseType.Community;
Console.WriteLine("Hello, World! XXX");
// code in your main method
var document = Document.Create(container =>
{
    // page 1 content  size width 538
    container.Page(page =>
    {
        page.Margin(1, Unit.Centimetre);
        page.PageColor(Colors.White);
        page.Size(PageSizes.A4);

        #region SEC-1 REPORT CABECERA
        page.Header().Border(1).Row(row =>
        {
            //FileStream fs = _TestPDFService.GetFileStrem("src/logo.jpg");
            FileStream fs = File.Open("C:\\Users\\rzenteno\\OneDrive - AGP GROUP\\5.0 TI\\Proyectos\\17.0 Snowden\\AGPSnowden\\ConsolePresentation\\logo.jpg", FileMode.Open);

                row.ConstantItem(100).Background(Colors.White).Border(1).AlignMiddle()
                .Image(fs); //.Image("logo.jpg");
            
            row.ConstantItem(288).Background(Colors.White).Border(1).AlignMiddle().AlignCenter().Text(
                text =>
                {
                    text.Span("QUALITY REPORT - FINAL INSPECTION").FontFamily(Fonts.Arial).FontSize(14).FontColor(Colors.Black).Bold();
                    text.EmptyLine();
                    text.Span("QUALITY CONTROL DEPARTMENT").FontFamily(Fonts.Arial).FontSize(10).FontColor(Colors.Black).Bold();
                });
            row.ConstantItem(150).Background(Colors.White).Border(1).AlignCenter().AlignMiddle().Background(Colors.Yellow.Medium).Table(table =>
            {
                IContainer DefaultCellStyle(IContainer container, string backgroundColor)
                {
                    return container
                        .Border(1)
                        .BorderColor(Colors.Grey.Lighten1)
                        .Background(backgroundColor)
                       // .PaddingVertical(2)
                       // .PaddingHorizontal(5)
                        .AlignCenter()
                        .AlignMiddle();
                }
                
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(75);
                    columns.ConstantColumn(75);

                });

                table.Header(header =>
                {
                    // please be sure to call the 'header' handler!
                    header.Cell().Element(CellStyle).Text("CODIGO/CODE").FontFamily(Fonts.Arial).FontSize(8);
                    header.Cell().Element(CellStyle).Text("CORP-CAL-CF-001").FontFamily(Fonts.Arial).FontSize(8);
                    // you can extend existing styles by creating additional methods
                });

                table.Cell().Element(CellStyle).Text("VERSION/VERSION").FontFamily(Fonts.Arial).FontSize(8);
                table.Cell().Element(CellStyle).Text("1").FontFamily(Fonts.Arial).FontSize(8);

                table.Cell().Element(CellStyle).Text("FECHA / DATE").FontFamily(Fonts.Arial).FontSize(8);
                table.Cell().Element(CellStyle).Text(DateTime.Now.ToString("dd/mm/yyyy")).FontFamily(Fonts.Arial).FontSize(8);

                table.Cell().Element(CellStyle).Text("HOJA/SHEET").FontFamily(Fonts.Arial).FontSize(8);
                table.Cell().Element(CellStyle).Text("1").FontFamily(Fonts.Arial).FontSize(8);


                IContainer CellStyle(IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
            });
            fs.Dispose();
            fs.Close();
            });
        #endregion

        page.Content().PaddingVertical(5, Unit.Millimetre)
        .Column(column =>
        {
            #region SEC-1 CABECERA CERTIFICADO INFO
            column.Item().Row( row =>
            {
                row.ConstantItem(538).Background(Colors.White).Border(0).Table(table =>
                {
                    IContainer DefaultCellStyle(IContainer container, string backgroundColor)
                    {
                        return container
                            .Height(25)
                            .Border(1)
                            .BorderColor(Colors.Grey.Lighten1)
                            .Background(backgroundColor)

                            // .PaddingVertical(2)
                            // .PaddingHorizontal(5)
                            //.AlignCenter()
                            .AlignMiddle();
                    }

                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(89);
                        columns.ConstantColumn(89);
                        columns.ConstantColumn(89);
                        columns.ConstantColumn(89);
                        columns.ConstantColumn(89);
                        columns.ConstantColumn(89);

                    });

                    table.Header(header =>
                    {
                        // please be sure to call the 'header' handler!
                        header.Cell().Text("CERTIFICATE NUMBER :").FontFamily(Fonts.Arial).FontSize(9).Bold();
                        header.Cell().Text(CERTIFICATE_NUMBER).FontFamily(Fonts.Arial).FontSize(9).Bold();
                        header.Cell().Text("").FontFamily(Fonts.Arial).FontSize(8);
                        header.Cell().Text("").FontFamily(Fonts.Arial).FontSize(8);
                        header.Cell().Text("").FontFamily(Fonts.Arial).FontSize(8);
                        header.Cell().Text("").FontFamily(Fonts.Arial).FontSize(8);
                        // you can extend existing styles by creating additional methods
                    });

                    table.Cell().Text("SUPPLIER:").FontFamily(Fonts.Arial).FontSize(8).Bold();
                    table.Cell().AlignLeft().Text(SUPPLIER).FontFamily(Fonts.Arial).FontSize(8);

                    table.Cell().Text("SUPPLIER ADDRESS:").FontFamily(Fonts.Arial).FontSize(8).Bold();
                    table.Cell().ColumnSpan(3).AlignLeft().Text(SUPPLIER_ADDRESS).FontFamily(Fonts.Arial).FontSize(8);


                    table.Cell().Text("CLIENT:").FontFamily(Fonts.Arial).FontSize(8).Bold();
                    table.Cell().Text(CLIENT).FontFamily(Fonts.Arial).FontSize(8);
                    table.Cell().Text("PRODUCTION ORDER:").FontFamily(Fonts.Arial).FontSize(8).Bold();
                    table.Cell().Text(PRODUCTION_ORDER).FontFamily(Fonts.Arial).FontSize(8);
                    table.Cell().Text("COLOR:").FontFamily(Fonts.Arial).FontSize(8).Bold();
                    table.Cell().Text(COLOR).FontFamily(Fonts.Arial).FontSize(8);

                    table.Cell().Text("VEHICLE:").FontFamily(Fonts.Arial).FontSize(8).Bold();
                    table.Cell().Text(VEHICLE).FontFamily(Fonts.Arial).FontSize(8);
                    table.Cell().Text("COMPOSITION:").FontFamily(Fonts.Arial).FontSize(8).Bold();
                    table.Cell().Text(COMPOSITION).FontFamily(Fonts.Arial).FontSize(8);
                    table.Cell().Text("THICKNESS(mm):").FontFamily(Fonts.Arial).FontSize(8).Bold();
                    table.Cell().Text(THICKNESS).FontFamily(Fonts.Arial).FontSize(8);

                    table.Cell().Text("AQL:").FontFamily(Fonts.Arial).FontSize(8).Bold();
                    table.Cell().Text(AQL).FontFamily(Fonts.Arial).FontSize(8);
                    table.Cell().Text("SAMPLE SIZE:").FontFamily(Fonts.Arial).FontSize(8).Bold();
                    table.Cell().Text(SAMPLE_SIZE).FontFamily(Fonts.Arial).FontSize(8);
                    table.Cell().Text("PRODUCTION LOT:").FontFamily(Fonts.Arial).FontSize(8).Bold();
                    table.Cell().Text(PRODUCTION_LOT).FontFamily(Fonts.Arial).FontSize(8);

                    IContainer CellStyle(IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                });


            });
            #endregion

            column.Item().PaddingTop(10).Text("");

            //column.Item().Border(1).Width(100).Height(200).Image(STREAM_IMAGEN_TECNICA);
            #region SEC-2 IMAGEN TECNICA
            column.Item().Border(0).Width(538).Height(150).Row(row =>
            {
                
                if(BYTE_IMAGEN_TECNICA!=null)
                    row.ConstantItem(269).Border(1).AlignCenter().Width(220).Background(Colors.White).AlignMiddle()
                   .Image(BYTE_IMAGEN_TECNICA);
                else
                    row.ConstantItem(269).Border(1).AlignCenter().Width(220).Background(Colors.White).AlignMiddle()
                   .Text("");
                // row.ConstantItem(300).Background(Colors.Blue.Medium).Border(1).Background(Colors.Blue.Medium).Height(100).AlignMiddle()
                // .Image(STREAM_IMAGEN_TECNICA);
                // if (BYTE_IMAGEN_PLANO_STANDAR != null)
                if (BYTE_IMAGEN_PLANO_STANDAR != null)
                    row.ConstantItem(269).Border(1).AlignCenter().Width(220).Background(Colors.White).AlignMiddle()
                    .Image(BYTE_IMAGEN_PLANO_STANDAR);
                else
                    row.ConstantItem(269).Border(1).AlignCenter().Width(220).Background(Colors.White).AlignMiddle()
                    .Text("");
            });

            #endregion
            // column.Item().Width(100).Height(200).Image(STREAM_IMAGEN_TECNICA);
            column.Spacing(15);
            column.Item().AlignCenter().Text("Dimensional Results");
            column.Spacing(5);

            #region SEC-3 DIMENSIONAL RESULT
            column.Item().Row(row =>
            {

                row.ConstantItem(538).AlignMiddle().Border(1).Table(table =>
                {
                    IContainer DefaultCellStyle(IContainer container, string backgroundColor)
                    {
                        return container
                            //.Height(5)
                            .Border(1)
                            .BorderColor(Colors.Grey.Lighten1)
                            .Background(backgroundColor)
                            // .PaddingVertical(2)
                            // .PaddingHorizontal(5)
                            .AlignCenter()
                            .AlignMiddle();
                    }

                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(138);
                        for (int i = 1; i <= 20; i++)
                        {
                            columns.ConstantColumn(20);
                        }
                    });

                    table.Header(header =>
                    {
                        // please be sure to call the 'header' handler!
                        header.Cell().Element(CellStyle).Text("CHARACTERISTCS").FontFamily(Fonts.Arial).FontSize(10).Bold();
                        header.Cell().Element(CellStyle).Text("A").FontFamily(Fonts.Arial).FontSize(9).Bold();
                        header.Cell().Element(CellStyle).Text("B").FontFamily(Fonts.Arial).FontSize(9).Bold();
                        header.Cell().Element(CellStyle).Text("C").FontFamily(Fonts.Arial).FontSize(9).Bold();
                        header.Cell().Element(CellStyle).Text("D").FontFamily(Fonts.Arial).FontSize(9).Bold();
                        header.Cell().Element(CellStyle).Text("E").FontFamily(Fonts.Arial).FontSize(9).Bold();
                        header.Cell().Element(CellStyle).Text("F").FontFamily(Fonts.Arial).FontSize(9).Bold();
                        header.Cell().Element(CellStyle).Text("G").FontFamily(Fonts.Arial).FontSize(9).Bold();
                        header.Cell().Element(CellStyle).Text("H").FontFamily(Fonts.Arial).FontSize(9).Bold();
                        header.Cell().Element(CellStyle).Text("I").FontFamily(Fonts.Arial).FontSize(9).Bold();
                        header.Cell().Element(CellStyle).Text("J").FontFamily(Fonts.Arial).FontSize(9).Bold();
                        header.Cell().Element(CellStyle).Text("K").FontFamily(Fonts.Arial).FontSize(9).Bold();
                        header.Cell().Element(CellStyle).Text("L").FontFamily(Fonts.Arial).FontSize(9).Bold();
                        header.Cell().Element(CellStyle).Text("M").FontFamily(Fonts.Arial).FontSize(9).Bold();
                        header.Cell().Element(CellStyle).Text("N").FontFamily(Fonts.Arial).FontSize(9).Bold();
                        header.Cell().Element(CellStyle).Text("O").FontFamily(Fonts.Arial).FontSize(9).Bold();
                        header.Cell().Element(CellStyle).Text("P").FontFamily(Fonts.Arial).FontSize(9).Bold();
                        header.Cell().Element(CellStyle).Text("Q").FontFamily(Fonts.Arial).FontSize(9).Bold();
                        header.Cell().Element(CellStyle).Text("R").FontFamily(Fonts.Arial).FontSize(9).Bold();
                        header.Cell().Element(CellStyle).Text("S").FontFamily(Fonts.Arial).FontSize(9).Bold();
                        header.Cell().Element(CellStyle).Text("T").FontFamily(Fonts.Arial).FontSize(9).Bold();

                        // you can extend existing styles by creating additional methods
                    });

                    foreach(CertificadoIfdimension param in DIMENSIONAL_RESULT.Where(x=>x.ParametroInspeccionId== 550))
                    {
                        table.Cell().Element(CellStyle).Text(param.Parametro).FontFamily(Fonts.Arial).FontSize(9).Bold();
                        
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val1).FontFamily(Fonts.Arial).FontSize(8).Bold();
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val2).FontFamily(Fonts.Arial).FontSize(8).Bold();
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val3).FontFamily(Fonts.Arial).FontSize(8).Bold();
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val4).FontFamily(Fonts.Arial).FontSize(8).Bold();
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val5).FontFamily(Fonts.Arial).FontSize(8).Bold();
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val6).FontFamily(Fonts.Arial).FontSize(8).Bold();
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val7).FontFamily(Fonts.Arial).FontSize(8).Bold();
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val8).FontFamily(Fonts.Arial).FontSize(8).Bold();
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val9).FontFamily(Fonts.Arial).FontSize(8).Bold();
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val10).FontFamily(Fonts.Arial).FontSize(8).Bold();
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val11).FontFamily(Fonts.Arial).FontSize(8).Bold();
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val12).FontFamily(Fonts.Arial).FontSize(8).Bold();
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val13).FontFamily(Fonts.Arial).FontSize(8).Bold();
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val14).FontFamily(Fonts.Arial).FontSize(8).Bold();
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val15).FontFamily(Fonts.Arial).FontSize(8).Bold();
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val16).FontFamily(Fonts.Arial).FontSize(8).Bold();
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val17).FontFamily(Fonts.Arial).FontSize(8).Bold();
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val18).FontFamily(Fonts.Arial).FontSize(8).Bold();
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val19).FontFamily(Fonts.Arial).FontSize(8).Bold();
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val20).FontFamily(Fonts.Arial).FontSize(8).Bold();
                    }

                    IContainer CellStyle(IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                });
            });
            column.Spacing(5);
            column.Item().Row(row =>
            {

                row.ConstantItem(538).Background(Colors.White).Border(1).Table(table =>
                {
                    IContainer DefaultCellStyle(IContainer container, string backgroundColor)
                    {
                        return container
                            //.Height(5)
                            .Border(1)
                            .BorderColor(Colors.Grey.Lighten1)
                            .Background(backgroundColor)
                            // .PaddingVertical(2)
                            // .PaddingHorizontal(5)
                            .AlignCenter()
                            .AlignMiddle();
                    }

                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(138);
                        for (int i = 1; i <= 20; i++)
                        {
                            columns.ConstantColumn(20);
                        }
                    });

                    table.Header(header =>
                    {
                        // please be sure to call the 'header' handler!
                        header.Cell().Element(CellStyle).Text("CHARACTERISTCS").FontFamily(Fonts.Arial).FontSize(10).Bold();
                        for (int i = 1; i <= 20; i++)
                        {
                            header.Cell().Element(CellStyle).Text(i.ToString()).FontFamily(Fonts.Arial).FontSize(9).Bold();
                        }
                        // you can extend existing styles by creating additional methods
                    });

                    foreach (CertificadoIfdimension param in DIMENSIONAL_RESULT.Where(x => x.ParametroInspeccionId != 550))
                    {
                        table.Cell().Element(CellStyle).Text(param.Parametro).FontFamily(Fonts.Arial).FontSize(9).Bold();
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val1).FontFamily(Fonts.Arial).FontSize(8);
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val2).FontFamily(Fonts.Arial).FontSize(8);
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val3).FontFamily(Fonts.Arial).FontSize(8);
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val4).FontFamily(Fonts.Arial).FontSize(8);
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val5).FontFamily(Fonts.Arial).FontSize(8);
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val6).FontFamily(Fonts.Arial).FontSize(8);
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val7).FontFamily(Fonts.Arial).FontSize(8);
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val8).FontFamily(Fonts.Arial).FontSize(8);
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val9).FontFamily(Fonts.Arial).FontSize(8);
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val10).FontFamily(Fonts.Arial).FontSize(8);
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val11).FontFamily(Fonts.Arial).FontSize(8);
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val12).FontFamily(Fonts.Arial).FontSize(8);
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val13).FontFamily(Fonts.Arial).FontSize(8);
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val14).FontFamily(Fonts.Arial).FontSize(8);
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val15).FontFamily(Fonts.Arial).FontSize(8);
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val16).FontFamily(Fonts.Arial).FontSize(8);
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val17).FontFamily(Fonts.Arial).FontSize(8);
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val18).FontFamily(Fonts.Arial).FontSize(8);
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val19).FontFamily(Fonts.Arial).FontSize(8);
                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val20).FontFamily(Fonts.Arial).FontSize(8);
                    }


                    IContainer CellStyle(IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                });
            });
            #endregion
            
            column.Spacing(5);
            column.Item().PaddingTop(20).AlignCenter().Text("Apariencia Results");
            column.Spacing(5);

            

            #region SEC-4 APARIENCIA RESULT
            column.Item().AlignCenter().Row(row =>
            {

                row.ConstantItem(538).AlignCenter().Background(Colors.White).Border(1).Table(table =>
                {
                    IContainer DefaultCellStyle(IContainer container, string backgroundColor)
                    {
                        return container
                            //.Height(5)
                            .Border(1)
                            .BorderColor(Colors.Grey.Lighten1)
                            .Background(backgroundColor)
                            // .PaddingVertical(2)
                            // .PaddingHorizontal(5)
                            .AlignCenter()
                            .AlignMiddle();
                    }

                    table.ColumnsDefinition(columns =>
                    {
                        columns.ConstantColumn(120);
                        columns.ConstantColumn(120);
                        columns.ConstantColumn(120);
                        columns.ConstantColumn(120);
                    });

                       foreach(CertificadoIfapariencias apariencia in APARIENCIA_RESULT)
                        {
                            table.Cell().Element(CellStyle).Text(apariencia.Parametro).FontFamily(Fonts.Arial).FontSize(10).Bold();
                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(apariencia.Valor).FontFamily(Fonts.Arial).FontSize(10);
                        }


                    IContainer CellStyle(IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                });
            });

            #endregion

            #region SEC-5 OPTICAL INSPECTION

            column.Item().PageBreak();
            column.Item().Grid(grid =>
             {
                 grid.VerticalSpacing(15);
                 grid.HorizontalSpacing(15);
                 grid.AlignCenter();
                 grid.Columns(10); // 12 by default

                 foreach(InspeccionOptica ins in INSPECCIONES_OPTICAS)
                 {
                     if(ins.ImageByte!=null)
                     grid.Item(5).Background(Colors.White).Table(table =>
                     {
                         table.ColumnsDefinition(columns =>
                         {
                             columns.RelativeColumn(100);

                         });

                         table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(ins.Parametro).FontFamily(Fonts.Arial).FontSize(8);
                         table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Image(ins.ImageByte);
                     });
                 }
                 
             });
        });
        #endregion

        #region SEC - FOOTER
        page.Footer().AlignCenter().Border(0).Row(row=>
        {
            row.ConstantItem(175).Background(Colors.White).Border(0).Height(50).AlignMiddle().AlignCenter()
               .Table(table =>
               {
                   table.ColumnsDefinition(columns =>
                   {
                       columns.RelativeColumn(100);

                   });

                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(INSPECTOR).FontFamily(Fonts.Arial).FontSize(9);
                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text("___________________").FontFamily(Fonts.Arial).FontSize(9);
                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text("INSPECTED").FontFamily(Fonts.Arial).FontSize(9);
                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text("INSPECTOR").FontFamily(Fonts.Arial).FontSize(9).Bold();
               });

            row.ConstantItem(175).Background(Colors.White).Border(0).Height(50).AlignMiddle().AlignCenter()
               .Table(table =>
               {
                   table.ColumnsDefinition(columns =>
                   {
                       columns.RelativeColumn(100);

                   });

                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(QUALITY_ENGINEER).FontFamily(Fonts.Arial).FontSize(9);
                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text("___________________").FontFamily(Fonts.Arial).FontSize(9);
                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text("CHECKED").FontFamily(Fonts.Arial).FontSize(9);
                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text("QUALITY ENGINEER").FontFamily(Fonts.Arial).FontSize(9).Bold();
               });
            row.ConstantItem(175).Background(Colors.White).Border(0).Height(50).AlignMiddle().AlignCenter()
               .Table(table =>
               {
                   table.ColumnsDefinition(columns =>
                   {
                       columns.RelativeColumn(100);

                   });

                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(QUALITY_MANAGER).FontFamily(Fonts.Arial).FontSize(9);
                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text("___________________").FontFamily(Fonts.Arial).FontSize(9);
                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text("AUTHORIZED").FontFamily(Fonts.Arial).FontSize(9);
                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text("QUALITY MANAGER").FontFamily(Fonts.Arial).FontSize(9).Bold();
               });


        });
        #endregion
    });

});

// instead of the standard way of generating a PDF file
document.GeneratePdf("hello.pdf");

// use the following invocation
document.ShowInPreviewer();

