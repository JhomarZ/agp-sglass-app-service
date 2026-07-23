using AGP.Gordon.DataAccessLayer.SAPEXPANSION;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using System.Reflection.Metadata.Ecma335;
using SkiaSharp;
using System.Drawing;
using System.IO;
using Microsoft.Identity.Client;
using AGP.Gordon.CommonLayer;
using System.ComponentModel;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using System.Runtime.InteropServices;
using SixLabors.ImageSharp;
using Microsoft.EntityFrameworkCore;
using AGP.Gordon.DataAccessLayer.Configurations;

namespace AGP.Gordon.ServiceLayer
{
    public class CertificadoIFService
    {
        // Clave: Zfer del cliente especial. Valor: rutas fijas en el proyecto.  (certificados sentinel)
        private static readonly Dictionary<string, (string Silueta, string Zonas)> _mapa =
            new Dictionary<string, (string, string)>(StringComparer.OrdinalIgnoreCase)
        {
        { "700167180", ("src/sentinel/delantero_derecho_zonas.jpg", "src/sentinel/delantero_derecho_zonas.jpg") },
        { "700167179", ("src/sentinel/delantera_izquierda_zonas.jpg", "src/sentinel/delantera_izquierda_zonas.jpg") },
        { "700165337",   ("src/sentinel/trazera_derecha_zonas.jpg",   "src/sentinel/trazera_derecha_zonas.jpg") },
        { "700165336",    ("src/sentinel/trazera_izquierda_zonas.jpg",    "src/sentinel/trazera_izquierda_zonas.jpg") },
        { "700164766",    ("src/sentinel/parabrisas_zonas.jpg",    "src/sentinel/parabrisas_zonas.jpg") }
        };

  

        private readonly HelpImage _HelpImage;
        private readonly HelpExcel _HelpExcel;
        // GET: STable
        public CertificadoIFService()
        {
            _HelpImage = new HelpImage();
            _HelpExcel = new HelpExcel();
        }

        #region Funciones para Certificados Sentinel

        private static readonly (string Label, int ParamId)[] FilasIzquierda = new[]
        {
            ("External black band",    533),
            ("Internal black band 1",  639),
            ("Internal black band 2",  671),
            ("Thickness",              593),
        };

        private static readonly (string Label, int ParamId)[] FilasDerecha = new[]
        {
            ("Hot resistance, Ohms",   682),
            ("Cold resistance, Ohms",  680), // ⚠ confirmar
            ("Mdp Zone 1",             739),
            ("Mdp Zone 2",             740),
        };

        private static readonly (string Label, int ParamId)[] FilasApariencia = new[]
        {
            ("Image Reflection",              0),   // TODO: ID pendiente
            ("Package edge appareance",     635),
            ("Silkscreen appareance",       461),
            ("Thermographic imaging",         0),   // TODO
            ("Distortion with Zebra at 45",   0),   // TODO
            ("Distortion with Zebra at 0",    0),   // TODO
            ("Distortion with Zebra",         0),   // TODO
            ("double image",                  0),   // TODO
            ("Appareance external face",      0),   // TODO
            ("Black band design",             0),   // TODO
            ("Appareance inner side",         0),   // TODO
            ("Finishing and cleaning edges", 634),
            ("Gradient",                     578),
            ("Mass Defect",                  460),
            ("Color",                        633),
            ("Logo (art)",                   459),
        };

        // TODO: URGENTE/TEMPORAL — reemplazar por tabla maestra Parámetro+Valor+Tolerancia cuando esté disponible en el sistema principal.
        private static readonly Dictionary<string, Dictionary<int, string[]>> TolerenciasPorZfer =
            new Dictionary<string, Dictionary<int, string[]>>
            {
                ["700164766"] = new Dictionary<int, string[]> // Parabrisas
                {
                    [533] = new[] { "67±3", "33±3", "108±3", "33±3" },  //External black band
                    [639] = new[] { "68", "36", "134", "36" },          //Internal black band 1
                    [671] = new[] { "297+3", "41+3", "186+3", "51+3" }, //Internal black band 2
                    [593] = new[] { "41±2", "41±2", "41±2", "41±2" },   //Thickness
                    [682] = new[] { "0,67-1,24", "0,67-1,24" },         //Hot resistance, Ohms
                    [680] = new[] { "0,67-1,24", "0,67-1,24" },         //Cold resistance, Ohms
                    [739] = new[] { "<175", "<175" },                   //Mdp Zone 1
                    [740] = new[] { "<175", "<175" },                   //Mdp Zone 2
                },
                ["700165336"] = new Dictionary<int, string[]> // PIEZA: TRAZERA IZQUIERDA
                {
                    [533] = new[] { "34±2", "34±2", "58±2", "37±2" },  //External black band
                    [639] = new[] { "65±2", "59±2", "-", "51±2" },          //Internal black band 1
                    [671] = new[] { "83±2", "67±2", "68±2", "50±2" }, //Internal black band 2
                    [593] = new[] { "61±2", "61±2", "61±2", "61±2" },   //Thickness
                    [682] = new[] { "-", "-" },         //Hot resistance, Ohms
                    [680] = new[] { "-", "-" },         //Cold resistance, Ohms
                    [739] = new[] { "-", "-" },                   //Mdp Zone 1
                    [740] = new[] { "-", "-" },                   //Mdp Zone 2
                },
                ["700165337"] = new Dictionary<int, string[]> // PIEZA: TRASERA DERECHA 
                {
                    [533] = new[] { "34±2", "34±2", "58±2", "37±2" },  //External black band
                    [639] = new[] { "65±2", "59±2", "-", "51±2" },          //Internal black band 1
                    [671] = new[] { "83±2", "67±2", "68±2", "50±2" }, //Internal black band 2
                    [593] = new[] { "61±2", "61±2", "61±2", "61±2" },   //Thickness
                    [682] = new[] { "-", "-" },         //Hot resistance, Ohms
                    [680] = new[] { "-", "-" },         //Cold resistance, Ohms
                    [739] = new[] { "-", "-" },                   //Mdp Zone 1
                    [740] = new[] { "-", "-" },                   //Mdp Zone 2
                },
                ["700167179"] = new Dictionary<int, string[]> // PIEZA: TRASERA DERECHA 
                {
                    [533] = new[] { "34±2", "37±2", "58±2", "34±2" },  //External black band
                    [639] = new[] { "65±2", "51±2", "-", "59±2" },          //Internal black band 1
                    [671] = new[] { "83±2", "50±2", "68±2", "67±2" }, //Internal black band 2
                    [593] = new[] { "61±2", "61±2", "61±2", "61±2" },   //Thickness
                    [682] = new[] { "-", "-" },         //Hot resistance, Ohms
                    [680] = new[] { "-", "-" },         //Cold resistance, Ohms
                    [739] = new[] { "<89", "<275" },                   //Mdp Zone 1
                    [740] = new[] { "<89", "<275" },                   //Mdp Zone 2
                },
                ["700167180"] = new Dictionary<int, string[]> // PIEZA: DELANTERO DERECHO  
                {
                    [533] = new[] { "34±2", "34±2", "58±2", "37±2" },  //External black band
                    [639] = new[] { "65±2", "59±2", "-", "51±2" },          //Internal black band 1
                    [671] = new[] { "83±2", "67±2", "68±2", "50±2" }, //Internal black band 2
                    [593] = new[] { "61±2", "61±2", "61±2", "61±2" },   //Thickness
                    [682] = new[] { "-", "-" },         //Hot resistance, Ohms
                    [680] = new[] { "-", "-" },         //Cold resistance, Ohms
                    [739] = new[] { "<89", "<275" },                   //Mdp Zone 1
                    [740] = new[] { "<89", "<275" },                   //Mdp Zone 2
                },
                // TODO: agregar 700167179, 700167180, 700165336, 700165337 cuando lleguen las tolerancias reales
            };
        public static bool TryGetImagenes(string zfer, out string rutaSilueta, out string rutaZonas)
        {
      
            if (zfer != null && _mapa.TryGetValue(zfer.Trim(), out var val))
            {
                rutaSilueta = val.Silueta;
                rutaZonas = val.Zonas;
                return true;
            }
            rutaSilueta = null;
            rutaZonas = null;
            return false;
        }

        // Obtiene hasta N valores dinámicos (Val1..Val25) de un parámetro dimensional
        private static string[] GetValoresDinamicos(CertificadoIfdimension? param, int numColumnas)
        {
            if (param == null)
                return Enumerable.Repeat("", numColumnas).ToArray();

            var todos = new[]
            {
                param.Val1, param.Val2, param.Val3, param.Val4, param.Val5,
                param.Val6, param.Val7, param.Val8, param.Val9, param.Val10,
                param.Val11, param.Val12, param.Val13, param.Val14, param.Val15,
                param.Val16, param.Val17, param.Val18, param.Val19, param.Val20,
                param.Val21, param.Val22, param.Val23, param.Val24, param.Val25
             };

            return todos.Take(numColumnas).Select(v => v ?? "").ToArray();
        }

        // Renderiza el par de filas: "Nom. & Tol." (gris, hardcode) + "Label" (blanco, valores reales)
        private static void RenderFilaCaracteristica(
            QuestPDF.Fluent.TableDescriptor table,
            Func<QuestPDF.Infrastructure.IContainer, QuestPDF.Infrastructure.IContainer> cellStyleGris,
            string label,
            string[] tolHardcode,      // TODO: mover a config/BD más adelante
            CertificadoIfdimension? paramData,
            int numColumnas)
        {
            // Fila gris "Nom. & Tol." — hardcodeada por ahora
            table.Cell().Element(cellStyleGris).Text("Nom. & Tol.").FontFamily(Fonts.Arial).FontSize(8).Bold();
            for (int i = 0; i < numColumnas; i++)
            {
                string tol = i < tolHardcode.Length ? tolHardcode[i] : "";
                table.Cell().Element(cellStyleGris).Text(tol).FontFamily(Fonts.Arial).FontSize(7).Bold();
            }

            // Fila blanca con el label + valores reales (dinámico por ParametroInspeccionId)
            table.Cell().Element(cellStyleGris).Text(label).FontFamily(Fonts.Arial).FontSize(8).Bold();
            var valores = GetValoresDinamicos(paramData, numColumnas);
            foreach (var val in valores)
            {
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter()
                     .Text(val).FontFamily(Fonts.Arial).FontSize(7);
            }
        }

        private static void RenderFilaCaracteristicaDinamica(
            QuestPDF.Fluent.TableDescriptor table,
            Func<QuestPDF.Infrastructure.IContainer, QuestPDF.Infrastructure.IContainer> cellStyle,
            string label, int parametroId, string zfer,
            List<CertificadoIfdimension> dimensionalResult, int numColumnas)
        {
            var paramData = dimensionalResult.FirstOrDefault(x => x.ParametroInspeccionId == parametroId);

            string[] tolerancias = (TolerenciasPorZfer.TryGetValue(zfer ?? "", out var tolPorParam)
                                     && tolPorParam.TryGetValue(parametroId, out var tolArray))
                ? tolArray
                : Enumerable.Repeat("-", numColumnas).ToArray();

            table.Cell().Element(cellStyle).Text("Nom. & Tol.").FontFamily(Fonts.Arial).FontSize(8).Bold();
            for (int i = 0; i < numColumnas; i++)
                table.Cell().Element(cellStyle).Text(i < tolerancias.Length ? tolerancias[i] : "-")
                    .FontFamily(Fonts.Arial).FontSize(7).Bold();

            table.Cell().Element(cellStyle).Text(label).FontFamily(Fonts.Arial).FontSize(8).Bold();
            string[] valores = paramData != null
                ? GetValoresDinamicos(paramData, numColumnas)   // ya lo tienes definido
                : Enumerable.Repeat("NA", numColumnas).ToArray();

            foreach (var val in valores)
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter()
                    .Text(val).FontFamily(Fonts.Arial).FontSize(7);
        }
        private static void RenderAparienciaCheckbox(QuestPDF.Fluent.TableDescriptor table, CertificadoIfapariencias? apariencia)
        {
            if (apariencia == null)
            {
                // El parámetro no aplica a este ZFER/pieza
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).Background(Colors.White)
                    .AlignCenter().AlignMiddle()
                    .Text("NA").FontFamily(Fonts.Arial).FontSize(8).Bold();
                return;
            }

            string valor = apariencia.Valor?.Trim() ?? "";

            if (string.Equals(valor, "NA", StringComparison.OrdinalIgnoreCase))
            {
                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).Background(Colors.White)
                    .AlignCenter().AlignMiddle()
                    .Text("NA").FontFamily(Fonts.Arial).FontSize(8).Bold();
                return;
            }

            bool cumple = string.Equals(valor, "CUMPLE", StringComparison.OrdinalIgnoreCase);

            var celda = table.Cell()
                .Border(1).BorderColor(Colors.Grey.Lighten1).Background(Colors.White)
                .AlignCenter().AlignMiddle().Padding(2);

            if (cumple)
            {
                string rutaIcono = Path.Combine(AppContext.BaseDirectory, "src", "iconos", "check.png");
                celda.MaxWidth(16).MaxHeight(16).Image(rutaIcono);
            }
            else
            {
                celda.Text(""); // NO CUMPLE explícito: casilla vacía, como ya definimos antes
            }
        }

        #endregion

        public CertificadoIf GetById(long CertificadoId)
        {
            CertificadoIf certificado = new CertificadoIf();
            using (var db = new SapexpansionContext())
            {
                certificado = db.CertificadoIfs.Where(x=>x.Id==CertificadoId).FirstOrDefault();
            }
            return certificado;
        }

        public PiezaSap GetPiezaByOrden(int? Compania,string Orden)
        {
            PiezaSap pieza = new PiezaSap();
            using (var db = new SapexpansionContext())
            {
                pieza = db.PiezaSaps.Where(x=>x.IdCompania== Compania && x.OrdProceso==Orden ).FirstOrDefault();
            }
            return pieza;
        }

        public async Task<List<CertificadoIfdimension>> GetMedicionesDimensionales(long CertificadoId)
        {
            //List<CertificadoIfdimension> dimensionales = new List<CertificadoIfdimension>();

            using (var db = new SapexpansionContext())
            {
                try
                {
                    var dimensionales = await db.CertificadoIfdimensions
                        .Include(x => x.ParametroInspeccion)  // Asumiendo que tienes la relación configurada
                        .Where(x => x.CertificadoId == CertificadoId
                                && x.Origen == 1)
                        .AsSplitQuery() // Mejora el rendimiento para queries con Include
                        .ToListAsync();

                    return dimensionales;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            /*
            using (var db = new SapexpansionContext())
            {
                dimensionales = db.CertificadoIfdimensions.Where(x => x.CertificadoId== CertificadoId && x.Origen==1).ToList();
                foreach (CertificadoIfdimension item in dimensionales)
                {
                    int? parametroId = item.ParametroInspeccionId;
                    item.parametro = db.ParametrosInspeccions.Where(x=>x.Id==parametroId).FirstOrDefault();
                }
            }
            return dimensionales;*/
        }

        public async Task<List<CertificadoIfdimension>> GetMedicionesElectricas(int CertificadoId)
        {
            /*
            List<CertificadoIfdimension> electricas = new List<CertificadoIfdimension>();
            using (var db = new SapexpansionContext())
            {
                electricas = db.CertificadoIfdimensions.Where(x => x.CertificadoId == CertificadoId && x.Origen == 2).ToList();
                foreach (CertificadoIfdimension item in electricas)
                {
                    int? parametroId = item.ParametroInspeccionId;
                    item.parametro = db.ParametrosInspeccions.Where(x => x.Id == parametroId).FirstOrDefault();
                }
            }
            return electricas;*/

            using (var db = new SapexpansionContext())
            {
                try
                {
                    // Carga anticipada (eager loading) con Include para evitar consultas separadas

                    // Realizar una única consulta con Include para evitar N+1
                    var inspecionesElectricas = await db.CertificadoIfdimensions
                        .Include(x => x.ParametroInspeccion)  // Asumiendo que tienes la relación configurada
                        .Where(x => x.CertificadoId == CertificadoId
                                && x.Origen== 2)
                        .AsSplitQuery() // Mejora el rendimiento para queries con Include
                        .ToListAsync();
                    /*
                    var electricas = db.CertificadoIfdimensions
                        .Where(x => x.CertificadoId == CertificadoId && x.Origen == 2)
                        .Select(item => new
                        {
                            Dimension = item,
                            Parametro = item.ParametroInspeccionId.HasValue ?
                                db.ParametrosInspeccions.FirstOrDefault(p => p.Id == item.ParametroInspeccionId) : null
                        })
                        .ToList()
                        .Select(x =>
                        {
                            x.Dimension.Parametro = x.Parametro;
                            return x.Dimension;
                        })
                        .ToList();*/

                    return inspecionesElectricas;
                }
                catch (Exception ex)
                {
                    // Opcional: Logging del error
                    // Logger.LogError($"Error en GetMedicionesElectricas: {ex.Message}");

                    // Devuelve una lista vacía en caso de error
                    throw new Exception(ex.Message);
                }
            }

        }
        public async Task<List<CertificadoIfapariencias>> GetDatosApariencia(long CertificadoId)
        {
            using (var db = new SapexpansionContext())
            {
                try
                {
                    // Carga anticipada (eager loading) con Include para evitar consultas separadas

                    // Realizar una única consulta con Include para evitar N+1
                    var inspecionesApariencia = await db.CertificadoIfapariencia
                        .Include(x => x.ParametroInspeccion)  // Asumiendo que tienes la relación configurada
                        .Where(x => x.CertificadoId == CertificadoId)
                        .AsSplitQuery() // Mejora el rendimiento para queries con Include
                        .ToListAsync();
                  

                    return inspecionesApariencia;
                }
                catch (Exception ex)
                {
                    // Opcional: Logging del error
                    // Logger.LogError($"Error en GetMedicionesElectricas: {ex.Message}");

                    // Devuelve una lista vacía en caso de error
                    throw new Exception(ex.Message);
                }
            }


            /*
            List<CertificadoIfapariencias> apariencias = new List<CertificadoIfapariencias>();
            using (var db = new SapexpansionContext())
            {
                apariencias = db.CertificadoIfapariencia.Where(x => x.CertificadoId == CertificadoId).ToList();
                foreach (CertificadoIfapariencias item in apariencias)
                {
                    int? parametroId = item.ParametroInspeccionId;
                    item.parametro = db.ParametrosInspeccions.Where(x => x.Id == parametroId).FirstOrDefault();
                }
            }
            return apariencias;*/
        }

        public async Task< List<InspeccionOptica>> GetInspeccionesOpticas(long certificadoId)
        {
            /*
            List<InspeccionOptica> inspeccionesOpticas = new List<InspeccionOptica>();
            using (var db = new SapexpansionContext())
            {
                inspeccionesOpticas = db.InspeccionOpticas.Where(x => x.CertificadoId == CertificadoId && x.ParametroInspeccionId!=534 && x.ParametroInspeccionId != 274 && x.TieneImagen=1).ToList();
                foreach (InspeccionOptica item in inspeccionesOpticas)
                {
                    int? parametroId = item.ParametroInspeccionId;
                    item.parametro = db.ParametrosInspeccions.Where(x => x.Id == parametroId).FirstOrDefault();
                }
            }
            return inspeccionesOpticas;*/
            try
            {
                using var db = new SapexpansionContext();

                // Realizar una única consulta con Include para evitar N+1
                var inspeccionesOpticas = await db.InspeccionOpticas
                    .Include(x => x.ParametroInspeccion)  // Asumiendo que tienes la relación configurada
                    .Where(x => x.CertificadoId == certificadoId
                            && x.TieneImagen == 1
                            && x.ParametroInspeccionId != 76 // parametro Ficha-OT
                            && x.ParametroInspeccionId != 534 // Ficha Tecnica
                            )
                    .AsSplitQuery() // Mejora el rendimiento para queries con Include
                    .ToListAsync();
                /*
                _logger.LogInformation(
                    "Retrieved {Count} optical inspections for certificate {CertificadoId}",
                    inspeccionesOpticas.Count,
                    certificadoId);*/

                return inspeccionesOpticas;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<PiezaConcesion> GetImagenTecnicaObservaciones(long CertificadoId)
        {
            List<PiezaConcesion> observaciones = new List<PiezaConcesion>();
            using (var db = new SapexpansionContext())
            {
                observaciones = db.PiezaConcesions.Where(x => x.CertificadoId == CertificadoId && x.Tipo==3).ToList();

                foreach(PiezaConcesion def in observaciones)
                {
                    int? zonaId = def.ZonaId;
                    Clasificadore clasificador = db.Clasificadores.Where(x => x.Id == zonaId).FirstOrDefault();
                    def.Zona = (clasificador!=null) ? clasificador.Nombre:"";
                    int? clasificadorID = def.DefectoId;
                    def.DefectoMaestro = db.Defectos.Where(x => x.Id == clasificadorID).FirstOrDefault(); 
                }

            }
            return observaciones;
        }

        #region Certificados Sentinel
        public async Task<string> CertificadoPDFSglassSentinel(string Idioma, CertificadoIf certificado, PiezaSap pieza, List<CertificadoIfdimension> DIMENSIONAL_RESULT, List<CertificadoIfapariencias> APARIENCIA_RESULT, List<InspeccionOptica> INSPECCIONES_OPTICAS, List<PiezaConcesion> OBSERVACIONES)
        {
            string UrlImageGordon = "http://4.228.184.32:8081/Userimage/";
            string fileName = "Reports/Results/" + certificado.Id.ToString() + "/" + pieza.LoteLogistico + ".pdf";
            try
            {
                #region SEC-1 CABECERA CERTIFICADO INFO
                string CERTIFICATE_NUMBER = certificado.Id.ToString();
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

                DIMENSIONAL_RESULT = await LimitLenCharactersToCellPdf(DIMENSIONAL_RESULT);

                string currentPage = "";

                string rutaSilueta = pieza.ImagenFt;
                string rutaZonas = pieza.IMAGEN_PLANO_STANDAR;
                bool esRutaLocal = false;

                if (TryGetImagenes(pieza.Zfer, out var rutaSiluetaHc, out var rutaZonasHc))
                {
                    rutaSilueta = rutaSiluetaHc;
                    rutaZonas = rutaZonasHc;
                    esRutaLocal = true;
                }


                #region SEC - FOOTER
                string INSPECTOR = pieza.UsuarioCrea;
                string QUALITY_ENGINEER = pieza.GetQualityEngineer();
                string QUALITY_MANAGER = pieza.GetQualityManager();
                #endregion

                //QuestPDF.Settings.License = LicenseType.Community;

                // code in your main method
                var document = QuestPDF.Fluent.Document.Create(container =>
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
                            //FileStream fs = GetFileStrem("src/logo.jpg");
                            FileStream fs = File.Open("src/logo.jpg", FileMode.Open);

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
                                QuestPDF.Infrastructure.IContainer DefaultCellStyle(QuestPDF.Infrastructure.IContainer container, string backgroundColor)
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
                                table.Cell().Element(CellStyle).Text("2").FontFamily(Fonts.Arial).FontSize(8);

                                table.Cell().Element(CellStyle).Text("FECHA / DATE").FontFamily(Fonts.Arial).FontSize(8);
                                table.Cell().Element(CellStyle).Text(DateTime.Now.ToString("dd/MM/yyyy")).FontFamily(Fonts.Arial).FontSize(8);

                                table.Cell().Element(CellStyle).Text("HOJA/SHEET").FontFamily(Fonts.Arial).FontSize(8);
                                table.Cell().Element(CellStyle).Text(text => {

                                    text.CurrentPageNumber().FontFamily(Fonts.Arial).FontSize(8);
                                    //currentPage = text.CurrentPageNumber().ToString();
                                    //var currentPage = text.CurrentPageNumber().ToString();
                                    //text.Span(currentPage).FontFamily(Fonts.Arial).FontSize(8);
                                });


                                QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                            });
                            fs.Dispose();
                            fs.Close();
                        });
                        #endregion

                        page.Content().PaddingVertical(3, Unit.Millimetre)
                        .Column(column =>
                        {
                            #region SEC-1 CABECERA CERTIFICADO INFO
                            column.Item().Row(row =>
                            {
                                row.ConstantItem(538).Background(Colors.White).Border(0).Table(table =>
                                {
                                    QuestPDF.Infrastructure.IContainer DefaultCellStyle(QuestPDF.Infrastructure.IContainer container, string backgroundColor)
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

                                    QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                                });


                            });
                            #endregion

                            column.Item().PaddingTop(2).Text("");


                            #region SEC-2 IMAGEN TECNICA

                            column.Item().Border(0).Width(538).Height(150).Row(row =>
                            {
                                /*  validar si se va usar la imagen de sap o una personalizada
                                if (!string.IsNullOrEmpty(rutaSilueta))
                                    row.ConstantItem(269).Border(1).AlignCenter().Width(220).Background(Colors.White).AlignMiddle()
                                        .Image(esRutaLocal ? File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, rutaSilueta)) : HelpImage.GetFileContent(rutaSilueta));
                                else
                                    row.ConstantItem(269).Border(1).AlignCenter().Width(220).Background(Colors.White).AlignMiddle().Text("");*/

                                if (pieza.ImagenFt != null && pieza.ImagenFt != "")
                                    row.ConstantItem(269).Border(1).AlignCenter().Width(220).Background(Colors.White).AlignMiddle()
                                   .Image(HelpImage.GetFileContent(pieza.ImagenFt));
                                else
                                    row.ConstantItem(269).Border(1).AlignCenter().Width(220).Background(Colors.White).AlignMiddle()
                                   .Text("");

                                if (!string.IsNullOrEmpty(rutaZonas))
                                    row.ConstantItem(269).Border(1).AlignCenter().Width(220).Background(Colors.White).AlignMiddle()
                                        .Image(esRutaLocal ? File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, rutaZonas)) : HelpImage.GetFileContent(rutaZonas));
                                else
                                    row.ConstantItem(269).Border(1).AlignCenter().Width(220).Background(Colors.White).AlignMiddle().Text("");
                            });


                            #endregion
                            // column.Item().Width(100).Height(200).Image(STREAM_IMAGEN_TECNICA);
                            column.Spacing(5);
                            column.Item().AlignCenter().Text("Dimensional Results");
                            column.Spacing(5);

                            #region SEC-3 DIMENSIONAL RESULT (formato Sentinel: 2 tablas)

                            // TODO: por ahora TODAS las filas usan el mismo parámetro 533 ("Banda negra externa")
                            // hasta tener los ParametroInspeccionId reales de cada fila.
                            const int ID_TEMPLATE_TEMPORAL = 533;
                            var paramTemplate = DIMENSIONAL_RESULT.FirstOrDefault(x => x.ParametroInspeccionId == ID_TEMPLATE_TEMPORAL);

                            column.Item().Row(row =>
                            {
                                // ---- TABLA IZQUIERDA (4 columnas) ----
                                row.RelativeItem().Border(1).Table(table =>
                                {
                                    QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer c) =>
                                        c.Border(1).BorderColor(Colors.Grey.Lighten1).Background(Colors.Grey.Lighten3).AlignCenter().AlignMiddle();

                                    table.ColumnsDefinition(cols =>
                                    {
                                        cols.RelativeColumn(2);
                                        cols.ConstantColumn(40);
                                        cols.ConstantColumn(40);
                                        cols.ConstantColumn(40);
                                        cols.ConstantColumn(40);
                                    });

                                    table.Header(header =>
                                    {
                                        header.Cell().Element(CellStyle).Text("CHARACTERISTICS").FontFamily(Fonts.Arial).FontSize(9).Bold();
                                        header.Cell().Element(CellStyle).Text("1").FontFamily(Fonts.Arial).FontSize(9).Bold();
                                        header.Cell().Element(CellStyle).Text("2").FontFamily(Fonts.Arial).FontSize(9).Bold();
                                        header.Cell().Element(CellStyle).Text("3").FontFamily(Fonts.Arial).FontSize(9).Bold();
                                        header.Cell().Element(CellStyle).Text("4").FontFamily(Fonts.Arial).FontSize(9).Bold();
                                    });

                                    // TODO: reemplazar tolHardcode y el ID cuando existan los parámetros reales por fila
                                    foreach (var fila in FilasIzquierda)
                                        RenderFilaCaracteristicaDinamica(table, CellStyle, fila.Label, fila.ParamId, pieza.Zfer, DIMENSIONAL_RESULT, 4);
                                });

                                row.ConstantItem(10); // separación entre tablas

                                // ---- TABLA DERECHA (2 columnas) ----
                                row.RelativeItem().Border(1).Table(table =>
                                {
                                    QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer c) =>
                                        c.Border(1).BorderColor(Colors.Grey.Lighten1).Background(Colors.Grey.Lighten3).AlignCenter().AlignMiddle();

                                    table.ColumnsDefinition(cols =>
                                    {
                                        cols.RelativeColumn(3);
                                        cols.ConstantColumn(55);
                                        cols.ConstantColumn(55);
                                    });

                                    table.Header(header =>
                                    {
                                        header.Cell().Element(CellStyle).Text("CHARACTERISTICS").FontFamily(Fonts.Arial).FontSize(9).Bold();
                                        header.Cell().Element(CellStyle).Text("1").FontFamily(Fonts.Arial).FontSize(9).Bold();
                                        header.Cell().Element(CellStyle).Text("2").FontFamily(Fonts.Arial).FontSize(9).Bold();
                                    });

                                    foreach (var fila in FilasDerecha)
                                        RenderFilaCaracteristicaDinamica(table, CellStyle, fila.Label, fila.ParamId, pieza.Zfer, DIMENSIONAL_RESULT, 2);
                                });
                            });

                            #endregion

                            column.Item().PaddingTop(2).Text("");
                            column.Spacing(15);
                            column.Item().PaddingTop(0).AlignCenter().Text("Appearance Results");
                            column.Spacing(5);



                            #region SEC-4 APARIENCIA RESULT

                            column.Item().AlignCenter().Row(row =>
                            {
                                row.ConstantItem(538).AlignCenter().Background(Colors.White).Border(1).Table(table =>
                                {
                                    QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container)
                                    {
                                        return container
                                            .Border(1).BorderColor(Colors.Grey.Lighten1)
                                            .Background(Colors.Grey.Lighten3)
                                            .AlignCenter().AlignMiddle();
                                    }

                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(3);
                                        columns.ConstantColumn(40);
                                        columns.RelativeColumn(3);
                                        columns.ConstantColumn(40);
                                    });

                                    // recorremos el catálogo de a pares (izquierda/derecha), igual que tu tabla actual de 2 columnas
                                    for (int i = 0; i < FilasApariencia.Length; i += 2)
                                    {
                                        var filaIzq = FilasApariencia[i];
                                        var dataIzq = APARIENCIA_RESULT.FirstOrDefault(x => x.ParametroInspeccionId == filaIzq.ParamId);
                                        table.Cell().Element(CellStyle).Text(filaIzq.Label).FontFamily(Fonts.Arial).FontSize(9).Bold();
                                        RenderAparienciaCheckbox(table, dataIzq);

                                        if (i + 1 < FilasApariencia.Length)
                                        {
                                            var filaDer = FilasApariencia[i + 1];
                                            var dataDer = APARIENCIA_RESULT.FirstOrDefault(x => x.ParametroInspeccionId == filaDer.ParamId);
                                            table.Cell().Element(CellStyle).Text(filaDer.Label).FontFamily(Fonts.Arial).FontSize(9).Bold();
                                            RenderAparienciaCheckbox(table, dataDer);
                                        }
                                    }
                                });
                            });

                            #endregion

                            #region SEC-4.1 Defectos

                            if (OBSERVACIONES.Count > 0)
                            {
                                column.Item().PageBreak(); // fuerza que esta sección siempre inicie en página nueva

                                column.Item().Border(0).Width(538).AlignMiddle().Height(300).AlignMiddle().Row(row =>
                                {

                                    if (pieza.DefectoImagen != null && pieza.DefectoImagen != "")
                                        row.ConstantItem(269).Border(0).AlignCenter().Width(220).Background(Colors.White).Height(300).AlignMiddle()
                                        .Image(HelpImage.GetFileContent(pieza.DefectoImagen));
                                    else
                                        row.ConstantItem(269).Border(0).AlignCenter().Width(220).Background(Colors.White).AlignMiddle()
                                        .Text("");

                                    row.ConstantItem(260).Height(300).AlignMiddle().Border(1).Table(table =>
                                    {
                                        QuestPDF.Infrastructure.IContainer DefaultCellStyle(QuestPDF.Infrastructure.IContainer container, string backgroundColor)
                                        {
                                            return container
                                                //.Height(5)
                                                .Border(1)
                                                .BorderColor(Colors.Grey.Lighten1)
                                                .Background(backgroundColor)
                                                .PaddingVertical(1)
                                                .PaddingHorizontal(1)
                                                .AlignCenter()
                                                .AlignMiddle();
                                        }

                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.ConstantColumn(30);
                                            columns.ConstantColumn(30);
                                            columns.ConstantColumn(120);
                                            columns.ConstantColumn(80);

                                        });

                                        table.Header(header =>
                                        {
                                            // please be sure to call the 'header' handler!
                                            header.Cell().Element(CellStyle).Text("ITEM").FontFamily(Fonts.Arial).FontSize(8).Bold();
                                            header.Cell().Element(CellStyle).Text("ZONE").FontFamily(Fonts.Arial).FontSize(8).Bold();
                                            header.Cell().Element(CellStyle).Text("DEFECT").FontFamily(Fonts.Arial).FontSize(8).Bold();
                                            header.Cell().Element(CellStyle).Text("SIZE").FontFamily(Fonts.Arial).FontSize(8).Bold();

                                            // you can extend existing styles by creating additional methods
                                        });

                                        int index = 0;
                                        int maxRows = 10;
                                        foreach (PiezaConcesion obs in OBSERVACIONES.Take(maxRows))
                                        {
                                            index++;

                                            // ITEM
                                            table.Cell().Element(DataCellStyle).Text(index.ToString())
                                                .FontFamily(Fonts.Arial).FontSize(7);

                                            // ZONE  
                                            table.Cell().Element(DataCellStyle).Text(obs.Zona ?? "")
                                                .FontFamily(Fonts.Arial).FontSize(7);

                                            // DEFECT - Con manejo de texto largo
                                            table.Cell().Element(DataCellStyle).Text(obs.DefectoMaestro?.NombreIngles ?? "")
                                                .FontFamily(Fonts.Arial).FontSize(7).LineHeight(0.9f);

                                            // SIZE
                                            table.Cell().Element(DataCellStyle).Text(obs.Tamanio ?? null)
                                                .FontFamily(Fonts.Arial).FontSize(7);
                                        }

                                        // Estilos de celda
                                        QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container) =>
                                            DefaultCellStyle(container, Colors.Grey.Lighten3);

                                        QuestPDF.Infrastructure.IContainer DataCellStyle(QuestPDF.Infrastructure.IContainer container) =>
                                            DefaultCellStyle(container, Colors.White);

                                        //QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                                    });

                                });

                            }

                            #endregion


                            column.Spacing(10);

                            #region SEC-5 OPTICAL INSPECTION

                            if (INSPECCIONES_OPTICAS.Count > 0)
                            {
                                // Filtrar solo las inspecciones que tienen imagen
                                var inspeccionesConImagen = INSPECCIONES_OPTICAS
                                    .Where(x => !string.IsNullOrEmpty(x.PathImage))
                                    .ToList();


                                if (inspeccionesConImagen.Count > 0)
                                {

                                    for (int i = 0; i < inspeccionesConImagen.Count; i += 2)
                                    {
                                        column.Item().Grid(grid =>
                                        {
                                            grid.VerticalSpacing(10);
                                            grid.HorizontalSpacing(10);
                                            grid.AlignCenter();
                                            grid.Columns(10); // 8 columnas para mejor distribución

                                            //var primeraFilaItems = inspeccionesConImagen.Take(4);
                                            var imagenesFila = inspeccionesConImagen.Skip(i).Take(2).ToList();

                                            foreach (var ins in imagenesFila)
                                            {
                                                string parametro = (Idioma == "I" ?
                                                    ins.ParametroInspeccion.ParametroIngles :
                                                    ins.ParametroInspeccion.Parametro);

                                                grid.Item(5).Border(1).ShowEntire().BorderColor(Colors.Grey.Lighten1)
                                                    .Background(Colors.White).Column(column =>
                                                    {
                                                        // Título en la parte superior con altura fija
                                                        column.Item().Height(25).AlignCenter().AlignMiddle()
                                                            .Background(Colors.Grey.Lighten4)
                                                            .Padding(2)
                                                            .Text(parametro)
                                                            .FontFamily(Fonts.Arial)
                                                            .FontSize(8)
                                                            .Bold();

                                                        // Imagen en la parte inferior con altura fija
                                                        column.Item().AlignCenter().AlignMiddle()
                                                            .Padding(3)
                                                            .Image(HelpImage.GetFileContent(ins.PathImage));
                                                    });
                                            }
                                        });

                                    }

                                    // Primera fila - máximo 4 imágenes

                                }
                            }

                        });
                        #endregion

                        #region SEC - FOOTER
                        page.Footer().AlignCenter().Border(0).Row(row =>
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
                                   string FOOTSECTION_QUALITY_ENGINEER = pieza.UsuarioCompania == 1003 ? "QUALITY SUPERVISOR" : "QUALITY ENGINEER";
                                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(QUALITY_ENGINEER).FontFamily(Fonts.Arial).FontSize(9);
                                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text("___________________").FontFamily(Fonts.Arial).FontSize(9);
                                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text("CHECKED").FontFamily(Fonts.Arial).FontSize(9);

                                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(FOOTSECTION_QUALITY_ENGINEER).FontFamily(Fonts.Arial).FontSize(9).Bold();
                               });
                            row.ConstantItem(175).Background(Colors.White).Border(0).Height(50).AlignMiddle().AlignCenter()
                               .Table(table =>
                               {
                                   table.ColumnsDefinition(columns =>
                                   {
                                       columns.RelativeColumn(100);

                                   });

                                   string FOOTSECTION_QUALITY_MANAGER = pieza.UsuarioCompania == 1003 ? "QUALITY COORDINATOR" : "QUALITY MANAGER";

                                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(QUALITY_MANAGER).FontFamily(Fonts.Arial).FontSize(9);
                                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text("___________________").FontFamily(Fonts.Arial).FontSize(9);
                                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text("AUTHORIZED").FontFamily(Fonts.Arial).FontSize(9);
                                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(FOOTSECTION_QUALITY_MANAGER).FontFamily(Fonts.Arial).FontSize(9).Bold();
                               });


                        });
                        #endregion
                    });

                });


                // instead of the standard way of generating a PDF file
                document.GeneratePdf(fileName);

            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            return fileName;
        }

        #endregion

        #region Certificados Estandar
        public async Task<string> CertificadoPDFIngles(string Idioma, CertificadoIf certificado, PiezaSap pieza,
        List<CertificadoIfdimension> DIMENSIONAL_RESULT, List<CertificadoIfapariencias> APARIENCIA_RESULT,
        List<InspeccionOptica> INSPECCIONES_OPTICAS, List<PiezaConcesion> OBSERVACIONES)
        {
            var zferSentinel = new HashSet<string>
            {
                "700167179",
                "700167180",
                "700165336",
                "700165337",
                "700164766",
            };
            pieza.Zfer = pieza.Zfer.Substring(9);
            bool esSentinel = pieza.Zfer != null && zferSentinel.Contains(pieza.Zfer.Trim());

            if (esSentinel)
            {
                return await CertificadoPDFSglassSentinel(Idioma, certificado, pieza,
                    DIMENSIONAL_RESULT, APARIENCIA_RESULT, INSPECCIONES_OPTICAS, OBSERVACIONES);
            }

            return await CertificadoPDFSglassInglesEstandar(Idioma, certificado, pieza,
                DIMENSIONAL_RESULT, APARIENCIA_RESULT, INSPECCIONES_OPTICAS, OBSERVACIONES);
        }
        public async Task<string> CertificadoPDFSglassInglesEstandar(string Idioma,CertificadoIf certificado,PiezaSap pieza, List<CertificadoIfdimension> DIMENSIONAL_RESULT, List<CertificadoIfapariencias> APARIENCIA_RESULT, List<InspeccionOptica> INSPECCIONES_OPTICAS, List<PiezaConcesion> OBSERVACIONES)
        {
            string UrlImageGordon = "http://4.228.184.32:8081/Userimage/";
            string fileName = "Reports/Results/" + certificado.Id.ToString() +"/" + pieza.LoteLogistico + ".pdf";
            try
            {
                #region SEC-1 CABECERA CERTIFICADO INFO
                string CERTIFICATE_NUMBER = certificado.Id.ToString();
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

                DIMENSIONAL_RESULT = await LimitLenCharactersToCellPdf(DIMENSIONAL_RESULT);

                string currentPage = "";
                #region SEC-2 IMAGEN TECNICA
                
                /*
                string IMAGEN_TECNICA = UrlImageGordon + pieza.IdCompania + "/GraficoExterno/"+(pieza.IdCompania == 1006 ? pieza.GetPlantNameOrigen() : "") + "/" + pieza.CodigoImagenTecnica + ".jpg";
                string IMAGEN_PLANO_STANDAR = UrlImageGordon + pieza.IdCompania + "/GraficoExterno/" + pieza.CodigoImagenStandar + ".jpg";
                byte[] BYTE_IMAGEN_TECNICA = await _HelpImage.ConvertImageUrlToByte(IMAGEN_TECNICA);
                byte[] BYTE_IMAGEN_PLANO_STANDAR = await _HelpImage.ConvertImageUrlToByte(IMAGEN_PLANO_STANDAR);
                byte[] BYTE_IMAGEN_DEFECTOS =(pieza.DefectoImagenByte!=null)?_HelpImage.ResizeImage(pieza.DefectoImagenByte, 220, 145):null;
                if (BYTE_IMAGEN_TECNICA != null) BYTE_IMAGEN_TECNICA=_HelpImage.ResizeImage(BYTE_IMAGEN_TECNICA, 220, 145);
                if (BYTE_IMAGEN_PLANO_STANDAR != null) BYTE_IMAGEN_PLANO_STANDAR =_HelpImage.ResizeImage(BYTE_IMAGEN_PLANO_STANDAR, 220, 145);
                */

                #endregion



                #region SEC - FOOTER
                string INSPECTOR = pieza.UsuarioCrea;
                string QUALITY_ENGINEER = pieza.GetQualityEngineer();
                string QUALITY_MANAGER = pieza.GetQualityManager();
                #endregion

                //QuestPDF.Settings.License = LicenseType.Community;

                // code in your main method
                var document = QuestPDF.Fluent.Document.Create(container =>
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
                            //FileStream fs = GetFileStrem("src/logo.jpg");
                            FileStream fs = File.Open("src/logo.jpg", FileMode.Open);

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
                                QuestPDF.Infrastructure.IContainer DefaultCellStyle(QuestPDF.Infrastructure.IContainer container, string backgroundColor)
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
                                table.Cell().Element(CellStyle).Text("2").FontFamily(Fonts.Arial).FontSize(8);

                                table.Cell().Element(CellStyle).Text("FECHA / DATE").FontFamily(Fonts.Arial).FontSize(8);
                                table.Cell().Element(CellStyle).Text(DateTime.Now.ToString("dd/MM/yyyy")).FontFamily(Fonts.Arial).FontSize(8);

                                table.Cell().Element(CellStyle).Text("HOJA/SHEET").FontFamily(Fonts.Arial).FontSize(8);
                                table.Cell().Element(CellStyle).Text(text => {

                                    text.CurrentPageNumber().FontFamily(Fonts.Arial).FontSize(8);
                                    //currentPage = text.CurrentPageNumber().ToString();
                                    //var currentPage = text.CurrentPageNumber().ToString();
                                    //text.Span(currentPage).FontFamily(Fonts.Arial).FontSize(8);
                                });


                                QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                            });
                            fs.Dispose();
                            fs.Close();
                        });
                        #endregion

                        page.Content().PaddingVertical(3, Unit.Millimetre)
                        .Column(column =>
                        {
                            #region SEC-1 CABECERA CERTIFICADO INFO
                            column.Item().Row(row =>
                            {
                                row.ConstantItem(538).Background(Colors.White).Border(0).Table(table =>
                                {
                                    QuestPDF.Infrastructure.IContainer DefaultCellStyle(QuestPDF.Infrastructure.IContainer container, string backgroundColor)
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

                                    QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                                });


                            });
                            #endregion

                            column.Item().PaddingTop(2).Text("");
                    
                            
                            #region SEC-2 IMAGEN TECNICA
                            
                            column.Item().Border(0).Width(538).Height(150).Row(row =>
                            {
                                
                                if (pieza.ImagenFt != null && pieza.ImagenFt!="")
                                    row.ConstantItem(269).Border(1).AlignCenter().Width(220).Background(Colors.White).AlignMiddle()
                                   .Image(HelpImage.GetFileContent(pieza.ImagenFt));
                                else
                                    row.ConstantItem(269).Border(1).AlignCenter().Width(220).Background(Colors.White).AlignMiddle()
                                   .Text("");
                                
                                if (pieza.IMAGEN_PLANO_STANDAR != null && pieza.IMAGEN_PLANO_STANDAR != "")
                                    row.ConstantItem(269).Border(1).AlignCenter().Width(220).Background(Colors.White).AlignMiddle()
                                    .Image(HelpImage.GetFileContent(pieza.IMAGEN_PLANO_STANDAR));
                                else
                                    row.ConstantItem(269).Border(1).AlignCenter().Width(220).Background(Colors.White).AlignMiddle()
                                    .Text("");
                                
                            });
                            

                            #endregion
                            // column.Item().Width(100).Height(200).Image(STREAM_IMAGEN_TECNICA);
                            column.Spacing(5);
                            column.Item().AlignCenter().Text("Dimensional Results");
                            column.Spacing(5);

                            #region SEC-3 DIMENSIONAL RESULT

                            if (DIMENSIONAL_RESULT.Where(x => x.Parametro.ToUpper().Contains("CURVATURE") || x.Parametro.ToUpper().Contains("CURVATURA")).Count() > 0)
                            {
                                column.Item().Row(row =>
                                {

                                    row.ConstantItem(538).AlignMiddle().Border(1).Table(table =>
                                    {
                                        QuestPDF.Infrastructure.IContainer DefaultCellStyle(QuestPDF.Infrastructure.IContainer container, string backgroundColor)
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

                                        foreach (CertificadoIfdimension param in DIMENSIONAL_RESULT.Where(x => x.Parametro.ToUpper().Contains("CURVATURE") || x.Parametro.ToUpper().Contains("CURVATURA")))
                                        {
                                            string parametro = (Idioma == "I" ? param.ParametroInspeccion.ParametroIngles: param.Parametro);
                                            table.Cell().Element(CellStyle).Text(parametro).FontFamily(Fonts.Arial).FontSize(8).Bold();

                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val1).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val2).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val3).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val4).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val5).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val6).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val7).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val8).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val9).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val10).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val11).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val12).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val13).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val14).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val15).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val16).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val17).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val18).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val19).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val20).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                        }

                                        QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                                    });
                                });

                            }
                            column.Spacing(5);
                            column.Item().Row(row =>
                            {

                                row.ConstantItem(538).Background(Colors.White).Border(1).Table(table =>
                                {
                                    QuestPDF.Infrastructure.IContainer DefaultCellStyle(QuestPDF.Infrastructure.IContainer container, string backgroundColor)
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

                                    foreach (CertificadoIfdimension param in DIMENSIONAL_RESULT.Where(x => !x.Parametro.ToUpper().Contains("CURVATURE") && !x.Parametro.ToUpper().Contains("CURVATURA")))
                                    {
                                        string parametro = (Idioma == "I" ? param.ParametroInspeccion.ParametroIngles : param.Parametro);
                                        table.Cell().Element(CellStyle).Text(parametro).FontFamily(Fonts.Arial).FontSize(8).Bold();
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val1).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val2).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val3).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val4).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val5).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val6).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val7).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val8).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val9).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val10).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val11).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val12).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val13).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val14).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val15).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val16).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val17).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val18).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val19).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val20).FontFamily(Fonts.Arial).FontSize(7);
                                    }


                                    QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                                });
                            });

                            #endregion

                            column.Spacing(5);
                            column.Item().PaddingTop(0).AlignCenter().Text("Appearance Results");
                            column.Spacing(5);



                            #region SEC-4 APARIENCIA RESULT
                            
                            column.Item().AlignCenter().Row(row =>
                            {

                                row.ConstantItem(538).AlignCenter().Background(Colors.White).Border(1).Table(table =>
                                {
                                    QuestPDF.Infrastructure.IContainer DefaultCellStyle(QuestPDF.Infrastructure.IContainer container, string backgroundColor)
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

                                    foreach (CertificadoIfapariencias apariencia in APARIENCIA_RESULT)
                                    {
                                        string parametro = (Idioma == "I" ? apariencia.ParametroInspeccion.ParametroIngles : apariencia.Parametro);
                                        table.Cell().Element(CellStyle).Text(parametro).FontFamily(Fonts.Arial).FontSize(10).Bold();
                                        string resultado = "";
                                        switch (apariencia.Valor.ToUpper())
                                        {
                                            case "CUMPLE":
                                                resultado = "OK";
                                                break;
                                            case "NO CUMPLE":
                                                resultado = "NOK";
                                                break;
                                            default:
                                                resultado = apariencia.Valor;
                                                break;
                                        }
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(resultado).FontFamily(Fonts.Arial).FontSize(10);
                                    }


                                    QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                                });
                            });
                            
                            #endregion


                            #region SEC-4.1 Defectos
                            
                            if (OBSERVACIONES.Count > 0)
                            {
                                column.Item().Border(0).Width(538).AlignMiddle().Height(150).Row(row =>
                                {

                                    if (pieza.DefectoImagen != null && pieza.DefectoImagen != "")
                                        row.ConstantItem(269).Border(0).AlignCenter().Width(220).Background(Colors.White).AlignMiddle()
                                        .Image(HelpImage.GetFileContent(pieza.DefectoImagen));
                                    else
                                        row.ConstantItem(269).Border(0).AlignCenter().Width(220).Background(Colors.White).AlignMiddle()
                                        .Text("");

                                    row.ConstantItem(260).Height(150).AlignMiddle().Border(1).Table(table =>
                                    {
                                        QuestPDF.Infrastructure.IContainer DefaultCellStyle(QuestPDF.Infrastructure.IContainer container, string backgroundColor)
                                        {
                                            return container
                                                //.Height(5)
                                                .Border(1)
                                                .BorderColor(Colors.Grey.Lighten1)
                                                .Background(backgroundColor)
                                                .PaddingVertical(1)
                                                .PaddingHorizontal(1)
                                                .AlignCenter()
                                                .AlignMiddle();
                                        }

                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.ConstantColumn(30);
                                            columns.ConstantColumn(30);
                                            columns.ConstantColumn(120);
                                            columns.ConstantColumn(80);

                                        });

                                        table.Header(header =>
                                        {
                                            // please be sure to call the 'header' handler!
                                            header.Cell().Element(CellStyle).Text("ITEM").FontFamily(Fonts.Arial).FontSize(8).Bold();
                                            header.Cell().Element(CellStyle).Text("ZONE").FontFamily(Fonts.Arial).FontSize(8).Bold();
                                            header.Cell().Element(CellStyle).Text("DEFECT").FontFamily(Fonts.Arial).FontSize(8).Bold();
                                            header.Cell().Element(CellStyle).Text("SIZE").FontFamily(Fonts.Arial).FontSize(8).Bold();

                                            // you can extend existing styles by creating additional methods
                                        });

                                        int index = 0;
                                        int maxRows = 10;
                                        foreach (PiezaConcesion obs in OBSERVACIONES.Take(maxRows))
                                        {
                                            index++;
                                       
                                            // ITEM
                                            table.Cell().Element(DataCellStyle).Text(index.ToString())
                                                .FontFamily(Fonts.Arial).FontSize(7);

                                            // ZONE  
                                            table.Cell().Element(DataCellStyle).Text(obs.Zona ?? "")
                                                .FontFamily(Fonts.Arial).FontSize(7);

                                            // DEFECT - Con manejo de texto largo
                                            table.Cell().Element(DataCellStyle).Text(obs.DefectoMaestro?.NombreIngles ?? "")
                                                .FontFamily(Fonts.Arial).FontSize(7).LineHeight(0.9f);

                                            // SIZE
                                            table.Cell().Element(DataCellStyle).Text(obs.Tamanio ?? null)
                                                .FontFamily(Fonts.Arial).FontSize(7);
                                        }

                                        // Estilos de celda
                                        QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container) =>
                                            DefaultCellStyle(container, Colors.Grey.Lighten3);

                                        QuestPDF.Infrastructure.IContainer DataCellStyle(QuestPDF.Infrastructure.IContainer container) =>
                                            DefaultCellStyle(container, Colors.White);

                                        //QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                                    });

                                });

                            }
                            
                            #endregion

                            
                           
                            if (OBSERVACIONES.Count==0)
                            {
                                column.Item().PageBreak();
                            }
                            
                            column.Spacing(10);

                            #region SEC-5 OPTICAL INSPECTION

                            if (INSPECCIONES_OPTICAS.Count > 0)
                            {
                                // Filtrar solo las inspecciones que tienen imagen
                                var inspeccionesConImagen = INSPECCIONES_OPTICAS
                                    .Where(x => !string.IsNullOrEmpty(x.PathImage))
                                    .ToList();

                                
                                if (inspeccionesConImagen.Count > 0)
                                {

                                    for (int i = 0; i < inspeccionesConImagen.Count; i+=2)
                                    {
                                        column.Item().Grid(grid =>
                                        {
                                            grid.VerticalSpacing(10);
                                            grid.HorizontalSpacing(10);
                                            grid.AlignCenter();
                                            grid.Columns(10); // 8 columnas para mejor distribución

                                            //var primeraFilaItems = inspeccionesConImagen.Take(4);
                                            var imagenesFila = inspeccionesConImagen.Skip(i).Take(2).ToList();

                                            foreach (var ins in imagenesFila)
                                            {
                                                string parametro = (Idioma == "I" ?
                                                    ins.ParametroInspeccion.ParametroIngles :
                                                    ins.ParametroInspeccion.Parametro);

                                                grid.Item(5).Border(1).ShowEntire().BorderColor(Colors.Grey.Lighten1)
                                                    .Background(Colors.White).Column(column =>
                                                    {
                                                        // Título en la parte superior con altura fija
                                                        column.Item().Height(25).AlignCenter().AlignMiddle()
                                                            .Background(Colors.Grey.Lighten4)
                                                            .Padding(2)
                                                            .Text(parametro)
                                                            .FontFamily(Fonts.Arial)
                                                            .FontSize(8)
                                                            .Bold();

                                                        // Imagen en la parte inferior con altura fija
                                                        column.Item().AlignCenter().AlignMiddle()
                                                            .Padding(3)
                                                            .Image(HelpImage.GetFileContent(ins.PathImage));
                                                    });
                                            }
                                        });

                                    }

                                    // Primera fila - máximo 4 imágenes

                                    }
                            }
                      
                        });
                        #endregion

                        #region SEC - FOOTER
                        page.Footer().AlignCenter().Border(0).Row(row =>
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
                                   string FOOTSECTION_QUALITY_ENGINEER = pieza.UsuarioCompania == 1003 ? "QUALITY SUPERVISOR" : "QUALITY ENGINEER";
                                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(QUALITY_ENGINEER).FontFamily(Fonts.Arial).FontSize(9);
                                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text("___________________").FontFamily(Fonts.Arial).FontSize(9);
                                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text("CHECKED").FontFamily(Fonts.Arial).FontSize(9);

                                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(FOOTSECTION_QUALITY_ENGINEER).FontFamily(Fonts.Arial).FontSize(9).Bold();
                               });
                            row.ConstantItem(175).Background(Colors.White).Border(0).Height(50).AlignMiddle().AlignCenter()
                               .Table(table =>
                               {
                                   table.ColumnsDefinition(columns =>
                                   {
                                       columns.RelativeColumn(100);

                                   });
                                   
                                   string FOOTSECTION_QUALITY_MANAGER = pieza.UsuarioCompania== 1003 ? "QUALITY COORDINATOR" : "QUALITY MANAGER";

                                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(QUALITY_MANAGER).FontFamily(Fonts.Arial).FontSize(9);
                                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text("___________________").FontFamily(Fonts.Arial).FontSize(9);
                                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text("AUTHORIZED").FontFamily(Fonts.Arial).FontSize(9);
                                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(FOOTSECTION_QUALITY_MANAGER).FontFamily(Fonts.Arial).FontSize(9).Bold();
                               });


                        });
                        #endregion
                    });

                });


                // instead of the standard way of generating a PDF file
                document.GeneratePdf(fileName);
                
            }

            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
         
            
            return fileName;
        }

        public async Task<string> CertificadoPDFSglassEspanol(string Idioma, CertificadoIf certificado, PiezaSap pieza, List<CertificadoIfdimension> DIMENSIONAL_RESULT, List<CertificadoIfapariencias> APARIENCIA_RESULT, List<InspeccionOptica> INSPECCIONES_OPTICAS, List<PiezaConcesion> OBSERVACIONES)
        {
            string UrlImageGordon = "http://4.228.184.32:8081/Userimage/";
            string fileName = "Reports/Results/" + certificado.Id.ToString() + "/" + pieza.LoteLogistico + ".pdf";
            try
            {
                #region SEC-1 CABECERA CERTIFICADO INFO
                string CERTIFICATE_NUMBER = certificado.Id.ToString();
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


                DIMENSIONAL_RESULT = await LimitLenCharactersToCellPdf(DIMENSIONAL_RESULT);


                #region SEC-2 IMAGEN TECNICA

                /*
                string IMAGEN_TECNICA = UrlImageGordon + pieza.IdCompania + "/GraficoExterno/"+(pieza.IdCompania == 1006 ? pieza.GetPlantNameOrigen() : "") + "/" + pieza.CodigoImagenTecnica + ".jpg";
                string IMAGEN_PLANO_STANDAR = UrlImageGordon + pieza.IdCompania + "/GraficoExterno/" + pieza.CodigoImagenStandar + ".jpg";
                byte[] BYTE_IMAGEN_TECNICA = await _HelpImage.ConvertImageUrlToByte(IMAGEN_TECNICA);
                byte[] BYTE_IMAGEN_PLANO_STANDAR = await _HelpImage.ConvertImageUrlToByte(IMAGEN_PLANO_STANDAR);
                byte[] BYTE_IMAGEN_DEFECTOS =(pieza.DefectoImagenByte!=null)?_HelpImage.ResizeImage(pieza.DefectoImagenByte, 220, 145):null;
                if (BYTE_IMAGEN_TECNICA != null) BYTE_IMAGEN_TECNICA=_HelpImage.ResizeImage(BYTE_IMAGEN_TECNICA, 220, 145);
                if (BYTE_IMAGEN_PLANO_STANDAR != null) BYTE_IMAGEN_PLANO_STANDAR =_HelpImage.ResizeImage(BYTE_IMAGEN_PLANO_STANDAR, 220, 145);
                */

                #endregion



                #region SEC - FOOTER
                string INSPECTOR = pieza.UsuarioCrea;
                string QUALITY_ENGINEER = pieza.GetQualityEngineer();
                string QUALITY_MANAGER = pieza.GetQualityManager();
                #endregion

                //QuestPDF.Settings.License = LicenseType.Community;

                // code in your main method
                var document = QuestPDF.Fluent.Document.Create(container =>
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
                            //FileStream fs = GetFileStrem("src/logo.jpg");
                            FileStream fs = File.Open("src/logo.jpg", FileMode.Open);

                            row.ConstantItem(100).Background(Colors.White).Border(1).AlignMiddle()
                            .Image(fs); //.Image("logo.jpg");

                            row.ConstantItem(288).Background(Colors.White).Border(1).AlignMiddle().AlignCenter().Text(
                                text =>
                                {
                                    text.Span("CERTIFICADO CALIDAD - INSPECCIÓN FINAL").FontFamily(Fonts.Arial).FontSize(14).FontColor(Colors.Black).Bold();
                                    text.EmptyLine();
                                    text.Span("DEPARTAMENTO DE CONTROL DE CALIDAD").FontFamily(Fonts.Arial).FontSize(10).FontColor(Colors.Black).Bold();
                                });
                            row.ConstantItem(150).Background(Colors.White).Border(1).AlignCenter().AlignMiddle().Background(Colors.Yellow.Medium).Table(table =>
                            {
                                QuestPDF.Infrastructure.IContainer DefaultCellStyle(QuestPDF.Infrastructure.IContainer container, string backgroundColor)
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
                                table.Cell().Element(CellStyle).Text("2").FontFamily(Fonts.Arial).FontSize(8);

                                table.Cell().Element(CellStyle).Text("FECHA / DATE").FontFamily(Fonts.Arial).FontSize(8);
                                table.Cell().Element(CellStyle).Text(DateTime.Now.ToString("dd/MM/yyyy")).FontFamily(Fonts.Arial).FontSize(8);

                                table.Cell().Element(CellStyle).Text("HOJA/SHEET").FontFamily(Fonts.Arial).FontSize(8);
                                table.Cell().Element(CellStyle).Text(text => {

                                    text.CurrentPageNumber().FontFamily(Fonts.Arial).FontSize(8);
                                    //var currentPage = text.CurrentPageNumber().ToString();
                                    //text.Span(currentPage).FontFamily(Fonts.Arial).FontSize(8);
                                });


                                QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                            });
                            fs.Dispose();
                            fs.Close();
                        });
                        #endregion

                        page.Content().PaddingVertical(3, Unit.Millimetre)
                        .Column(column =>
                        {
                            #region SEC-1 CABECERA CERTIFICADO INFO
                            column.Item().Row(row =>
                            {
                                row.ConstantItem(538).Background(Colors.White).Border(0).Table(table =>
                                {
                                    QuestPDF.Infrastructure.IContainer DefaultCellStyle(QuestPDF.Infrastructure.IContainer container, string backgroundColor)
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
                                        header.Cell().Text("N° CERTIFICADO:").FontFamily(Fonts.Arial).FontSize(9).Bold();
                                        header.Cell().Text(CERTIFICATE_NUMBER).FontFamily(Fonts.Arial).FontSize(9).Bold();
                                        header.Cell().Text("").FontFamily(Fonts.Arial).FontSize(8);
                                        header.Cell().Text("").FontFamily(Fonts.Arial).FontSize(8);
                                        header.Cell().Text("").FontFamily(Fonts.Arial).FontSize(8);
                                        header.Cell().Text("").FontFamily(Fonts.Arial).FontSize(8);
                                        // you can extend existing styles by creating additional methods
                                    });

                                    table.Cell().Text("PROVEEDOR:").FontFamily(Fonts.Arial).FontSize(8).Bold();
                                    table.Cell().AlignLeft().Text(SUPPLIER).FontFamily(Fonts.Arial).FontSize(8);

                                    table.Cell().Text("DIRECCIÓN:").FontFamily(Fonts.Arial).FontSize(8).Bold();
                                    table.Cell().ColumnSpan(3).AlignLeft().Text(SUPPLIER_ADDRESS).FontFamily(Fonts.Arial).FontSize(8);


                                    table.Cell().Text("CLIENTE:").FontFamily(Fonts.Arial).FontSize(8).Bold();
                                    table.Cell().Text(CLIENT).FontFamily(Fonts.Arial).FontSize(8);
                                    table.Cell().Text("ORDEN PROD:").FontFamily(Fonts.Arial).FontSize(8).Bold();
                                    table.Cell().Text(PRODUCTION_ORDER).FontFamily(Fonts.Arial).FontSize(8);
                                    table.Cell().Text("COLOR:").FontFamily(Fonts.Arial).FontSize(8).Bold();
                                    table.Cell().Text(COLOR).FontFamily(Fonts.Arial).FontSize(8);

                                    table.Cell().Text("VEHICULO:").FontFamily(Fonts.Arial).FontSize(8).Bold();
                                    table.Cell().Text(VEHICLE).FontFamily(Fonts.Arial).FontSize(8);
                                    table.Cell().Text("COMPOSICIÓN:").FontFamily(Fonts.Arial).FontSize(8).Bold();
                                    table.Cell().Text(COMPOSITION).FontFamily(Fonts.Arial).FontSize(8);
                                    table.Cell().Text("ESPESOR(mm):").FontFamily(Fonts.Arial).FontSize(8).Bold();
                                    table.Cell().Text(THICKNESS).FontFamily(Fonts.Arial).FontSize(8);

                                    table.Cell().Text("AQL:").FontFamily(Fonts.Arial).FontSize(8).Bold();
                                    table.Cell().Text(AQL).FontFamily(Fonts.Arial).FontSize(8);
                                    table.Cell().Text("TAMAÑO:").FontFamily(Fonts.Arial).FontSize(8).Bold();
                                    table.Cell().Text(SAMPLE_SIZE).FontFamily(Fonts.Arial).FontSize(8);
                                    table.Cell().Text("LOTE:").FontFamily(Fonts.Arial).FontSize(8).Bold();
                                    table.Cell().Text(PRODUCTION_LOT).FontFamily(Fonts.Arial).FontSize(8);

                                    QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                                });


                            });
                            #endregion

                            column.Item().PaddingTop(2).Text("");

                            //column.Item().Border(1).Width(100).Height(200).Image(STREAM_IMAGEN_TECNICA);
                            #region SEC-2 IMAGEN TECNICA

                            column.Item().Border(0).Width(538).Height(150).Row(row =>
                            {

                                if (pieza.ImagenFt != null && pieza.ImagenFt != "")
                                    row.ConstantItem(269).Border(1).AlignCenter().Width(220).Background(Colors.White).AlignMiddle()
                                   .Image(HelpImage.GetFileContent(pieza.ImagenFt));
                                else
                                    row.ConstantItem(269).Border(1).AlignCenter().Width(220).Background(Colors.White).AlignMiddle()
                                   .Text("");

                                if (pieza.IMAGEN_PLANO_STANDAR != null && pieza.IMAGEN_PLANO_STANDAR!="")
                                    row.ConstantItem(269).Border(1).AlignCenter().Width(220).Background(Colors.White).AlignMiddle()
                                    .Image(HelpImage.GetFileContent(pieza.IMAGEN_PLANO_STANDAR));
                                else
                                    row.ConstantItem(269).Border(1).AlignCenter().Width(220).Background(Colors.White).AlignMiddle()
                                    .Text("");

                            });


                            #endregion
                            // column.Item().Width(100).Height(200).Image(STREAM_IMAGEN_TECNICA);
                            column.Spacing(5);
                            column.Item().AlignCenter().Text("Dimensional");
                            column.Spacing(5);

                            #region SEC-3 DIMENSIONAL RESULT

                            if (DIMENSIONAL_RESULT.Where(x => x.Parametro.ToUpper().Contains("CURVATURE") || x.Parametro.ToUpper().Contains("CURVATURA")).Count() > 0)
                            {
                                column.Item().Row(row =>
                                {

                                    row.ConstantItem(538).AlignMiddle().Border(1).Table(table =>
                                    {
                                        QuestPDF.Infrastructure.IContainer DefaultCellStyle(QuestPDF.Infrastructure.IContainer container, string backgroundColor)
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
                                            header.Cell().Element(CellStyle).Text("CARACTERISTICA").FontFamily(Fonts.Arial).FontSize(10).Bold();
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

                                        foreach (CertificadoIfdimension param in DIMENSIONAL_RESULT.Where(x => x.Parametro.ToUpper().Contains("CURVATURE") || x.Parametro.ToUpper().Contains("CURVATURA")))
                                        {
                                            string parametro = (Idioma == "I" ? param.ParametroInspeccion.ParametroIngles : param.Parametro);
                                            table.Cell().Element(CellStyle).Text(parametro).FontFamily(Fonts.Arial).FontSize(8).Bold();

                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val1).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val2).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val3).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val4).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val5).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val6).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val7).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val8).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val9).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val10).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val11).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val12).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val13).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val14).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val15).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val16).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val17).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val18).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val19).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val20).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                        }

                                        QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                                    });
                                });

                            }
                            column.Spacing(5);
                            column.Item().Row(row =>
                            {

                                row.ConstantItem(538).Background(Colors.White).Border(1).Table(table =>
                                {
                                    QuestPDF.Infrastructure.IContainer DefaultCellStyle(QuestPDF.Infrastructure.IContainer container, string backgroundColor)
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
                                        header.Cell().Element(CellStyle).Text("CARACTERISTICA").FontFamily(Fonts.Arial).FontSize(10).Bold();
                                        for (int i = 1; i <= 20; i++)
                                        {
                                            header.Cell().Element(CellStyle).Text(i.ToString()).FontFamily(Fonts.Arial).FontSize(9).Bold();
                                        }
                                        // you can extend existing styles by creating additional methods
                                    });

                                    foreach (CertificadoIfdimension param in DIMENSIONAL_RESULT.Where(x => !x.Parametro.ToUpper().Contains("CURVATURE") && !x.Parametro.ToUpper().Contains("CURVATURA")))
                                    {
                                        string parametro = (Idioma == "I" ? param.ParametroInspeccion.ParametroIngles : param.Parametro);
                                        table.Cell().Element(CellStyle).Text(parametro).FontFamily(Fonts.Arial).FontSize(8).Bold();
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val1).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val2).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val3).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val4).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val5).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val6).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val7).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val8).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val9).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val10).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val11).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val12).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val13).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val14).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val15).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val16).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val17).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val18).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val19).FontFamily(Fonts.Arial).FontSize(7);
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(param.Val20).FontFamily(Fonts.Arial).FontSize(7);
                                    }


                                    QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                                });
                            });

                            #endregion

                            column.Spacing(5);
                            column.Item().PaddingTop(0).AlignCenter().Text("Apariencia");
                            column.Spacing(5);



                            #region SEC-4 APARIENCIA RESULT

                            column.Item().AlignCenter().Row(row =>
                            {

                                row.ConstantItem(538).AlignCenter().Background(Colors.White).Border(1).Table(table =>
                                {
                                    QuestPDF.Infrastructure.IContainer DefaultCellStyle(QuestPDF.Infrastructure.IContainer container, string backgroundColor)
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

                                    foreach (CertificadoIfapariencias apariencia in APARIENCIA_RESULT)
                                    {
                                        string parametro = (Idioma == "I" ? apariencia.ParametroInspeccion.ParametroIngles : apariencia.Parametro);
                                        table.Cell().Element(CellStyle).Text(parametro).FontFamily(Fonts.Arial).FontSize(10).Bold();
                                        table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(apariencia.Valor).FontFamily(Fonts.Arial).FontSize(10);
                                    }


                                    QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                                });
                            });

                            #endregion


                            #region SEC-4.1 Defectos
                            if (OBSERVACIONES.Count > 0)
                            {
                                column.Item().Border(0).Width(538).AlignMiddle().Height(150).Row(row =>
                                {

                                    if (pieza.DefectoImagen != null && pieza.DefectoImagen != "")
                                        row.ConstantItem(269).Border(0).AlignCenter().Width(220).Background(Colors.White).AlignMiddle()
                                        .Image(HelpImage.GetFileContent(pieza.DefectoImagen));
                                    else
                                        row.ConstantItem(269).Border(0).AlignCenter().Width(220).Background(Colors.White).AlignMiddle()
                                        .Text("");

                                    row.ConstantItem(260).Height(150).AlignMiddle().Border(1).Table(table =>
                                    {
                                        QuestPDF.Infrastructure.IContainer DefaultCellStyle(QuestPDF.Infrastructure.IContainer container, string backgroundColor)
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
                                            columns.RelativeColumn(15);
                                            columns.RelativeColumn(15);
                                            columns.RelativeColumn(40);
                                            columns.RelativeColumn(65);
                                            columns.RelativeColumn(15);

                                        });

                                        table.Header(header =>
                                        {
                                            // please be sure to call the 'header' handler!
                                            header.Cell().Element(CellStyle).Text("ITEM").FontFamily(Fonts.Arial).FontSize(9).Bold();
                                            header.Cell().Element(CellStyle).Text("ZONA").FontFamily(Fonts.Arial).FontSize(9).Bold();
                                            header.Cell().Element(CellStyle).Text("DEFECTO").FontFamily(Fonts.Arial).FontSize(9).Bold();
                                            header.Cell().Element(CellStyle).Text("DESCRIPCIÓN").FontFamily(Fonts.Arial).FontSize(9).Bold();
                                            header.Cell().Element(CellStyle).Text("TAMAÑO").FontFamily(Fonts.Arial).FontSize(9).Bold();

                                            // you can extend existing styles by creating additional methods
                                        });

                                        int index = 0;
                                        foreach (PiezaConcesion obs in OBSERVACIONES)
                                        {
                                            index++;
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(index.ToString()).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(obs.Zona).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(obs.DefectoMaestro.Defecto).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(obs.Observacion).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                            table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(obs.Tamanio).FontFamily(Fonts.Arial).FontSize(7).Bold();

                                        }

                                        QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                                    });

                                });

                            }

                            #endregion

                            if (OBSERVACIONES.Count == 0)
                            {
                                column.Item().PageBreak();
                            }

                            column.Spacing(10);

                            #region SEC-5 OPTICAL INSPECTION

                            if (INSPECCIONES_OPTICAS.Count > 0)
                            {
                                //column.Item().PageBreak();
                                column.Item().Grid(grid =>
                                {
                                    grid.VerticalSpacing(15);
                                    grid.HorizontalSpacing(15);
                                    grid.AlignCenter();
                                    grid.Columns(10); // 12 by default

                                    for (int i = 1; i <= INSPECCIONES_OPTICAS.Count; i++)
                                    {
                                        var ins = INSPECCIONES_OPTICAS[i - 1];
                                        string parametro = (Idioma == "I" ? ins.ParametroInspeccion.ParametroIngles : ins.ParametroInspeccion.Parametro);
                                        if (ins.PathImage != "" && ins.PathImage != null)
                                            grid.Item(5).Background(Colors.White).Table(table =>
                                            {
                                                table.ColumnsDefinition(columns =>
                                                {
                                                    columns.RelativeColumn(50);

                                                });
                                                /*
                                                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(text =>
                                                 {
                                                     text.DefaultTextStyle(x => x.FontFamily(Fonts.Arial).FontSize(8) );
                                                     text.Span(ins.Parametro);
                                                     text.EmptyLine();
                                                     text.Element().
                                                         .Image(ins.PathImage);

                                                 });*/

                                                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(parametro).FontFamily(Fonts.Arial).FontSize(8);
                                                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Image(HelpImage.GetFileContent(ins.PathImage));

                                            });
                                        if (i == 4 && INSPECCIONES_OPTICAS.Count > 4) { break; }
                                    }
                                });
                            }



                            if (INSPECCIONES_OPTICAS.Where(x => x.PathImage != "" && x.PathImage != null).ToList().Count > 4)
                            {
                                //  column.Item().PageBreak();
                                column.Item().Grid(grid =>
                                {
                                    grid.VerticalSpacing(15);
                                    grid.HorizontalSpacing(15);
                                    grid.AlignCenter();
                                    grid.Columns(10); // 12 by default

                                    for (int i = 5; i <= INSPECCIONES_OPTICAS.Count; i++)
                                    {
                                        var ins = INSPECCIONES_OPTICAS[i - 1];

                                        string parametro = (Idioma == "I" ? ins.ParametroInspeccion.ParametroIngles : ins.ParametroInspeccion.Parametro);

                                        if (ins.PathImage != "" && ins.PathImage != null)
                                            grid.Item(5).Background(Colors.White).Table(table =>
                                            {
                                                table.ColumnsDefinition(columns =>
                                                {
                                                    columns.RelativeColumn(50);

                                                });

                                                table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(text =>
                                                {
                                                    text.DefaultTextStyle(x => x.FontFamily(Fonts.Arial).FontSize(8));
                                                    text.Span(parametro);
                                                    text.EmptyLine();
                                                    text.Element()
                                                        .Image(HelpImage.GetFileContent(ins.PathImage));

                                                });

                                                //table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(ins.Parametro).FontFamily(Fonts.Arial).FontSize(8);
                                                //table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Image(ins.ImageByte);
                                            });
                                    }
                                });
                            }

                        });
                        #endregion

                        #region SEC - FOOTER
                        page.Footer().AlignCenter().Border(0).Row(row =>
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
                                   string FOOTSECTION_QUALITY_ENGINEER = pieza.UsuarioCompania == 1003 ? "QUALITY SUPERVISOR" : "QUALITY ENGINEER";


                                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(QUALITY_ENGINEER).FontFamily(Fonts.Arial).FontSize(9);
                                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text("___________________").FontFamily(Fonts.Arial).FontSize(9);
                                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text("CHECKED").FontFamily(Fonts.Arial).FontSize(9);
                                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(FOOTSECTION_QUALITY_ENGINEER).FontFamily(Fonts.Arial).FontSize(9).Bold();
                               });
                            row.ConstantItem(175).Background(Colors.White).Border(0).Height(50).AlignMiddle().AlignCenter()
                               .Table(table =>
                               {
                                   table.ColumnsDefinition(columns =>
                                   {
                                       columns.RelativeColumn(100);

                                   });
                                   string FOOTSECTION_QUALITY_MANAGER = pieza.UsuarioCompania == 1003 ? "QUALITY COORDINATOR" : "QUALITY_MANAGER";


                                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(QUALITY_MANAGER).FontFamily(Fonts.Arial).FontSize(9);
                                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text("___________________").FontFamily(Fonts.Arial).FontSize(9);
                                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text("AUTHORIZED").FontFamily(Fonts.Arial).FontSize(9);
                                   table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(FOOTSECTION_QUALITY_MANAGER).FontFamily(Fonts.Arial).FontSize(9).Bold();
                               });


                        });
                        #endregion
                    });

                });


                // instead of the standard way of generating a PDF file
                document.GeneratePdf(fileName);

            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


            return fileName;
        }


        private async Task<List<CertificadoIfdimension>> LimitLenCharactersToCellPdf(List<CertificadoIfdimension> mediciones, int Len=5)
        {
            for (int i = 0; i < mediciones.Count; i++)
            {
                mediciones[i].Val1 = mediciones[i].Val1?.Length > 4 && mediciones[i].Val1!=null ? mediciones[i].Val1.Substring(0, 5) : mediciones[i].Val1;
                mediciones[i].Val2 = mediciones[i].Val2?.Length > 4 && mediciones[i].Val2 != null ? mediciones[i].Val2.Substring(0, 5): mediciones[i].Val2;
                mediciones[i].Val3 = mediciones[i].Val3?.Length > 4 && mediciones[i].Val3 != null ? mediciones[i].Val3.Substring(0, 5):mediciones[i].Val3;
                mediciones[i].Val4 = mediciones[i].Val4?.Length > 4 && mediciones[i].Val4 != null ? mediciones[i].Val4.Substring(0, 5):mediciones[i].Val4 ;
                mediciones[i].Val5 = mediciones[i].Val5?.Length > 4 && mediciones[i].Val5 != null ? mediciones[i].Val5.Substring(0, 5):mediciones[i].Val5;
                mediciones[i].Val6 = mediciones[i].Val6?.Length > 4 && mediciones[i].Val6 != null ? mediciones[i].Val6.Substring(0, 5):mediciones[i].Val6;
                mediciones[i].Val7 = mediciones[i].Val7?.Length > 4 && mediciones[i].Val7 != null ? mediciones[i].Val7.Substring(0, 5) : mediciones[i].Val7;
                mediciones[i].Val8 = mediciones[i].Val8?.Length > 4 && mediciones[i].Val8 != null ? mediciones[i].Val8.Substring(0, 5) : mediciones[i].Val8;
                mediciones[i].Val9 = mediciones[i].Val9?.Length > 4 && mediciones[i].Val9 != null ? mediciones[i].Val9.Substring(0, 5) : mediciones[i].Val9;
                mediciones[i].Val10 = mediciones[i].Val10?.Length > 4 && mediciones[i].Val10 != null ? mediciones[i].Val10.Substring(0, 5) : mediciones[i].Val10;
                mediciones[i].Val11 = mediciones[i].Val11?.Length > 4 && mediciones[i].Val11 != null ? mediciones[i].Val11.Substring(0, 5) : mediciones[i].Val11;
                mediciones[i].Val12 = mediciones[i].Val12?.Length > 4 && mediciones[i].Val12 != null ? mediciones[i].Val12.Substring(0, 5) : mediciones[i].Val12;
                mediciones[i].Val13 = mediciones[i].Val13?.Length > 4 && mediciones[i].Val13 != null ? mediciones[i].Val13.Substring(0, 5) : mediciones[i].Val13;
                mediciones[i].Val14 = mediciones[i].Val14?.Length > 4 && mediciones[i].Val14 != null ? mediciones[i].Val14.Substring(0, 5) : mediciones[i].Val14;
                mediciones[i].Val15 = mediciones[i].Val15?.Length > 4 && mediciones[i].Val15 != null ? mediciones[i].Val15.Substring(0, 5) : mediciones[i].Val15;
                mediciones[i].Val16 = mediciones[i].Val16?.Length > 4 && mediciones[i].Val16 != null ? mediciones[i].Val16.Substring(0, 5) : mediciones[i].Val16;
                mediciones[i].Val17 = mediciones[i].Val17?.Length > 4 && mediciones[i].Val17 != null ? mediciones[i].Val17.Substring(0, 5) : mediciones[i].Val17;
                mediciones[i].Val18 = mediciones[i].Val18?.Length > 4 && mediciones[i].Val18 != null ? mediciones[i].Val18.Substring(0, 5) : mediciones[i].Val18;
                mediciones[i].Val19 = mediciones[i].Val19?.Length > 4 && mediciones[i].Val19!= null ? mediciones[i].Val19.Substring(0, 5) : mediciones[i].Val19;
                mediciones[i].Val20 = mediciones[i].Val20?.Length > 4 && mediciones[i].Val20 != null ? mediciones[i].Val20.Substring(0, 5) : mediciones[i].Val20;
            }
            return mediciones;

        }

        public async Task<string> GetCertificadoExcel()
        {
            
            return "";
        }


        public void GenerarExcel(string plantillaRuta, string resultadoRuta, string texto, string imagenRuta)
        {
            // Cargamos la plantilla de Excel existente
            FileInfo plantillaArchivo = new FileInfo(plantillaRuta);
            using (ExcelPackage excelPackage = new ExcelPackage(plantillaArchivo))
            {
                // Obtenemos la hoja de cálculo del archivo Excel
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Hoja1"];

                // Agregamos el texto a una celda específica
                worksheet.Cells["A1"].Value = texto;

                // Cargamos la imagen y la insertamos en una celda específica
                if (!string.IsNullOrEmpty(imagenRuta))
                {
                    worksheet = _HelpExcel.AddImageToSheet(worksheet, 3, 3,4,4,100,100,imagenRuta,"img");
                }

                
                // Insertamos una forma (shape) en el archivo Excel
                worksheet = _HelpExcel.AddShapeToExcel(worksheet, "Shape1", "Ejemplo con Help", 10, 10, 100, 50);

                // Guardamos el archivo de Excel con los cambios
                FileInfo resultadoArchivo = new FileInfo(resultadoRuta);
                excelPackage.SaveAs(resultadoArchivo);
            }
        }

        #endregion

        #region FormatoExcel 00
        // Metodo para cargar la plantilla del formato 00
        public bool CargarExcelFormato00_v1(string PlantillaMacro,string ResultadoRuta,CertificadoIf Certificado, PiezaSap Pieza, List<CertificadoIfdimension> LstDimensional, List<CertificadoIfapariencias> LstApariencia, List<PiezaConcesion> LstDefecto, List<InspeccionOptica> LstOpticos)
        {
            bool result = false;
            FileInfo plantillaArchivo = new FileInfo(PlantillaMacro);
            try{
                using (ExcelPackage excelPackage = new ExcelPackage(plantillaArchivo))
                {
                    // Obtenemos la hoja de cálculo del archivo Excel
                    ExcelWorksheet xlWorkSheetDatos = excelPackage.Workbook.Worksheets["DATOS"];
                    ExcelWorksheet xlWorkSheetTermo = excelPackage.Workbook.Worksheets["TERMO"];
                    ExcelWorksheet xlWorkSheetDistorsion = excelPackage.Workbook.Worksheets["DISTORSION"];
                    ExcelWorksheet xlWorkSheetDobleImagen = excelPackage.Workbook.Worksheets["DOBLE IMAGEN"];
                    ExcelWorksheet xlWorkSheetAccesories = excelPackage.Workbook.Worksheets["ACCESSORIES"];
                    ExcelWorksheet xlWorkSheetSerigrafia = excelPackage.Workbook.Worksheets["SERIGRAFIA"];
                    ExcelWorksheet xlWorkSheetBN = excelPackage.Workbook.Worksheets["BN"];
                    ExcelWorksheet xlWorkSheetTNT = excelPackage.Workbook.Worksheets["TNT"];
                    ExcelWorksheet xlWorkSheetPC = excelPackage.Workbook.Worksheets["PC"];
                    ExcelWorksheet xlWorkSheetSA = excelPackage.Workbook.Worksheets["STICKER ADUANA"];
                    ExcelWorksheet xlWorkSheetTORKER = excelPackage.Workbook.Worksheets["TORKER"];

                    string responsableApariencia = (LstApariencia.Count > 0) ? LstApariencia[0].UsuarioCrea : "";
                    //worksheet.Cells["A1"].Value = texto;

                    xlWorkSheetDatos.Cells["AE7"].Value = Pieza.LoteLogistico;
                    xlWorkSheetDatos.Cells["AR7"].Value = Pieza.Color;
                    xlWorkSheetDatos.Cells["AE8"].Value = Pieza.Formula;
                    xlWorkSheetDatos.Cells["N83"].Value = Pieza.UsuarioCrea;
                    xlWorkSheetDatos.Cells["AS84"].Value = DateTime.Now.Day.ToString(); //pieza.FechaCrea.Substring(0, 2);
                    xlWorkSheetDatos.Cells["AT84"].Value = DateTime.Now.Month.ToString(); //pieza.FechaCrea.Substring(3, 2);
                    xlWorkSheetDatos.Cells["AU84"].Value = DateTime.Now.Year.ToString();

                    xlWorkSheetTermo.Cells["M4"].Value = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
                    xlWorkSheetTermo.Cells["F38"].Value = responsableApariencia;

                    xlWorkSheetDistorsion.Cells["H6"].Value = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
                    xlWorkSheetDistorsion.Cells["E76"].Value = responsableApariencia;

                    xlWorkSheetDobleImagen.Cells["G6"].Value = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
                    xlWorkSheetDobleImagen.Cells["D46"].Value = responsableApariencia;

                    xlWorkSheetAccesories.Cells["M4"].Value = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
                    xlWorkSheetAccesories.Cells["F37"].Value = responsableApariencia;

                    xlWorkSheetSerigrafia.Cells["M4"].Value = Pieza.FechaCrea.ToString("dd/MM/yyyy");
                    xlWorkSheetSerigrafia.Cells["F48"].Value = responsableApariencia;

                    xlWorkSheetBN.Cells["F48"].Value = responsableApariencia;
                    xlWorkSheetBN.Cells["M4"].Value = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();

                    xlWorkSheetTNT.Cells["J4"].Value = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
                    xlWorkSheetTNT.Cells["E47"].Value = responsableApariencia;

                    xlWorkSheetPC.Cells["F48"].Value = responsableApariencia;
                    xlWorkSheetPC.Cells["L4"].Value = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();

                    xlWorkSheetSA.Cells["F32"].Value = responsableApariencia;
                    xlWorkSheetSA.Cells["L4"].Value = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();


                    //CARGAR IMAGEN DEFECTO
                    if (Pieza.DefectoImagen != "" && Pieza.DefectoImagen != null)
                    {
                        _HelpExcel.AddImageToSheet(xlWorkSheetDatos,12,2,10,10, 790, 380, Pieza.DefectoImagen, Pieza.Id.ToString());
                         //cargarImagenHoja(xlWorkSheetDatos, 50, 200, 790, 380, Pieza.DefectoImagen); // todo:borrar
                    }

                    //Cargar los defectos
                    int intContador = 49;
                    foreach (var item in LstDefecto)
                    {
                        xlWorkSheetDatos.Cells["E" + intContador.ToString()].Value = item.Zona;
                        xlWorkSheetDatos.Cells["K" + intContador.ToString()].Value = item.DefectoMaestro.Defecto;
                        xlWorkSheetDatos.Cells["AK" + intContador.ToString()].Value = item.Tamanio;
                        xlWorkSheetDatos.Cells["AP" + intContador.ToString()].Value = "";
                        xlWorkSheetDatos.Cells["AR" + intContador.ToString()].Value = item.Observacion;
                        ++intContador;
                    }


                    //Cargar la inspeccion de apariencia

                    foreach (var item in LstApariencia)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {
                            case "322": //  AISLAMIENTO DE TERMINALES - K63
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "K63" : "V63")].Value = "X";
                                break;
                            case "368": //  Terminales - K64
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "K64" : "V64")].Value = "X";
                                break;
                            case "266": //  Acabado y limpieza de bordes - K65
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper()=="CUMPLE") ? "K65" : "V65")].Value = "X";
                                break;
                            case "323": //  SOPORTE ESPEJO  MIRROR BRACKET - K66
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "K66" : "V66")].Value = "X";
                                break;
                            case "297": //   Espesor X5 (PBS)    THICKNEES K67
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K67"].Value = item.Valor;
                                break;
                            case "298": //  Espesor X5 (LD) THICKNEES   K67
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K67"].Value = item.Valor;
                                break;
                            case "299": //  Espesor X5 (LE) THICKNEES
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K67"].Value = item.Valor;
                                break;
                            case "300": //  Espesor X5 (CABINA/PARTICIÓN)   THICKNEES
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K67"].Value = item.Valor;
                                break;
                            case "259": //  Apariencia serigrafia   SERIGRAPHY K68
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "K68" : "V68")].Value = "X";
                                break;
                            case "257": // Logo    LOGO (INTERNAL BLACK) AL63
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL63" : "AS63")].Value = "X";
                                break;
                            case "263": // Chaflan CHAMFER
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL64" : "AS64")].Value = "Xd";
                                break;
                            case "367": // Reflexion   REFLECTION
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL65" : "AS65")].Value = "X";
                                break;
                            case "324": // BANDA NEGRA INTERNA BLACK BAND INTERNAL
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL66" : "AS66")].Value = "X";
                                break;
                            case "325": // BANDA NEGRA EXTERNA BLACK BAND EXTERNAL
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL67" : "AS67")].Value = "X";
                                break;
                            default:
                                break;
                        }
                    }


                    // Cargar la inspeccion dimensional

                    foreach (var item in LstDimensional)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {
                            case "334": // Zona A (Mdop)  - Zona A - E26
                                xlWorkSheetDistorsion.Cells["E26"].Value = item.Val1;
                                xlWorkSheetDistorsion.Cells["E27"].Value = item.Val2;
                                xlWorkSheetDistorsion.Cells["F26"].Value = item.Val3;
                                xlWorkSheetDistorsion.Cells["F27"].Value = item.Val4;
                                break;
                            case "335": // Zona A (Mdop)  - Zona A - E26
                                xlWorkSheetDistorsion.Cells["G26"].Value = item.Val1;
                                xlWorkSheetDistorsion.Cells["G27"].Value = item.Val2;
                                xlWorkSheetDistorsion.Cells["H26"].Value = item.Val3;
                                xlWorkSheetDistorsion.Cells["H27"].Value = item.Val4;
                                break;
                            case "345": // banda negra externa A  - BLACK BAND EXTERNAL - RECUADRO A
                                        //cargarShapeHoja(xlWorkSheetBN, 20, 4, 17, item.Val1); // local
                                        //cargarShapeHoja(xlWorkSheetBN, 20, 4, 17, item.Val1, -2);   // servidor

                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString()+"_shape", item.Val1, 19, 4, 24, 12,-14,0);
                                break;
                            case "346": //  banda negra externa B -  BLACK BAND EXTERNAL - RECUADRO B   J24
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 23, 9, 24, 12,5,-2);
                                break;
                            case "347": // banda negra externa C -  BLACK BAND EXTERNAL - RECUADRO C   - M18
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 20, 13, 24, 12,-60,-7);
                                break;
                            case "348": // banda negra externa D -  BLACK BAND EXTERNAL - RECUADRO D  -  J14

                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 15, 9, 24, 12,6,-5);
                                // cargarShapeHoja(xlWorkSheetBN, 15, 10, 4, item.Val1,7); //local
                                //cargarShapeHoja(xlWorkSheetBN, 15, 10, 4, item.Val1, 5);    //servidor
                                break;
                            case "349": // banda negra externa A -  BLACK BAND EXTERNAL - RECUADRO A  - D39
                                        // cargarShapeHoja(xlWorkSheetBN, 39, 4, 3, item.Val1); // Local 
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 38, 3, 24, 12,5,0);
                                // cargarShapeHoja(xlWorkSheetBN, 39, 4, 3, item.Val1, -2);    // servidor
                                break;
                            case "350": // banda negra externa B -  BLACK BAND EXTERNAL - RECUADRO B - J42
                                        // cargarShapeHoja(xlWorkSheetBN, 42, 9, 28, item.Val1,2); // Local 
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 41, 9, 24, 12,2,4);
                                //cargarShapeHoja(xlWorkSheetBN, 42, 9, 28, item.Val1, -2);
                                break;
                            case "351": // banda negra externa C -  BLACK BAND EXTERNAL - RECUADRO C  - M39
                                        // cargarShapeHoja(xlWorkSheetBN, 39, 13, 32, item.Val1,-1);   // Local 
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 38, 12, 24, 12,50,0);
                                // cargarShapeHoja(xlWorkSheetBN, 39, 13, 32, item.Val1, -3);
                                break;
                            case "352": // banda negra interna D -  BLACK BAND INTERNAL - RECUADRO D  - J34
                                        // cargarShapeHoja(xlWorkSheetBN, 34, 10, 0, item.Val1,6); // Local 
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 34, 9, 24, 12);
                                //cargarShapeHoja(xlWorkSheetBN, 34, 10, -4, item.Val1, 6);
                                break;
                            case "357": // Medición TNT FLEX A-1 -  TNT FLEX MEASUREMET - E37
                                xlWorkSheetTNT.Cells["E37"].Value = item.Val1;
                                break;
                            case "358": // Medición TNT FLEX C-1  - TNT FLEX MEASUREMET - E39
                                xlWorkSheetTNT.Cells["E39"].Value = item.Val1;
                                break;
                            case "359": // Medición TNT FLEX A-2 -  TNT FLEX MEASUREMET - G37
                                xlWorkSheetTNT.Cells["G37"].Value = item.Val1;
                                break;
                            case "360": // Medición TNT FLEX B-2  - TNT FLEX MEASUREMET  - G38
                                xlWorkSheetTNT.Cells["G38"].Value = item.Val1;
                                break;
                            case "361": // Medición TNT FLEX C-2  -  TNT FLEX MEASUREMET - G39
                                xlWorkSheetTNT.Cells["G39"].Value = item.Val1;
                                break;
                            case "362": // Medición TNT FLEX D-2  - TNT FLEX MEASUREMET - G40
                                xlWorkSheetTNT.Cells["G40"].Value = item.Val1;
                                break;
                            case "327": // Medición TNT FLEX C-2  -  TNT FLEX MEASUREMET - H39
                                xlWorkSheetTermo.Cells["M15"].Value = item.Val1;
                                break;
                            case "328": //  Medición TNT FLEX D-2  - TNT FLEX MEASUREMET - H40
                                xlWorkSheetTermo.Cells["M16"].Value = item.Val1;
                                break;
                            default:
                                break;
                        }
                    }

                    // Cargar Inspeccion optica
                    string strArchivo = "";
                    LstOpticos=LstOpticos.Where(x => x.PathImage != "").ToList();
                    foreach (var item in LstOpticos)
                    {

                        switch (item.ParametroInspeccionId.ToString())
                        {
                            // HOLA TERMO

                            case "329": // Termografia izquierda PBS  -  Left Thermograph - B20
                                        //worksheet = _HelpExcel.AddImageToSheet(worksheet, 3, 3,3,3, item.UrlImage, item.ParametroInspeccionId.ToString());
                                xlWorkSheetTermo = _HelpExcel.AddImageToSheet(xlWorkSheetTermo, 19, 1,15,15, 200, 140, item.PathImage, item.ParametroInspeccionId.ToString()+"_Optica");
                                break;
                            case "330": // Termografia derecha PBS Right Thermograph   - H20
                                        //cargarImagenHoja(xlWorkSheetTermo, 340, 290, 200, 150, strArchivo);
                                xlWorkSheetTermo = _HelpExcel.AddImageToSheet(xlWorkSheetTermo, 19, 7, 15, 15,200,150, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                break;
                            case "331": // Terminal superior PBS  - Upper central -  B34
                                xlWorkSheetTermo = _HelpExcel.AddImageToSheet(xlWorkSheetTermo, 33, 1, 15, 15,168,140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetTermo, 30, 490, 160, 140, strArchivo);
                                break;
                            case "332": // Terminal izquierda PBS  - Lower left  - E34
                                xlWorkSheetTermo = _HelpExcel.AddImageToSheet(xlWorkSheetTermo, 33, 4, 15, 15,160,140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetTermo, 220, 490, 160, 140, strArchivo);
                                break;
                            case "333": // Terminal derecha PBS  -  Lower right  - L34
                                xlWorkSheetTermo = _HelpExcel.AddImageToSheet(xlWorkSheetTermo, 33, 11, 15, 15,160,148, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetTermo, 410, 490, 160, 140, strArchivo);
                                break;

                            // HOLA DISTORSION
                            case "270": // Distorsion con cebra 0 grados -  DISTORTION IMAGE AT 0° - B42 
                                xlWorkSheetDistorsion = _HelpExcel.AddImageToSheet(xlWorkSheetDistorsion, 41, 1, 15,15,200,150, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                // cargarImagenHoja(xlWorkSheetDistorsion, 40, 395, 200, 150, strArchivo);
                                break;
                            case "271": // Distorsion con cebra 45 grados - DISTORTION IMAGE AT 45° - G42
                                xlWorkSheetDistorsion = _HelpExcel.AddImageToSheet(xlWorkSheetDistorsion, 41, 6, 15, 15,200,150, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetDistorsion, 410, 395, 200, 150, strArchivo);
                                break;
                            case "273": // Reflexion 1 - REFLECTION 1  - B57
                                xlWorkSheetDistorsion = _HelpExcel.AddImageToSheet(xlWorkSheetDistorsion, 56, 1, 15, 15,200,150, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetDistorsion, 40, 595, 200, 150, strArchivo);
                                break;
                            case "336": // Reflexion 2 - REFLECTION 2  -  G57
                                xlWorkSheetDistorsion = _HelpExcel.AddImageToSheet(xlWorkSheetDistorsion, 56, 6, 15, 15,200,150, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetDistorsion, 410, 595, 200, 150, strArchivo);
                                break;

                            // HOLA DOBLE IMAGEN
                            case "272": // Doble vision  -  DOUBLE IMAGE   -  D28
                                xlWorkSheetDobleImagen = _HelpExcel.AddImageToSheet(xlWorkSheetDobleImagen, 27, 3, 15, 15,200,150, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetDobleImagen, 218, 363, 200, 150, strArchivo);
                                break;

                            // HOLA ACCESORIES
                            case "337": // TNT FLEX 1 - TNT FLEX  -  B15
                                        //cargarImagenHoja(xlWorkSheetAccesories, 35, 250, 130, 120, strArchivo);  // Local
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 14, 1, 15,15,130,120, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 30, 250, 130, 120, strArchivo);  // serviodr
                                break;
                            case "338": // TNT FLEX 2  - TNT FLEX  -  D15 
                                        //cargarImagenHoja(xlWorkSheetAccesories, 180, 250, 130, 120, strArchivo); // Local
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 14, 3, 15, 15,130,120,item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 175, 250, 130, 120, strArchivo); // servidor
                                break;
                            case "339": // TNT FLEX 3 - TNT FLEX  - I15
                                        //cargarImagenHoja(xlWorkSheetAccesories, 330, 250, 130, 120, strArchivo); // local
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 14, 8, 15,15,130,120, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 325, 250, 130, 120, strArchivo); // servidor
                                break;
                            case "340": // 340 optico  TNT FLEX 4 - TNT FLEX  -  M15
                                        //cargarImagenHoja(xlWorkSheetAccesories, 490, 250, 130, 120, strArchivo); // local
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 14, 12, 15, 15,130,120, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 482, 250, 130, 120, strArchivo);   // servidor
                                break;
                            case "341": // soporte espejo. - MIRROR BRACKET - B32
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 31, 1, 15, 15,130,120, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 30, 490, 130, 120, strArchivo);   // servidor
                                break;
                            case "342": // Acero 1 - STEELS - D32
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 31, 3, 15, 15,130,120, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 175, 490, 130, 120, strArchivo);     // servidor
                                break;
                            case "343": // Acero 2 - STEELS - I32
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 31, 8, 15,15, 130, 120, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 325, 490, 130, 120, strArchivo);     // servidor
                                break;
                            case "344": // optico  Acero 3 - STEELS - M32
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 31, 12, 15, 15, 130, 120, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 482, 490, 130, 120, strArchivo);     // servidor
                                break;

                            // HOLA SERIGRAFIA
                            case "353": // Serigrafia 1  -  B13
                                        //   cargarImagenHoja(xlWorkSheetSerigrafia, 30, 210, 265, 195, strArchivo);     // local
                                xlWorkSheetSerigrafia = _HelpExcel.AddImageToSheet(xlWorkSheetSerigrafia, 12, 1, 15, 15,265,195, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetSerigrafia, 21, 210, 265, 195, strArchivo); // servidor
                                break;
                            case "354": // Serigrafia 2 - K13
                                        // cargarImagenHoja(xlWorkSheetSerigrafia, 380, 210, 265, 195, strArchivo);    // local
                                xlWorkSheetSerigrafia = _HelpExcel.AddImageToSheet(xlWorkSheetSerigrafia, 12, 10, 15, 15,265,195, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetSerigrafia, 364, 210, 265, 195, strArchivo);    // servidor
                                break;
                            case "355": // Serigrafia 3  - B31
                                        //  cargarImagenHoja(xlWorkSheetSerigrafia, 30, 455, 265, 205, strArchivo); // local
                                xlWorkSheetSerigrafia = _HelpExcel.AddImageToSheet(xlWorkSheetSerigrafia, 30, 1, 15, 15,265,205, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetSerigrafia, 21, 455, 265, 205, strArchivo);     // servidor
                                break;
                            case "356": // Serigrafia 4 - K31
                                        //   cargarImagenHoja(xlWorkSheetSerigrafia, 380, 455, 265, 205, strArchivo);    // local
                                xlWorkSheetSerigrafia = _HelpExcel.AddImageToSheet(xlWorkSheetSerigrafia, 30, 10, 15, 15,265,205, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetSerigrafia, 364, 455, 265, 205, strArchivo);    // servidor
                                break;

                            // HOLA PC
                            case "363": // PC 1  -  Image 1  - C13
                                xlWorkSheetPC = _HelpExcel.AddImageToSheet(xlWorkSheetPC,14 ,3, 20, 0,260,195, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetPC, 190, 206, 260, 195, strArchivo);
                                break;
                            case "364": // PC 2  -  Image 2 - C31
                                xlWorkSheetPC = _HelpExcel.AddImageToSheet(xlWorkSheetPC, 32, 3, 20, 0,260,200, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetPC, 190, 455, 260, 200, strArchivo);
                                break;

                            // HOJA STICKER
                            case "388": // C13
                                xlWorkSheetSA = _HelpExcel.AddImageToSheet(xlWorkSheetSA, 12, 2, 15,15,450, 220, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetSA, 125, 210, 380, 185, strArchivo);
                                break;
                            // HOJA TORKER TEST
                            case "647": // C13
                                xlWorkSheetTORKER = _HelpExcel.AddImageToSheet(xlWorkSheetTORKER, 12, 2, 15, 15, 450, 220, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                break;

                            default:
                                break;
                        }
                    }

                    // Guardamos el archivo de Excel con los cambios
                    FileInfo resultadoArchivo = new FileInfo(ResultadoRuta);
                    excelPackage.SaveAs(resultadoArchivo);
                }

                result = true;
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            /*
          
                */
            return result;
        }

        public bool CargarExcelFormato00(string PlantillaMacro, string ResultadoRuta, CertificadoIf Certificado, PiezaSap Pieza, List<CertificadoIfdimension> LstDimensional, List<CertificadoIfapariencias> LstApariencia, List<PiezaConcesion> LstDefecto, List<InspeccionOptica> LstOpticos)
        {
            bool result = false;
            FileInfo plantillaArchivo = new FileInfo(PlantillaMacro);
            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage(plantillaArchivo))
                {
                    // Obtenemos la hoja de cálculo del archivo Excel
                    ExcelWorksheet xlWorkSheetDatos = excelPackage.Workbook.Worksheets["DATOS"];
                    ExcelWorksheet xlWorkSheetTermo = excelPackage.Workbook.Worksheets["TERMO"];
                    ExcelWorksheet xlWorkSheetDistorsion = excelPackage.Workbook.Worksheets["DISTORSION"];
                    ExcelWorksheet xlWorkSheetDobleImagen = excelPackage.Workbook.Worksheets["DOBLE IMAGEN"];
                    ExcelWorksheet xlWorkSheetAccesories = excelPackage.Workbook.Worksheets["ACCESSORIES"];
                    ExcelWorksheet xlWorkSheetBN = excelPackage.Workbook.Worksheets["BN"];
                    ExcelWorksheet xlWorkSheetSerigrafia = excelPackage.Workbook.Worksheets["SERIGRAFIA"];
                    ExcelWorksheet xlWorkSheetTNT = excelPackage.Workbook.Worksheets["TNT"];
                    ExcelWorksheet xlWorkSheetPC = excelPackage.Workbook.Worksheets["PC"];
                    ExcelWorksheet xlWorkSheetTorker = excelPackage.Workbook.Worksheets["TORKER"];
                    ExcelWorksheet xlWorkSheetSticker = excelPackage.Workbook.Worksheets["STICKER ADUANA"];
                    //ExcelWorksheet xlWorkSheetBNEditable = excelPackage.Workbook.Worksheets["BN EDITABLE"];


                    string responsableApariencia = (LstApariencia.Count > 0) ? LstApariencia[0].UsuarioCrea : "";
                    //worksheet.Cells["A1"].Value = texto;


                    xlWorkSheetDatos.Cells["E7"].Value = Pieza.Cliente;  // CLIENT
                    xlWorkSheetDatos.Cells["E8"].Value = Pieza.Vehiculo;  // VEHICLE
                    xlWorkSheetDatos.Cells["E9"].Value = Pieza.Vidrio + " / " + Pieza.Modelo;  // Part

                    xlWorkSheetDatos.Cells["AE7"].Value = Pieza.LoteLogistico; //Production Lot
                    xlWorkSheetDatos.Cells["AE8"].Value = Pieza.Formula; //Composition
                    //xlWorkSheetDatos.Cells["AE9"].Value = Pieza.LoteLogistico; //Other Glass finish

                    xlWorkSheetDatos.Cells["AR7"].Value = Pieza.Color; //Color
                    xlWorkSheetDatos.Cells["AR8"].Value = Pieza.PartNumber; //Part Number
                    //xlWorkSheetDatos.Cells["AR9"].Value = Pieza.LoteLogistico; // PACKAGE EDGE TYPE:		

                    xlWorkSheetDatos.Cells["N83"].Value = responsableApariencia; // Reviewed By
                    xlWorkSheetDatos.Cells["AS84"].Value = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();


                    //CARGAR IMAGEN DEFECTO  -- B11
                    if (Pieza.DefectoImagen != "" && Pieza.DefectoImagen != null)
                    {
                        _HelpExcel.AddImageToSheet(xlWorkSheetDatos, 11, 2, 10, 10, 790, 380, Pieza.DefectoImagen, Pieza.Id.ToString());

                        //cargarImagenHoja(xlWorkSheetDatos, 50, 200, 790, 380, Pieza.DefectoImagen); // todo:borrar
                    }
                    else
                    {
                        _HelpExcel.AddImageToSheet(xlWorkSheetDatos, 11, 2, 10, 10, 890, 580, Pieza.IMAGEN_PLANO_STANDAR, Pieza.Id.ToString());
                    }


                    //Cargar los defectos
                    int intContador = 49;
                    foreach (var item in LstDefecto)
                    {
                        xlWorkSheetDatos.Cells["E" + intContador.ToString()].Value = item.Zona;
                        xlWorkSheetDatos.Cells["K" + intContador.ToString()].Value = item.DefectoMaestro.Defecto;
                        xlWorkSheetDatos.Cells["H" + intContador.ToString()].Value = item.DefectoMaestro.Id;
                        xlWorkSheetDatos.Cells["AK" + intContador.ToString()].Value = item.Tamanio;
                        xlWorkSheetDatos.Cells["AP" + intContador.ToString()].Value = 1;
                        xlWorkSheetDatos.Cells["AR" + intContador.ToString()].Value = item.Observacion;
                        ++intContador;
                    }


                    //Cargar la inspeccion de apariencia


                    foreach (var item in LstApariencia)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {
                            case "322": //  AISLAMIENTO DE TERMINALES - K63
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "K63" : "V63")].Value = "X";
                                break;
                            case "368": //  Terminales - K64
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "K64" : "V64")].Value = "X";
                                break;
                            case "266": //  Acabado y limpieza de bordes - K65
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "K65" : "V65")].Value = "X";
                                break;
                            case "323": //  SOPORTE ESPEJO  MIRROR BRACKET - K66
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "K66" : "V66")].Value = "X";
                                break;
                            case "297": //   Espesor X5 (PBS)    THICKNEES K67
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K67"].Value = item.Valor;
                                break;
                            case "298": //  Espesor X5 (LD) THICKNEES   K67
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K67"].Value = item.Valor;
                                break;
                            case "299": //  Espesor X5 (LE) THICKNEES
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K67"].Value = item.Valor;
                                break;
                            case "300": //  Espesor X5 (CABINA/PARTICIÓN)   THICKNEES
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K67"].Value = item.Valor;
                                break;
                            case "259": //  Apariencia serigrafia   SERIGRAPHY K68
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "K68" : "V68")].Value = "X";
                                break;
                            case "257": // Logo    LOGO (INTERNAL BLACK) AL63
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL63" : "AS63")].Value = "X";
                                break;
                            case "263": // Chaflan CHAMFER
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL64" : "AS64")].Value = "X";
                                break;
                            case "367": // Reflexion   REFLECTION
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL65" : "AS65")].Value = "X";
                                break;
                            case "324": // BANDA NEGRA INTERNA BLACK BAND INTERNAL
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL66" : "AS66")].Value = "X";
                                break;
                            case "325": // BANDA NEGRA EXTERNA BLACK BAND EXTERNAL
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL67" : "AS67")].Value = "X";
                                break;
                            case "262": // BRILLIANT ROUNDED / Tipo de borde vidrio pintura
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL64" : "AS64")].Value = "X";

                                break;
                            default:
                                break;
                        }
                    }


                    // Cargar la inspeccion dimensional

                    foreach (var item in LstDimensional)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {
                            case "334": // Zona A (Mdop)  - Zona A - E26
                                xlWorkSheetDistorsion.Cells["E26"].Value = item.Val1;
                                xlWorkSheetDistorsion.Cells["E27"].Value = item.Val2;
                                xlWorkSheetDistorsion.Cells["F26"].Value = item.Val3;
                                xlWorkSheetDistorsion.Cells["F27"].Value = item.Val4;
                                break;
                            case "335": // Zona B (Mdop)  - Zona B - G26
                                xlWorkSheetDistorsion.Cells["G26"].Value = item.Val1;
                                xlWorkSheetDistorsion.Cells["G27"].Value = item.Val2;
                                xlWorkSheetDistorsion.Cells["H26"].Value = item.Val3;
                                xlWorkSheetDistorsion.Cells["H27"].Value = item.Val4;
                                break;
                            case "389": // banda negra externa A  - BLACK BAND EXTERNAL - RECUADRO A  - E24
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 23, 4, 24, 12);
                                // cargarShapeHoja(xlWorkSheetBN, 23, 4, 16, item.Val1, 4);
                                break;
                            case "390": // banda negra externa B -  BLACK BAND EXTERNAL - RECUADRO B - L18
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 17, 11, 24, 12);
                                //cargarShapeHoja(xlWorkSheetBN, 17, 12, 13, item.Val1, 5);
                                break;
                            case "391": // banda negra externa C -  BLACK BAND EXTERNAL - RECUADRO C  - M25
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 24, 12, 24, 12);
                                //cargarShapeHoja(xlWorkSheetBN, 24, 13, 35, item.Val1, 6);
                                break;

                            case "392": // banda negra interna A -  BLACK BAND NTERNAL - RECUADRO A  -   E41
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 40, 4, 24, 12);
                                //cargarShapeHoja(xlWorkSheetBN, 41, 4, 20, item.Val1, 1);
                                break;
                            case "393": // banda negra interna B -  BLACK BAND INTERNAL - RECUADRO B -  L36
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 35, 11, 24, 12);
                                //cargarShapeHoja(xlWorkSheetBN, 35, 12, 16, item.Val1, 14);
                                break;
                            case "394": // banda negra interna C -  BLACK BAND INTERNAL - RECUADRO C  - M41
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 40, 12, 24, 12);
                                //cargarShapeHoja(xlWorkSheetBN, 41, 13, 32, item.Val1, 12);
                                break;
                            //case "352": // banda negra interna D -  BLACK BAND INTERNAL - RECUADRO B
                            //    cargarShapeHoja(xlWorkSheetBN, 36, 12, 0, item.Val1);
                            //   break;
                            case "381": // RESISTENCIA ELECTRICIA LD - INSPECCION ELECTRICA - M15
                                xlWorkSheetTermo.Cells["M15"].Value = item.Val1;
                                break;
                            default:
                                break;
                        }
                    }





                    // Cargar Inspeccion optica

                    string strArchivo = "";
                    foreach (var item in LstOpticos)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {
                            // HOJA TERMO
                            case "329": // Left Thermograph  -  Inspeccion Optico - B20
                                xlWorkSheetTermo = _HelpExcel.AddImageToSheet(xlWorkSheetTermo, 20, 1, 15, 15, 200, 140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetTermo, 35, 280, 200, 140, strArchivo);
                                break;
                            case "330": // Right Thermograph - Inspeccion Optico -  E20
                                xlWorkSheetTermo = _HelpExcel.AddImageToSheet(xlWorkSheetTermo, 20, 4, 15, 15, 200, 140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                // cargarImagenHoja(xlWorkSheetTermo, 35, 460, 200, 140, strArchivo);
                                break;
                            case "369": // Camera Thermograph- Inspeccion Optico -  L20
                                xlWorkSheetTermo = _HelpExcel.AddImageToSheet(xlWorkSheetTermo, 20, 11, 15, 15, 200, 140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                // cargarImagenHoja(xlWorkSheetTermo, 35, 460, 200, 140, strArchivo);
                                break;

                            case "331": // UPPER CENTRAL  Terminals - Inspeccion Optico -  B34
                                xlWorkSheetTermo = _HelpExcel.AddImageToSheet(xlWorkSheetTermo, 33, 1, 15, 15, 200, 140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                // cargarImagenHoja(xlWorkSheetTermo, 35, 460, 200, 140, strArchivo);
                                break;
                            case "332": // UPPER CENTRAL  Terminals - Inspeccion Optico -  E34
                                xlWorkSheetTermo = _HelpExcel.AddImageToSheet(xlWorkSheetTermo, 33, 4, 15, 15, 200, 140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                // cargarImagenHoja(xlWorkSheetTermo, 35, 460, 200, 140, strArchivo);
                                break;
                            case "333": // LOWER RIGHT  Terminals - Inspeccion Optico -  L34
                                xlWorkSheetTermo = _HelpExcel.AddImageToSheet(xlWorkSheetTermo, 33, 11, 15, 15, 200, 140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                // cargarImagenHoja(xlWorkSheetTermo, 35, 460, 200, 140, strArchivo);
                                break;


                            /*
                        case "395": // Termografia LDS  -  Inspeccion Optico - B15
                            xlWorkSheetTermo = _HelpExcel.AddImageToSheet(xlWorkSheetTermo, 18, 5, 15, 15, 200, 140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                            //cargarImagenHoja(xlWorkSheetTermo, 35, 280, 200, 140, strArchivo);
                            break;
                        case "380": // Terminal LD  - Inspeccion Optico -  B30
                            xlWorkSheetTermo = _HelpExcel.AddImageToSheet(xlWorkSheetTermo, 30, 5, 15, 15, 200, 140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                            // cargarImagenHoja(xlWorkSheetTermo, 35, 460, 200, 140, strArchivo);
                            break; */

                            // HOJA DISTORSION
                            case "270": // Distorsion con cebra 0 grados -  DISTORTION IMAGE AT 0° - B41
                                xlWorkSheetDistorsion = _HelpExcel.AddImageToSheet(xlWorkSheetDistorsion, 41, 1, 0, 15, 200, 140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetDistorsion, 40, 398, 200, 140, strArchivo);
                                break;

                            case "269": // Distorsion sin zebra 45 grados - DISTORTION WHIOUT ZEBRA - E41
                                xlWorkSheetDistorsion = _HelpExcel.AddImageToSheet(xlWorkSheetDistorsion, 41, 4, 15, 15, 200, 140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetDistorsion, 415, 398, 200, 140, strArchivo);
                                break;
                            case "271": // Distorsion con cebra 45 grados - DISTORTION IMAGE AT 45° - G42
                                xlWorkSheetDistorsion = _HelpExcel.AddImageToSheet(xlWorkSheetDistorsion, 41, 6, 15, 15, 200, 140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetDistorsion, 415, 398, 200, 140, strArchivo);
                                break;

                            case "273": // Reflexion 1 - REFLECTION 1  - B52
                                xlWorkSheetDistorsion = _HelpExcel.AddImageToSheet(xlWorkSheetDistorsion, 52, 1, 0, 15, 200, 140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetDistorsion, 40, 590, 200, 140, strArchivo);
                                break;
                            case "365": // LANDSCAPE DISTORTION  -  E52
                                xlWorkSheetDistorsion = _HelpExcel.AddImageToSheet(xlWorkSheetDistorsion, 52, 4, 15, 15, 200, 140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetDistorsion, 415, 590, 200, 140, strArchivo);
                                break;
                            case "336": // Reflexion 2 - REFLECTION 2  -  E52
                                xlWorkSheetDistorsion = _HelpExcel.AddImageToSheet(xlWorkSheetDistorsion, 52, 6, 15, 15, 200, 140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetDistorsion, 415, 590, 200, 140, strArchivo);
                                break;

                            // HOJA DOBLE IMAGEN
                            case "272": // Doble vision  -  DOUBLE IMAGE   -  D28
                                xlWorkSheetDobleImagen = _HelpExcel.AddImageToSheet(xlWorkSheetDobleImagen, 25, 3, 15, 15, 260, 190, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                // cargarImagenHoja(xlWorkSheetDobleImagen, 210, 350, 200, 150, strArchivo);
                                break;

                            // HOJA ACCESORIES
                            case "337": // TNT FLEX 1 - TNT FLEX  -  B15
                                        //cargarImagenHoja(xlWorkSheetAccesories, 35, 250, 130, 120, strArchivo);  // Local
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 14, 1, 15, 15, 130, 120, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 30, 250, 130, 120, strArchivo);  // serviodr
                                break;
                            case "338": // TNT FLEX 2  - TNT FLEX  -  D15 
                                        //cargarImagenHoja(xlWorkSheetAccesories, 180, 250, 130, 120, strArchivo); // Local
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 14, 3, 15, 15, 130, 120, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 175, 250, 130, 120, strArchivo); // servidor
                                break;
                            case "339": // TNT FLEX 3 - TNT FLEX  - I15
                                        //cargarImagenHoja(xlWorkSheetAccesories, 330, 250, 130, 120, strArchivo); // local
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 14, 8, 15, 15, 130, 120, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 325, 250, 130, 120, strArchivo); // servidor
                                break;
                            case "340": // 340 optico  TNT FLEX 4 - TNT FLEX  -  M15
                                        //cargarImagenHoja(xlWorkSheetAccesories, 490, 250, 130, 120, strArchivo); // local
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 14, 12, 15, 15, 130, 120, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 482, 250, 130, 120, strArchivo);   // servidor
                                break;
                            case "342": // Acero 1 - STEELS - C27
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 26, 2, 15, 15, 170, 135, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 205, 210, 250, 160, strArchivo);
                                break;
                            case "343": // Acero 2 - STEELS - G27
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 26, 6, 15, 15, 170, 135, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 205, 412, 250, 170, strArchivo);
                                break;
                            case "344": // Acero 3 - STEELS - L27
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 26, 11, 15, 15, 170, 135, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 205, 625, 250, 180, strArchivo);
                                break;

                            case "341": // MIRROR BRACKET - STEELS - B32
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 31, 1, 15, 15, 170, 115, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 205, 625, 250, 180, strArchivo);
                                break;
                            case "703": // CAMARA BRACKET - STEELS - D32
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 31, 3, 15, 15, 170, 115, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 205, 625, 250, 180, strArchivo);
                                break;
                            case "701": // CABLE OUTPUT (PINS) - STEELS - I32
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 31, 8, 15, 15, 170, 115, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 205, 625, 250, 180, strArchivo);
                                break;
                            case "725": // CONNECTORS - STEELS - M32
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 31, 12, 15, 15, 180, 115, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 205, 625, 250, 180, strArchivo);
                                break;


                            // HOJA SERIGRAFIA
                            case "353": // Serigrafia 1  -  B13
                                xlWorkSheetSerigrafia = _HelpExcel.AddImageToSheet(xlWorkSheetSerigrafia, 12, 1, 15, 15, 340, 250, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetSerigrafia, 23, 210, 260, 190, strArchivo);
                                break;
                            case "354": // Serigrafia 2  -  K13
                                xlWorkSheetSerigrafia = _HelpExcel.AddImageToSheet(xlWorkSheetSerigrafia, 12, 10, 15, 15, 340, 250, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetSerigrafia, 368, 210, 260, 190, strArchivo);
                                break;
                            case "355": // Serigrafia 3  -  B31
                                xlWorkSheetSerigrafia = _HelpExcel.AddImageToSheet(xlWorkSheetSerigrafia, 30, 1, 15, 15, 340, 250, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                // cargarImagenHoja(xlWorkSheetSerigrafia, 23, 450, 260, 200, strArchivo);
                                break;
                            case "356": // Serigrafia 4  - K31
                                xlWorkSheetSerigrafia = _HelpExcel.AddImageToSheet(xlWorkSheetSerigrafia, 30, 10, 15, 15, 340, 250, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetSerigrafia, 368, 450, 260, 200, strArchivo);
                                break;

                            // HOJA PC
                            case "363": // PC 1  -  Image 1  - C13
                                xlWorkSheetPC = _HelpExcel.AddImageToSheet(xlWorkSheetPC, 12, 2, 15, 15, 500, 230, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetPC, 120, 205, 380, 195, strArchivo);
                                break;
                            case "364": // PC 2  -  Image 2 - C31
                                xlWorkSheetPC = _HelpExcel.AddImageToSheet(xlWorkSheetPC, 30, 2, 15, 15, 500, 230, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                // cargarImagenHoja(xlWorkSheetPC, 120, 450, 380, 200, strArchivo);
                                break;

                            // HOJA TORKER
                            case "647": //  TORKER TEST 1
                                xlWorkSheetTorker = _HelpExcel.AddImageToSheet(xlWorkSheetTorker, 12, 2, 15, 15, 450, 220, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetSticker, 120, 210, 380, 185, strArchivo);
                                break;
                            case "704": //  TORKER TEST 2
                                xlWorkSheetTorker = _HelpExcel.AddImageToSheet(xlWorkSheetTorker, 26, 2, 15, 15, 450, 190, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetSticker, 120, 210, 380, 185, strArchivo);
                                break;

                            // HOJA STICKER
                            case "388": //  -C13
                                xlWorkSheetSticker = _HelpExcel.AddImageToSheet(xlWorkSheetSticker, 12, 2, 15, 15, 480, 240, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetSticker, 120, 210, 380, 185, strArchivo);
                                break;
                            default:
                                break;
                        }
                    }


                    // Guardamos el archivo de Excel con los cambios
                    FileInfo resultadoArchivo = new FileInfo(ResultadoRuta);
                    excelPackage.SaveAs(resultadoArchivo);
                }

                result = true;
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            /*
          
                */
            return result;
        }

        public bool CargarExcelFormato01(string PlantillaMacro, string ResultadoRuta, CertificadoIf Certificado, PiezaSap Pieza, List<CertificadoIfdimension> LstDimensional, List<CertificadoIfapariencias> LstApariencia, List<PiezaConcesion> LstDefecto, List<InspeccionOptica> LstOpticos)
        {
            bool result = false;
            FileInfo plantillaArchivo = new FileInfo(PlantillaMacro);
            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage(plantillaArchivo))
                {
                    // Obtenemos la hoja de cálculo del archivo Excel
                    ExcelWorksheet xlWorkSheetDatos = excelPackage.Workbook.Worksheets["DATOS"];
                    ExcelWorksheet xlWorkSheetTermo = excelPackage.Workbook.Worksheets["TERMOGRAFIA"];
                    ExcelWorksheet xlWorkSheetDistorsion = excelPackage.Workbook.Worksheets["DISTORSION"];
                    ExcelWorksheet xlWorkSheetDobleImagen = excelPackage.Workbook.Worksheets["DOBLE IMAGEN"];
                    ExcelWorksheet xlWorkSheetAccesories = excelPackage.Workbook.Worksheets["ACCESORIOS"];
                    ExcelWorksheet xlWorkSheetSerigrafia = excelPackage.Workbook.Worksheets["SERIGRAPHY"];
                    ExcelWorksheet xlWorkSheetBN = excelPackage.Workbook.Worksheets["BN"];
                    ExcelWorksheet xlWorkSheetPC = excelPackage.Workbook.Worksheets["PC"];
                    ExcelWorksheet xlWorkSheetSticker = excelPackage.Workbook.Worksheets["STICKER ADUANA"];
                    ExcelWorksheet xlWorkSheetBNEditable = excelPackage.Workbook.Worksheets["BN EDITABLE"];

                    string responsableApariencia = (LstApariencia.Count > 0) ? LstApariencia[0].UsuarioCrea : "";
                    //worksheet.Cells["A1"].Value = texto;

     
                    xlWorkSheetDatos.Cells["AE7"].Value = Pieza.LoteLogistico;
                    xlWorkSheetDatos.Cells["N84"].Value = responsableApariencia;
                    xlWorkSheetDatos.Cells["AS85"].Value = DateTime.Now.Day.ToString();
                    xlWorkSheetDatos.Cells["AT85"].Value = DateTime.Now.Month.ToString();
                    xlWorkSheetDatos.Cells["AU85"].Value = DateTime.Now.Year.ToString();

                    xlWorkSheetTermo.Cells["M4"].Value = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
                    xlWorkSheetTermo.Cells["F37"].Value = responsableApariencia;
                    xlWorkSheetDistorsion.Cells["E76"].Value = responsableApariencia;
                    xlWorkSheetDobleImagen.Cells["D46"].Value = responsableApariencia;
                    xlWorkSheetAccesories.Cells["F58"].Value = responsableApariencia;
                    xlWorkSheetSerigrafia.Cells["F48"].Value = responsableApariencia;
                    xlWorkSheetBN.Cells["F48"].Value = responsableApariencia;
                    xlWorkSheetSticker.Cells["F31"].Value = responsableApariencia;
                    xlWorkSheetPC.Cells["F48"].Value = responsableApariencia;


                    //CARGAR IMAGEN DEFECTO
                    if (Pieza.DefectoImagen != "" && Pieza.DefectoImagen != null)
                    {
                        _HelpExcel.AddImageToSheet(xlWorkSheetDatos, 12, 2, 10, 10, 790, 380, Pieza.DefectoImagen, Pieza.Id.ToString());
                        //cargarImagenHoja(xlWorkSheetDatos, 50, 200, 790, 380, Pieza.DefectoImagen); // todo:borrar
                    }


                    //Cargar los defectos
                    int intContador = 49;
                    foreach (var item in LstDefecto)
                    {
                        xlWorkSheetDatos.Cells["E" + intContador.ToString()].Value = item.Zona;
                        xlWorkSheetDatos.Cells["K" + intContador.ToString()].Value = item.DefectoMaestro.Defecto;
                        xlWorkSheetDatos.Cells["AK" + intContador.ToString()].Value = item.Tamanio;
                        xlWorkSheetDatos.Cells["AP" + intContador.ToString()].Value = "";
                        xlWorkSheetDatos.Cells["AR" + intContador.ToString()].Value = item.Observacion;
                        ++intContador;
                    }




                    //Cargar la inspeccion de apariencia
                    foreach (var item in LstApariencia)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {
                            case "322": //  AISLAMIENTO DE TERMINALES - K63
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "K63" : "V63")].Value = "X";
                                break;
                            case "368": //  Terminales - K64
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "K64" : "V64")].Value = "X";
                                break;
                            case "266": //  Acabado y limpieza de bordes - K65
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "K65" : "V65")].Value = "X";
                                break;
                            case "323": //  SOPORTE ESPEJO  MIRROR BRACKET - K66
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "K66" : "V66")].Value = "X";
                                break;
                            case "297": //   Espesor X5 (PBS)    THICKNEES K67
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K67"].Value = item.Valor;
                                break;
                            case "298": //  Espesor X5 (LD) THICKNEES   K67
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K67"].Value = item.Valor;
                                break;
                            case "299": //  Espesor X5 (LE) THICKNEES
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K67"].Value = item.Valor;
                                break;
                            case "300": //  Espesor X5 (CABINA/PARTICIÓN)   THICKNEES
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K67"].Value = item.Valor;
                                break;
                            case "259": //  Apariencia serigrafia   SERIGRAPHY K68
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "K68" : "V68")].Value = "X";
                                break;
                            case "257": // Logo    LOGO (INTERNAL BLACK) AL63
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL63" : "AS63")].Value = "X";
                                break;
                            case "262": // BRILLIANT ROUNDED / Tipo de borde vidrio pintura
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL64" : "AS64")].Value = "X";

                                break;
                            case "367": // Reflexion   REFLECTION
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL65" : "AS65")].Value = "X";
                                break;
                            case "324": // BANDA NEGRA INTERNA BLACK BAND INTERNAL
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL66" : "AS66")].Value = "X";
                                break;
                            case "325": // BANDA NEGRA EXTERNA BLACK BAND EXTERNAL
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL67" : "AS67")].Value = "X";
                                break;

                            default:
                                break;
                        }
                    }



                    // Cargar la inspeccion dimensional

                    foreach (var item in LstDimensional)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {
                            case "334": // Zona A (Mdop)  - Zona A - E26
                                xlWorkSheetDistorsion.Cells["E26"].Value = item.Val1;
                                xlWorkSheetDistorsion.Cells["E27"].Value = item.Val2;
                                xlWorkSheetDistorsion.Cells["F26"].Value = item.Val3;
                                xlWorkSheetDistorsion.Cells["F27"].Value = item.Val4;
                                break;
                            case "335": // Zona B (Mdop)  - Zona B - G26
                                xlWorkSheetDistorsion.Cells["G26"].Value = item.Val1;
                                xlWorkSheetDistorsion.Cells["G27"].Value = item.Val2;
                                xlWorkSheetDistorsion.Cells["H26"].Value = item.Val3;
                                xlWorkSheetDistorsion.Cells["H27"].Value = item.Val4;
                                break;
                            case "389": // banda negra externa A  - BLACK BAND EXTERNAL - RECUADRO A -  D24
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 22, 4, 24, 12,-25,0);
                                //cargarShapeHoja(xlWorkSheetBN, 23, 4, 14, item.Val1, -3);
                                break;
                            case "390": // banda negra externa B -  BLACK BAND EXTERNAL - RECUADRO B  - L15
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 16, 11, 24, 12,4,-4);
                                //xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, 17, 12, 10, item.Val1, 1);
                                //cargarShapeHoja(xlWorkSheetBN, 17, 12, 10, item.Val1, 1);
                                break;
                            case "391": // banda negra externa C -  BLACK BAND EXTERNAL - RECUADRO C  - M24
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 23, 12, 45, 12);
                                //cargarShapeHoja(xlWorkSheetBN, 24, 13, 26, item.Val1, 0);
                                break;

                            case "392": // banda negra interna A -  BLACK BAND NTERNAL - RECUADRO A - E41
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 40, 3, 10, 12);
                                //cargarShapeHoja(xlWorkSheetBN, 41, 4, 19, item.Val1, -5);
                                break;
                            case "393": // banda negra interna B -  BLACK BAND INTERNAL - RECUADRO B - L36
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 34, 11, 24, 12);
                                //cargarShapeHoja(xlWorkSheetBN, 35, 12, 14, item.Val1, 10);
                                break;
                            case "394": // banda negra interna C -  BLACK BAND INTERNAL - RECUADRO C - M41
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 40, 12, 20, 12);
                                //cargarShapeHoja(xlWorkSheetBN, 41, 13, 30, item.Val1, 6);
                                break;
                            //case "352": // banda negra interna D -  BLACK BAND INTERNAL - RECUADRO B
                            //    cargarShapeHoja(xlWorkSheetBN, 36, 12, 0, item.Val1);
                            //   break;
                            case "381": // RESISTENCIA ELECTRICIA LD - INSPECCION ELECTRICA - M15
                                xlWorkSheetTermo.Cells["M15"].Value = item.Val1;
                                break;
                            default:
                                break;
                        }
                    }


                    // Cargar Inspeccion optica

                    string strArchivo = "";
                    foreach (var item in LstOpticos)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {
                            // HOJA TERMO
                            case "395": // Termografia LDS  -  Inspeccion Optico - B17
                                xlWorkSheetTermo = _HelpExcel.AddImageToSheet(xlWorkSheetTermo, 17, 2, 15, 15, 200, 140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetTermo, 215, 280, 200, 140, strArchivo);
                                break;
                            case "380": // Terminal LD  - Inspeccion Optico -  B30
                                xlWorkSheetTermo = _HelpExcel.AddImageToSheet(xlWorkSheetTermo, 30, 2, 15, 15, 200, 140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetTermo, 215, 460, 200, 140, strArchivo);
                                break;

                            // HOJA DISTORSION
                            case "270": // Distorsion con cebra 0 grados -  DISTORTION IMAGE AT 0° - B42
                                xlWorkSheetDistorsion = _HelpExcel.AddImageToSheet(xlWorkSheetDistorsion, 42, 2, 15, 15, 200, 140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetDistorsion, 40, 400, 200, 140, strArchivo);
                                break;
                            case "271": // Distorsion con cebra 45 grados - DISTORTION IMAGE AT 45° - G42
                                xlWorkSheetDistorsion = _HelpExcel.AddImageToSheet(xlWorkSheetDistorsion, 42, 6, 15, 15, 200, 140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetDistorsion, 412, 400, 200, 140, strArchivo);
                                break;
                            case "273": // Reflexion 1 - REFLECTION 1  - B57
                                xlWorkSheetDistorsion = _HelpExcel.AddImageToSheet(xlWorkSheetDistorsion, 56, 1, 15, 15, 200, 140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetDistorsion, 40, 590, 200, 140, strArchivo);
                                break;
                            case "336": // Reflexion 2 - REFLECTION 2  -  G57
                                xlWorkSheetDistorsion = _HelpExcel.AddImageToSheet(xlWorkSheetDistorsion, 56, 6, 15, 15, 200, 140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetDistorsion, 412, 590, 200, 140, strArchivo);
                                break;

                            // HOJA DOBLE IMAGEN
                            case "272": // Doble vision  -  DOUBLE IMAGE   -  D28
                                xlWorkSheetDobleImagen = _HelpExcel.AddImageToSheet(xlWorkSheetDobleImagen, 27, 3, 15, 15, 200, 158, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                // cargarImagenHoja(xlWorkSheetDobleImagen, 210, 350, 200, 150, strArchivo);
                                break;

                            // HOJA ACCESORIES
                            case "342": // Acero 1 - STEELS - E13
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 12, 4, 15, 15, 260, 160, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 200, 210, 260, 160, strArchivo);
                                break;
                            case "343": // Acero 2 - STEELS - E28
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 28, 4, 15, 15, 260, 170, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 200, 410, 260, 170, strArchivo);
                                break;
                            case "344": // Acero 3 - STEELS - E43
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 42, 4, 15, 15, 260, 180, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 200, 610, 260, 180, strArchivo);
                                break;

                            // HOJA SERIGRAFIA
                            case "353": // Serigrafia 1  -  B13
                                xlWorkSheetSerigrafia = _HelpExcel.AddImageToSheet(xlWorkSheetSerigrafia, 12, 1, 15, 15, 260, 195, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetSerigrafia, 22, 205, 265, 195, strArchivo);
                                break;
                            case "354": // Serigrafia 2 - K13
                                xlWorkSheetSerigrafia = _HelpExcel.AddImageToSheet(xlWorkSheetSerigrafia, 12, 10, 15, 15, 260, 195, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetSerigrafia, 365, 205, 265, 195, strArchivo);
                                break;
                            case "355": // Serigrafia 3  - B31
                                xlWorkSheetSerigrafia = _HelpExcel.AddImageToSheet(xlWorkSheetSerigrafia, 30, 1, 15, 15, 260, 205, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetSerigrafia, 22, 450, 265, 205, strArchivo);
                                break;
                            case "356": // Serigrafia 4  - K31
                                xlWorkSheetSerigrafia = _HelpExcel.AddImageToSheet(xlWorkSheetSerigrafia, 30, 10, 15, 15, 260, 195, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetSerigrafia, 365, 450, 265, 205, strArchivo);
                                break;

                            // HOJA PC
                            case "363": // PC 1  -  Image 1  - C13
                                xlWorkSheetPC = _HelpExcel.AddImageToSheet(xlWorkSheetPC, 12, 2, 15, 15, 388, 195, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetPC, 120, 207, 380, 195, strArchivo);
                                break;
                            case "364": // PC 2  -  Image 2 - C31
                                xlWorkSheetPC = _HelpExcel.AddImageToSheet(xlWorkSheetPC, 30, 2, 15, 15, 388, 195, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetPC, 120, 450, 380, 200, strArchivo);
                                break;

                            // HOJA STICKER
                            case "388": //  - C13
                                xlWorkSheetSticker = _HelpExcel.AddImageToSheet(xlWorkSheetSticker, 12, 2, 15, 15, 380, 185, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetSticker, 120, 210, 380, 185, strArchivo);
                                break;
                            default:
                                break;
                        }
                    }


                    // Guardamos el archivo de Excel con los cambios
                    FileInfo resultadoArchivo = new FileInfo(ResultadoRuta);
                    excelPackage.SaveAs(resultadoArchivo);
                }

                result = true;
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            /*
          
                */
            return result;
        }

        public bool CargarExcelFormato02(string PlantillaMacro, string ResultadoRuta, CertificadoIf Certificado, PiezaSap Pieza, List<CertificadoIfdimension> LstDimensional, List<CertificadoIfapariencias> LstApariencia, List<PiezaConcesion> LstDefecto, List<InspeccionOptica> LstOpticos)
        {
            bool result = false;
            FileInfo plantillaArchivo = new FileInfo(PlantillaMacro);
            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage(plantillaArchivo))
                {
                    // Obtenemos la hoja de cálculo del archivo Excel
                    ExcelWorksheet xlWorkSheetDatos = excelPackage.Workbook.Worksheets["DATOS"];
                    ExcelWorksheet xlWorkSheetTermo = excelPackage.Workbook.Worksheets["TERMOGRAFIA"];
                    ExcelWorksheet xlWorkSheetDistorsion = excelPackage.Workbook.Worksheets["DISTORSION"];
                    ExcelWorksheet xlWorkSheetDobleImagen = excelPackage.Workbook.Worksheets["DOBLE IMAGEN"];
                    ExcelWorksheet xlWorkSheetAccesories = excelPackage.Workbook.Worksheets["ACCESORIOS"];
                    ExcelWorksheet xlWorkSheetBN = excelPackage.Workbook.Worksheets["BN"];
                    ExcelWorksheet xlWorkSheetSerigrafia = excelPackage.Workbook.Worksheets["SERIGRAPHY"];
                  //  ExcelWorksheet xlWorkSheetTNT = excelPackage.Workbook.Worksheets["TNT"];
                    ExcelWorksheet xlWorkSheetPC = excelPackage.Workbook.Worksheets["PC"];
                  //  ExcelWorksheet xlWorkSheetTorker = excelPackage.Workbook.Worksheets["TORKER"];
                    ExcelWorksheet xlWorkSheetSticker = excelPackage.Workbook.Worksheets["STICKER ADUANA"];
                    ExcelWorksheet xlWorkSheetBNEditable = excelPackage.Workbook.Worksheets["BN EDITABLE"];


                    string responsableApariencia = (LstApariencia.Count > 0) ? LstApariencia[0].UsuarioCrea : "";
                    //worksheet.Cells["A1"].Value = texto;


                    xlWorkSheetDatos.Cells["E7"].Value = Pieza.Cliente;  // CLIENT
                    xlWorkSheetDatos.Cells["E8"].Value = Pieza.Vehiculo;  // VEHICLE
                    xlWorkSheetDatos.Cells["E9"].Value = Pieza.Vidrio +" / "+ Pieza.Modelo;  // Part

                    xlWorkSheetDatos.Cells["AE7"].Value = Pieza.LoteLogistico; //Production Lot
                    xlWorkSheetDatos.Cells["AE8"].Value = Pieza.Formula; //Composition
                    //xlWorkSheetDatos.Cells["AE9"].Value = Pieza.LoteLogistico; //Other Glass finish

                    xlWorkSheetDatos.Cells["AR7"].Value = Pieza.Color; //Color
                    xlWorkSheetDatos.Cells["AR8"].Value = Pieza.PartNumber; //Part Number
                    //xlWorkSheetDatos.Cells["AR9"].Value = Pieza.LoteLogistico; // PACKAGE EDGE TYPE:		

                    xlWorkSheetDatos.Cells["N83"].Value = responsableApariencia; // Reviewed By
                    xlWorkSheetDatos.Cells["AS84"].Value = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();


                    //xlWorkSheetDatos.Cells["AS85"].Value = DateTime.Now.Day.ToString();
                    //xlWorkSheetDatos.Cells["AT85"].Value = DateTime.Now.Month.ToString();
                    //xlWorkSheetDatos.Cells["AU85"].Value = DateTime.Now.Year.ToString();

                    /*
                    xlWorkSheetDatos.Cells["F37"].Value = responsableApariencia;
                    xlWorkSheetDatos.Cells["E76"].Value = responsableApariencia;
                    xlWorkSheetDatos.Cells["D46"].Value = responsableApariencia;

                    xlWorkSheetDatos.Cells["F58"].Value = responsableApariencia;
                    xlWorkSheetDatos.Cells["F48"].Value = responsableApariencia;
                    xlWorkSheetDatos.Cells["F48"].Value = responsableApariencia;
                    xlWorkSheetDatos.Cells["F31"].Value = responsableApariencia;
                    xlWorkSheetDatos.Cells["F48"].Value = responsableApariencia;
                    */



                    //xlWorkSheetTermo.Cells["M4"].Value = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();


                    //CARGAR IMAGEN DEFECTO  -- B11
                    if (Pieza.DefectoImagen != "" && Pieza.DefectoImagen != null)
                    {
                        _HelpExcel.AddImageToSheet(xlWorkSheetDatos, 11, 2, 10, 10, 790, 380, Pieza.DefectoImagen, Pieza.Id.ToString());

                        //cargarImagenHoja(xlWorkSheetDatos, 50, 200, 790, 380, Pieza.DefectoImagen); // todo:borrar
                    }


                    //Cargar los defectos
                    int intContador = 49;
                    foreach (var item in LstDefecto)
                    {
                        xlWorkSheetDatos.Cells["E" + intContador.ToString()].Value = item.Zona;
                        xlWorkSheetDatos.Cells["K" + intContador.ToString()].Value = item.DefectoMaestro.Defecto;
                        xlWorkSheetDatos.Cells["H" + intContador.ToString()].Value = item.DefectoMaestro.Id;
                        xlWorkSheetDatos.Cells["AK" + intContador.ToString()].Value = item.Tamanio;
                        xlWorkSheetDatos.Cells["AP" + intContador.ToString()].Value = 1;
                        xlWorkSheetDatos.Cells["AR" + intContador.ToString()].Value = item.Observacion;
                        ++intContador;
                    }


                    //Cargar la inspeccion de apariencia

                    
                    foreach (var item in LstApariencia)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {
                            case "322": //  AISLAMIENTO DE TERMINALES - K63
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "K63" : "V63")].Value = "X";
                                break;
                            case "368": //  Terminales - K64
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "K64" : "V64")].Value = "X";
                                break;
                            case "266": //  Acabado y limpieza de bordes - K65
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "K65" : "V65")].Value = "X";
                                break;
                            case "323": //  SOPORTE ESPEJO  MIRROR BRACKET - K66
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "K66" : "V66")].Value = "X";
                                break;
                            case "297": //   Espesor X5 (PBS)    THICKNEES K67
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K67"].Value = item.Valor;
                                break;
                            case "298": //  Espesor X5 (LD) THICKNEES   K67
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K67"].Value = item.Valor;
                                break;
                            case "299": //  Espesor X5 (LE) THICKNEES
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K67"].Value = item.Valor;
                                break;
                            case "300": //  Espesor X5 (CABINA/PARTICIÓN)   THICKNEES
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K67"].Value = item.Valor;
                                break;
                            case "259": //  Apariencia serigrafia   SERIGRAPHY K68
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "K68" : "V68")].Value = "X";
                                break;
                            case "257": // Logo    LOGO (INTERNAL BLACK) AL63
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL63" : "AS63")].Value = "X";
                                break;
                            case "263": // Chaflan CHAMFER
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL64" : "AS64")].Value = "X";
                                break;
                            case "367": // Reflexion   REFLECTION
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL65" : "AS65")].Value = "X";
                                break;
                            case "324": // BANDA NEGRA INTERNA BLACK BAND INTERNAL
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL66" : "AS66")].Value = "X";
                                break;
                            case "325": // BANDA NEGRA EXTERNA BLACK BAND EXTERNAL
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL67" : "AS67")].Value = "X";
                                break;
                            case "262": // BRILLIANT ROUNDED / Tipo de borde vidrio pintura
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL64" : "AS64")].Value = "X";

                                break;
                            default:
                                break;
                        }
                    }


                    // Cargar la inspeccion dimensional

                    foreach (var item in LstDimensional)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {
                            case "334": // Zona A (Mdop)  - Zona A - E26
                                xlWorkSheetDistorsion.Cells["E26"].Value = item.Val1;
                                xlWorkSheetDistorsion.Cells["E27"].Value = item.Val2;
                                xlWorkSheetDistorsion.Cells["F26"].Value = item.Val3;
                                xlWorkSheetDistorsion.Cells["F27"].Value = item.Val4;
                                break;
                            case "335": // Zona B (Mdop)  - Zona B - G26
                                xlWorkSheetDistorsion.Cells["G26"].Value = item.Val1;
                                xlWorkSheetDistorsion.Cells["G27"].Value = item.Val2;
                                xlWorkSheetDistorsion.Cells["H26"].Value = item.Val3;
                                xlWorkSheetDistorsion.Cells["H27"].Value = item.Val4;
                                break;
                            case "389": // banda negra externa A  - BLACK BAND EXTERNAL - RECUADRO A  - E24
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 23, 4, 24, 12);
                                // cargarShapeHoja(xlWorkSheetBN, 23, 4, 16, item.Val1, 4);
                                break;
                            case "390": // banda negra externa B -  BLACK BAND EXTERNAL - RECUADRO B - L18
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 17, 11, 24, 12);
                                //cargarShapeHoja(xlWorkSheetBN, 17, 12, 13, item.Val1, 5);
                                break;
                            case "391": // banda negra externa C -  BLACK BAND EXTERNAL - RECUADRO C  - M25
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 24, 12, 24, 12);
                                //cargarShapeHoja(xlWorkSheetBN, 24, 13, 35, item.Val1, 6);
                                break;

                            case "392": // banda negra interna A -  BLACK BAND NTERNAL - RECUADRO A  -   E41
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 40, 4, 24, 12);
                                //cargarShapeHoja(xlWorkSheetBN, 41, 4, 20, item.Val1, 1);
                                break;
                            case "393": // banda negra interna B -  BLACK BAND INTERNAL - RECUADRO B -  L36
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 35, 11, 24, 12);
                                //cargarShapeHoja(xlWorkSheetBN, 35, 12, 16, item.Val1, 14);
                                break;
                            case "394": // banda negra interna C -  BLACK BAND INTERNAL - RECUADRO C  - M41
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 40, 12, 24, 12);
                                //cargarShapeHoja(xlWorkSheetBN, 41, 13, 32, item.Val1, 12);
                                break;
                            //case "352": // banda negra interna D -  BLACK BAND INTERNAL - RECUADRO B
                            //    cargarShapeHoja(xlWorkSheetBN, 36, 12, 0, item.Val1);
                            //   break;
                            case "381": // RESISTENCIA ELECTRICIA LD - INSPECCION ELECTRICA - M15
                                xlWorkSheetTermo.Cells["M15"].Value = item.Val1;
                                break;
                            default:
                                break;
                        }
                    }





                    // Cargar Inspeccion optica
                    
                    string strArchivo = "";
                    foreach (var item in LstOpticos)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {
                            // HOJA TERMO
                            case "329 ": // Left Thermograph  -  Inspeccion Optico - B20
                                xlWorkSheetTermo = _HelpExcel.AddImageToSheet(xlWorkSheetTermo, 20, 1, 15, 15, 200, 140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetTermo, 35, 280, 200, 140, strArchivo);
                                break;
                            case "330": // Right Thermograph - Inspeccion Optico -  E20
                                xlWorkSheetTermo = _HelpExcel.AddImageToSheet(xlWorkSheetTermo, 20, 4, 15, 15, 200, 140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                // cargarImagenHoja(xlWorkSheetTermo, 35, 460, 200, 140, strArchivo);
                                break;
                            case "369": // Camera Thermograph- Inspeccion Optico -  B17
                                xlWorkSheetTermo = _HelpExcel.AddImageToSheet(xlWorkSheetTermo, 16, 3, 15, 15, 400, 190, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                // cargarImagenHoja(xlWorkSheetTermo, 35, 460, 200, 140, strArchivo);
                                break;

                            case "331": // UPPER CENTRAL  Terminals - Inspeccion Optico -  B34
                                xlWorkSheetTermo = _HelpExcel.AddImageToSheet(xlWorkSheetTermo, 33, 1, 15, 15, 200, 140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                // cargarImagenHoja(xlWorkSheetTermo, 35, 460, 200, 140, strArchivo);
                                break;
                            case "332": // UPPER CENTRAL  Terminals - Inspeccion Optico -  E34
                                xlWorkSheetTermo = _HelpExcel.AddImageToSheet(xlWorkSheetTermo, 33, 4, 15, 15, 200, 140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                // cargarImagenHoja(xlWorkSheetTermo, 35, 460, 200, 140, strArchivo);
                                break;
                            case "333": // LOWER RIGHT  Terminals - Inspeccion Optico -  L34
                                xlWorkSheetTermo = _HelpExcel.AddImageToSheet(xlWorkSheetTermo, 33, 11, 15, 15, 200, 140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                // cargarImagenHoja(xlWorkSheetTermo, 35, 460, 200, 140, strArchivo);
                                break;
                            
                            case "380": // TERMINALES LD - Inspeccion Optico -  B30
                                xlWorkSheetTermo = _HelpExcel.AddImageToSheet(xlWorkSheetTermo, 29, 3, 15, 15, 400, 190, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                // cargarImagenHoja(xlWorkSheetTermo, 35, 460, 200, 140, strArchivo);
                                break;

                            /*
                        case "395": // Termografia LDS  -  Inspeccion Optico - B15
                            xlWorkSheetTermo = _HelpExcel.AddImageToSheet(xlWorkSheetTermo, 18, 5, 15, 15, 200, 140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                            //cargarImagenHoja(xlWorkSheetTermo, 35, 280, 200, 140, strArchivo);
                            break;
                        case "380": // Terminal LD  - Inspeccion Optico -  B30
                            xlWorkSheetTermo = _HelpExcel.AddImageToSheet(xlWorkSheetTermo, 30, 5, 15, 15, 200, 140, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                            // cargarImagenHoja(xlWorkSheetTermo, 35, 460, 200, 140, strArchivo);
                            break; */

                            // HOJA DISTORSION
                            case "270": // Distorsion con cebra 0 grados -  DISTORTION IMAGE AT 0° - B41
                                xlWorkSheetDistorsion = _HelpExcel.AddImageToSheet(xlWorkSheetDistorsion, 41, 1, 0, 15, 280, 200, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetDistorsion, 40, 398, 200, 140, strArchivo);
                                break;

                            case "269": // Distorsion sin zebra 45 grados - DISTORTION WHIOUT ZEBRA - E41
                                xlWorkSheetDistorsion = _HelpExcel.AddImageToSheet(xlWorkSheetDistorsion, 41, 6, 15, 15, 280, 200, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetDistorsion, 415, 398, 200, 140, strArchivo);
                                break; 
                            case "271": // Distorsion con cebra 45 grados - DISTORTION IMAGE AT 45° - G42
                                xlWorkSheetDistorsion = _HelpExcel.AddImageToSheet(xlWorkSheetDistorsion, 41, 6, 15, 15, 280, 200, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetDistorsion, 415, 398, 200, 140, strArchivo);
                                break;

                            case "273": // Reflexion 1 - REFLECTION 1  - B52
                                xlWorkSheetDistorsion = _HelpExcel.AddImageToSheet(xlWorkSheetDistorsion, 56, 1, 0, 15, 280, 200, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetDistorsion, 40, 590, 200, 140, strArchivo);
                                break;
                            case "365": // LANDSCAPE DISTORTION  -  E52
                                xlWorkSheetDistorsion = _HelpExcel.AddImageToSheet(xlWorkSheetDistorsion, 56, 6, 15, 15, 280, 200, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetDistorsion, 415, 590, 200, 140, strArchivo);
                                break;
                            case "336": // Reflexion 2 - REFLECTION 2  -  E52
                                xlWorkSheetDistorsion = _HelpExcel.AddImageToSheet(xlWorkSheetDistorsion, 56, 6, 15, 15, 280, 200, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetDistorsion, 415, 590, 200, 140, strArchivo);
                                break;

                            // HOJA DOBLE IMAGEN
                            case "272": // Doble vision  -  DOUBLE IMAGE   -  D28
                                xlWorkSheetDobleImagen = _HelpExcel.AddImageToSheet(xlWorkSheetDobleImagen, 28, 3, 15, 15, 260, 190, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                // cargarImagenHoja(xlWorkSheetDobleImagen, 210, 350, 200, 150, strArchivo);
                                break;

                            // HOJA ACCESORIES
                            case "342": // Acero 1 - STEELS - C27
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 12, 4, 15, 15, 280, 185, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 205, 210, 250, 160, strArchivo);
                                break;
                            case "343": // Acero 2 - STEELS - G27
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 27, 4, 15, 15, 280, 185, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 205, 412, 250, 170, strArchivo);
                                break;
                            case "344": // Acero 3 - STEELS - L27
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 43, 4, 15, 15, 280, 185, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 205, 625, 250, 180, strArchivo);
                                break;

                            case "341": // MIRROR BRACKET - STEELS - B32
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 31, 1, 15, 15, 170, 115, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 205, 625, 250, 180, strArchivo);
                                break;
                            case "703": // CAMARA BRACKET - STEELS - D32
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 31, 3, 15, 15, 170, 115, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 205, 625, 250, 180, strArchivo);
                                break;
                            case "701": // CABLE OUTPUT (PINS) - STEELS - I32
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 31, 8, 15, 15, 170, 115, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 205, 625, 250, 180, strArchivo);
                                break;
                            case "725": // CONNECTORS - STEELS - M32
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 31, 12, 15, 15, 180, 115, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 205, 625, 250, 180, strArchivo);
                                break;


                            // HOJA SERIGRAFIA
                            case "353": // Serigrafia 1  -  B13
                                xlWorkSheetSerigrafia = _HelpExcel.AddImageToSheet(xlWorkSheetSerigrafia, 12, 1, 15, 15, 340, 250, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetSerigrafia, 23, 210, 260, 190, strArchivo);
                                break;
                            case "354": // Serigrafia 2  -  K13
                                xlWorkSheetSerigrafia = _HelpExcel.AddImageToSheet(xlWorkSheetSerigrafia, 12, 10, 15, 15, 340, 250, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetSerigrafia, 368, 210, 260, 190, strArchivo);
                                break;
                            case "355": // Serigrafia 3  -  B31
                                xlWorkSheetSerigrafia = _HelpExcel.AddImageToSheet(xlWorkSheetSerigrafia, 30, 1, 15, 15, 340, 250, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                // cargarImagenHoja(xlWorkSheetSerigrafia, 23, 450, 260, 200, strArchivo);
                                break;
                            case "356": // Serigrafia 4  - K31
                                xlWorkSheetSerigrafia = _HelpExcel.AddImageToSheet(xlWorkSheetSerigrafia, 30, 10, 15, 15, 340, 250, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetSerigrafia, 368, 450, 260, 200, strArchivo);
                                break;

                            // HOJA PC
                            case "363": // PC 1  -  Image 1  - C13
                                xlWorkSheetPC = _HelpExcel.AddImageToSheet(xlWorkSheetPC, 12, 2, 15, 15, 500, 230, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetPC, 120, 205, 380, 195, strArchivo);
                                break;
                            case "364": // PC 2  -  Image 2 - C31
                                xlWorkSheetPC = _HelpExcel.AddImageToSheet(xlWorkSheetPC, 30, 2, 15, 15, 500, 230, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                // cargarImagenHoja(xlWorkSheetPC, 120, 450, 380, 200, strArchivo);
                                break;

                            // HOJA TORKER
                            case "647": //  TORKER TEST 1
                               // xlWorkSheetTorker = _HelpExcel.AddImageToSheet(xlWorkSheetTorker, 12, 2, 15, 15, 450, 220, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetSticker, 120, 210, 380, 185, strArchivo);
                                break;
                            case "704": //  TORKER TEST 2
                                //xlWorkSheetTorker = _HelpExcel.AddImageToSheet(xlWorkSheetTorker, 26, 2, 15, 15, 450, 190, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetSticker, 120, 210, 380, 185, strArchivo);
                                break;

                            // HOJA STICKER
                            case "388": //  -C13
                                xlWorkSheetSticker = _HelpExcel.AddImageToSheet(xlWorkSheetSticker, 12, 2, 15, 15, 480, 240, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetSticker, 120, 210, 380, 185, strArchivo);
                                break;
                            default:
                                break;
                        }
                    }
                    

                    // Guardamos el archivo de Excel con los cambios
                    FileInfo resultadoArchivo = new FileInfo(ResultadoRuta);
                    excelPackage.SaveAs(resultadoArchivo);
                }

                result = true;
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            /*
          
                */
            return result;
        }

        public bool CargarExcelFormato07(string PlantillaMacro, string ResultadoRuta, CertificadoIf Certificado, PiezaSap Pieza, List<CertificadoIfdimension> LstDimensional, List<CertificadoIfapariencias> LstApariencia, List<PiezaConcesion> LstDefecto, List<InspeccionOptica> LstOpticos)
        {
            bool result = false;
            FileInfo plantillaArchivo = new FileInfo(PlantillaMacro);
            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage(plantillaArchivo))
                {
                    // Obtenemos la hoja de cálculo del archivo Excel
                    ExcelWorksheet xlWorkSheetDatos = excelPackage.Workbook.Worksheets["DATOS"];
                    ExcelWorksheet xlWorkSheetBN = excelPackage.Workbook.Worksheets["BN"];
                    ExcelWorksheet xlWorkSheetSticker = excelPackage.Workbook.Worksheets["STICKER ADUANA"];
                    

                    string responsableApariencia = (LstApariencia.Count > 0) ? LstApariencia[0].UsuarioCrea : "";
                    //worksheet.Cells["A1"].Value = texto;



                    xlWorkSheetDatos.Cells["AE7"].Value = Pieza.LoteLogistico;
                    xlWorkSheetDatos.Cells["N79"].Value = responsableApariencia;
                    xlWorkSheetDatos.Cells["AS80"].Value = DateTime.Now.Day.ToString();
                    xlWorkSheetDatos.Cells["AT80"].Value = DateTime.Now.Month.ToString();
                    xlWorkSheetDatos.Cells["AU80"].Value = DateTime.Now.Year.ToString();

                    xlWorkSheetDatos.Cells["M4"].Value = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
                    xlWorkSheetDatos.Cells["F48"].Value = responsableApariencia;

                    xlWorkSheetSticker.Cells["F31"].Value = responsableApariencia;






                    //CARGAR IMAGEN DEFECTO  -- B11
                    if (Pieza.DefectoImagen != "" && Pieza.DefectoImagen != null)  // /// B11
                    {
                        _HelpExcel.AddImageToSheet(xlWorkSheetDatos, 11, 2, 10, 10, 650, 300, Pieza.DefectoImagen, Pieza.Id.ToString());

                        //cargarImagenHoja(xlWorkSheetDatos, 50, 200, 790, 380, Pieza.DefectoImagen); // todo:borrar
                    }


                    
                    //Cargar los defectos
                    int intContador = 39;
                    foreach (var item in LstDefecto)
                    {
                        xlWorkSheetDatos.Cells["E" + intContador.ToString()].Value = item.Zona;
                        xlWorkSheetDatos.Cells["K" + intContador.ToString()].Value = item.DefectoMaestro.Defecto; 
                        xlWorkSheetDatos.Cells["AK" + intContador.ToString()].Value = item.Tamanio;
                        xlWorkSheetDatos.Cells["AP" + intContador.ToString()].Value = "";
                        xlWorkSheetDatos.Cells["AR" + intContador.ToString()].Value = item.Observacion;
                        ++intContador;
                    }



                    //Cargar la inspeccion de apariencia

                    foreach (var item in LstApariencia)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {
                            case "266": //  Acabado y limpieza de bordes - K65
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "K62" : "V62")].Value = "X";
                                break;
                            case "297": //   Espesor X5 (PBS)    THICKNEES K67
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K64"].Value = item.Valor;
                                break;
                            case "298": //  Espesor X5 (LD) THICKNEES   K67
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K64"].Value = item.Valor;
                                break;
                            case "299": //  Espesor X5 (LE) THICKNEES
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K64"].Value = item.Valor;
                                break;
                            case "300": //  Espesor X5 (CABINA/PARTICIÓN)   THICKNEES
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K64"].Value = item.Valor;
                                break;
                            case "257": // Logo    LOGO (INTERNAL BLACK) AL63
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL60" : "AS60")].Value = "X";
                                break;
                            case "324": // BANDA NEGRA INTERNA BLACK BAND INTERNAL
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL63" : "AS63")].Value = "X";
                                break;
                            case "325": // BANDA NEGRA EXTERNA BLACK BAND EXTERNAL 
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL64" : "AS64")].Value = "X";
                                break;
                            default:
                                break;
                        }
                    }


                    // Cargar la inspeccion dimensional

                    foreach (var item in LstDimensional)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {
                            case "409": // banda negra externa A  - BLACK BAND EXTERNAL - RECUADRO A - E19
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape1", item.Val1, 18, 4, 24, 12);
                                //cargarShapeHoja(xlWorkSheetBN, 19, 5, 7, item.Val1, 6);
                                
                                // banda negra externa B -  BLACK BAND EXTERNAL - RECUADRO B   -   I27
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape2", item.Val1, 26, 8, 24, 12);
                                //cargarShapeHoja(xlWorkSheetBN, 27, 9, 0, item.Val2, 3);
                                
                                // banda negra externa C -  BLACK BAND EXTERNAL - RECUADRO C  - K15
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape3", item.Val1, 14,10, 24, 12);
                                //cargarShapeHoja(xlWorkSheetBN, 22, 13, 8, item.Val3, 4);

                                // banda negra externa C -  BLACK BAND EXTERNAL - RECUADRO D  -  M22
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape4", item.Val1, 21, 12, 24, 12);
                                //cargarShapeHoja(xlWorkSheetBN, 15, 11, 0, item.Val4, 4);
                                break;
                            case "410": // banda negra interna A -  BLACK BAND NTERNAL - RECUADRO A  -  E37

                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape1", item.Val1, 36, 4, 24, 12);
                                //cargarShapeHoja(xlWorkSheetBN, 37, 5, 8, item.Val1, 10);

                                // banda negra interna B -  BLACK BAND INTERNAL - RECUADRO B   - I45

                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape2", item.Val1, 44, 8, 24, 12);
                                //cargarShapeHoja(xlWorkSheetBN, 45, 9, 2, item.Val2, 6);

                                // banda negra interna C -  BLACK BAND INTERNAL - RECUADRO C -  M40

                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape3", item.Val1, 39, 12, 24, 12);
                                //cargarShapeHoja(xlWorkSheetBN, 40, 13, 4, item.Val3, 8);
                                // banda negra interna D -  BLACK BAND INTERNAL - RECUADRO D  - K33

                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape4", item.Val1, 32, 10, 24, 12);
                                //cargarShapeHoja(xlWorkSheetBN, 33, 10, 28, item.Val4, 8);
                                break;
                            default:
                                break;
                        }
                    }



                    // Cargar Inspeccion optica

                    foreach (var item in LstOpticos)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {
                            // HOJA STICKER ADUANA
                            case "388": //   C13
                                xlWorkSheetSticker = _HelpExcel.AddImageToSheet(xlWorkSheetSticker, 13, 3, 15, 15, 400, 260, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //  cargarImagenHoja(xlWorkSheetSticker, 12, 3, 400, 260, strArchivo);
                                break;

                        }
                    }


                    // Guardamos el archivo de Excel con los cambios
                    FileInfo resultadoArchivo = new FileInfo(ResultadoRuta);
                    excelPackage.SaveAs(resultadoArchivo);
                }

                result = true;
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            /*
          
                */
            return result;
        }

        public bool CargarExcelFormato08(string PlantillaMacro, string ResultadoRuta, CertificadoIf Certificado, PiezaSap Pieza, List<CertificadoIfdimension> LstDimensional, List<CertificadoIfapariencias> LstApariencia, List<PiezaConcesion> LstDefecto, List<InspeccionOptica> LstOpticos)
        {
            bool result = false;
            FileInfo plantillaArchivo = new FileInfo(PlantillaMacro);
            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage(plantillaArchivo))
                {


                    string responsableApariencia = (LstApariencia.Count > 0) ? LstApariencia[0].UsuarioCrea : "";

                    ExcelWorksheet xlWorkSheetDatos = excelPackage.Workbook.Worksheets["DATOS"];
                    ExcelWorksheet xlWorkSheetBN = excelPackage.Workbook.Worksheets["BN"];
                    ExcelWorksheet xlWorkSheetSticker = excelPackage.Workbook.Worksheets["STICKER ADUANA"];
                    //xlWorkSheet = xlWorkBook.Sheets["DATOS"];


                    xlWorkSheetDatos.Cells["AE7"].Value = Pieza.LoteLogistico;
                    //xlWorkSheetDatos.Range["AR7"].Value = pieza.Color;
                    //xlWorkSheetDatos.Range["AE8"].Value = pieza.formula;
                    xlWorkSheetDatos.Cells["N79"].Value = responsableApariencia;
                    xlWorkSheetDatos.Cells["AS80"].Value = DateTime.Now.Day.ToString();
                    xlWorkSheetDatos.Cells["AT80"].Value = DateTime.Now.Month.ToString();
                    xlWorkSheetDatos.Cells["AU80"].Value = DateTime.Now.Year.ToString();

                    xlWorkSheetBN.Cells["M4"].Value = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
                    xlWorkSheetBN.Cells["F48"].Value = responsableApariencia;

                    xlWorkSheetSticker.Cells["F31"].Value = responsableApariencia;



                    //CARGAR IMAGEN DEFECTO
                    if (Pieza.DefectoImagen != "" && Pieza.DefectoImagen != null)// B11
                    {
                        _HelpExcel.AddImageToSheet(xlWorkSheetDatos, 12, 2, 10, 10, 650, 300, Pieza.DefectoImagen, Pieza.Id.ToString());
                        //cargarImagenHoja(xlWorkSheetDatos, 50, 200, 790, 380, Pieza.DefectoImagen); // todo:borrar
                    }


                    //Cargar los defectos
                    int intContador = 39;
                    foreach (var item in LstDefecto)
                    {
                        xlWorkSheetDatos.Cells["E" + intContador.ToString()].Value = item.Zona;
                        xlWorkSheetDatos.Cells["K" + intContador.ToString()].Value = item.DefectoMaestro.Defecto; 
                        xlWorkSheetDatos.Cells["AK" + intContador.ToString()].Value = item.Tamanio;
                        xlWorkSheetDatos.Cells["AP" + intContador.ToString()].Value = "";
                        xlWorkSheetDatos.Cells["AR" + intContador.ToString()].Value = item.Observacion;
                        ++intContador;
                    }


                    //Cargar la inspeccion de apariencia

                    foreach (var item in LstApariencia)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {
                            case "266": //  Acabado y limpieza de bordes - K65
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "K62" : "V62")].Value = "X";
                                break;
                            case "297": //   Espesor X5 (PBS)    THICKNEES K67
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K64"].Value = item.Valor;
                                break;
                            case "298": //  Espesor X5 (LD) THICKNEES   K67
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K64"].Value = item.Valor;
                                break;
                            case "299": //  Espesor X5 (LE) THICKNEES
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K64"].Value = item.Valor;
                                break;
                            case "300": //  Espesor X5 (CABINA/PARTICIÓN)   THICKNEES
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K64"].Value = item.Valor;
                                break;
                            case "257": // Logo    LOGO (INTERNAL BLACK) AL63
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL60" : "AS60")].Value = "X";
                                break;
                            case "324": // BANDA NEGRA INTERNA BLACK BAND INTERNAL
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL63" : "AS63")].Value = "X";
                                break;
                            case "388": // BANDA NEGRA EXTERNA BLACK BAND EXTERNAL
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL64" : "AS64")].Value = "X";
                                break;
                            case "325": // BANDA NEGRA EXTERNA BLACK BAND EXTERNAL
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL64" : "AS64")].Value = "X";
                                break;

                            default:
                                break;
                        }
                    }


                    // Cargar la inspeccion dimensional

                    foreach (var item in LstDimensional)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {
                            case "409": // banda negra externa A  - BLACK BAND EXTERNAL - RECUADRO A  -  E19
                                    xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape_A", item.Val1, 19, 4, 24, 12, -14, 0);
                                    //cargarShapeHoja(xlWorkSheetBN, 19, 5, 7, item.Val1, 6);
                                // banda negra externa B -  BLACK BAND EXTERNAL - RECUADRO B  - I27
                                    xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape_B", item.Val1, 27, 8, 24, 12, 0, 0);
                                    //cargarShapeHoja(xlWorkSheetBN, 27, 9, 0, item.Val2, 0);
                                // banda negra externa C -  BLACK BAND EXTERNAL - RECUADRO C  - M22
                                    xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape_C", item.Val1, 21, 12, 24, 12, 0, 0);
                                    //cargarShapeHoja(xlWorkSheetBN, 22, 13, 8, item.Val3, 4);
                                // banda negra externa C -  BLACK BAND EXTERNAL - RECUADRO D  - K16
                                    xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape_D", item.Val1, 15, 10, 24, 12, 0, 0);
                                    //cargarShapeHoja(xlWorkSheetBN, 15, 11, 0, item.Val4, 4);
                                break;
                            case "410": // banda negra interna A -  BLACK BAND NTERNAL - RECUADRO A  - E38
                                    xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape_A", item.Val1, 37, 4, 24, 12, 0, 0);
                                    //cargarShapeHoja(xlWorkSheetBN, 37, 5, 8, item.Val1, 10);
                                // banda negra interna B -  BLACK BAND INTERNAL - RECUADRO B   - I45
                                    xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape_B", item.Val1, 44, 8, 24, 12, 0, 0);
                                    //cargarShapeHoja(xlWorkSheetBN, 45, 9, 2, item.Val2, 3);
                                // banda negra interna C -  BLACK BAND INTERNAL - RECUADRO C    -  M40
                                    xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape_C", item.Val1, 39, 12, 24, 12, 0, 0);
                                    //cargarShapeHoja(xlWorkSheetBN, 40, 13, 4, item.Val3, 7);
                                // banda negra interna D -  BLACK BAND INTERNAL - RECUADRO D    - K34
                                    xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape_D", item.Val1, 33, 10, 24, 12, 0, 0);
                                    //cargarShapeHoja(xlWorkSheetBN, 33, 10, 28, item.Val4, 8);
                                break;
                            default:
                                break;
                        }
                    }



                    // Cargar Inspeccion optica

                    foreach (var item in LstOpticos)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {
                            // HOJA STICKER ADUANA
                            case "388": //  C13
                                xlWorkSheetSticker = _HelpExcel.AddImageToSheet(xlWorkSheetSticker, 13, 3, 15, 15, 400, 260, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetSticker, 130, 210, 400, 260, strArchivo);
                                break;

                        }
                    }


                    // Guardamos el archivo de Excel con los cambios
                    FileInfo resultadoArchivo = new FileInfo(ResultadoRuta);
                    excelPackage.SaveAs(resultadoArchivo);
                }

                result = true;
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            /*
          
                */
            return result;
        }


        public bool CargarExcelFormato11(string PlantillaMacro, string ResultadoRuta, CertificadoIf Certificado, PiezaSap Pieza, List<CertificadoIfdimension> LstDimensional, List<CertificadoIfapariencias> LstApariencia, List<PiezaConcesion> LstDefecto, List<InspeccionOptica> LstOpticos)
        {
            bool result = false;
            FileInfo plantillaArchivo = new FileInfo(PlantillaMacro);
            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage(plantillaArchivo))
                {
                    string responsableApariencia = (LstApariencia.Count > 0) ? LstApariencia[0].UsuarioCrea : "";

                    // Obtenemos la hoja de cálculo del archivo Excel
                    ExcelWorksheet xlWorkSheetDatos = excelPackage.Workbook.Worksheets["DATOS"];
                    ExcelWorksheet xlWorkSheetReflexion = excelPackage.Workbook.Worksheets["REFLEXION"];
                    ExcelWorksheet xlWorkSheetAccesories = excelPackage.Workbook.Worksheets["ACCESORIOS"];
                    ExcelWorksheet xlWorkSheetBN = excelPackage.Workbook.Worksheets["BN"];
                    ExcelWorksheet xlWorkSheetSerigrafia = excelPackage.Workbook.Worksheets["SERIGRAFIA"];
                    ExcelWorksheet xlWorkSheetPC = excelPackage.Workbook.Worksheets["PC"];
                    ExcelWorksheet xlWorkSheetSticker = excelPackage.Workbook.Worksheets["STICKER ADUANA"];

                    // Obtenemos la hoja de cálculo del archivo Excel
                    xlWorkSheetDatos.Cells["AE7"].Value = Pieza.LoteLogistico;
                    xlWorkSheetDatos.Cells["AR7"].Value = Pieza.Color;
                    xlWorkSheetDatos.Cells["AE8"].Value = Pieza.Formula;
                    xlWorkSheetDatos.Cells["N83"].Value = responsableApariencia;
                    xlWorkSheetDatos.Cells["AS84"].Value = DateTime.Now.Day.ToString();
                    xlWorkSheetDatos.Cells["AT84"].Value = DateTime.Now.Month.ToString();
                    xlWorkSheetDatos.Cells["AU84"].Value = DateTime.Now.Year.ToString();

                    xlWorkSheetReflexion.Cells["H6"].Value = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
                    xlWorkSheetReflexion.Cells["E62"].Value = responsableApariencia;
                    xlWorkSheetAccesories.Cells["F48"].Value = responsableApariencia; 
                    xlWorkSheetSerigrafia.Cells["F48"].Value = responsableApariencia;
                    xlWorkSheetBN.Cells["F48"].Value = responsableApariencia;
                    xlWorkSheetSticker.Cells["F31"].Value = responsableApariencia;
                    xlWorkSheetPC.Cells["F48"].Value = responsableApariencia;




                    //CARGAR IMAGEN DEFECTO
                    if (Pieza.DefectoImagen != "" && Pieza.DefectoImagen != null) // B11
                    {
                        _HelpExcel.AddImageToSheet(xlWorkSheetDatos, 12, 2, 10, 10, 790, 380, Pieza.DefectoImagen, Pieza.Id.ToString());
                        //cargarImagenHoja(xlWorkSheetDatos, 50, 200, 790, 380, Pieza.DefectoImagen); // todo:borrar
                    }


                    //Cargar los defectos
                    int intContador = 49;
                    foreach (var item in LstDefecto)
                    {
                        xlWorkSheetDatos.Cells["E" + intContador.ToString()].Value = item.Zona;
                        xlWorkSheetDatos.Cells["K" + intContador.ToString()].Value = item.DefectoMaestro.Defecto; 
                        xlWorkSheetDatos.Cells["AK" + intContador.ToString()].Value = item.Tamanio;
                        xlWorkSheetDatos.Cells["AP" + intContador.ToString()].Value = "";
                        xlWorkSheetDatos.Cells["AR" + intContador.ToString()].Value = item.Observacion;
                        ++intContador;
                    }


                    //Cargar la inspeccion de apariencia
                    foreach (var item in LstApariencia)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {
                            case "266": //  Acabado y limpieza de bordes - K65
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "K65" : "V65")].Value = "X";
                                break;
                            case "297": //   Espesor X5 (PBS)    THICKNEES K67
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K67"].Value = item.Valor;
                                break;
                            case "298": //  Espesor X5 (LD) THICKNEES   K67
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K67"].Value = item.Valor;
                                break;
                            case "299": //  Espesor X5 (LE) THICKNEES
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K67"].Value = item.Valor;
                                break;
                            case "300": //  Espesor X5 (CABINA/PARTICIÓN)   THICKNEES
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K67"].Value = item.Valor;
                                break;
                            case "259": //  Apariencia serigrafia   SERIGRAPHY K68
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "K68" : "V68")].Value = "X";
                                break;
                            case "257": // Logo    LOGO (INTERNAL BLACK) AL63
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL63" : "AS63")].Value = "X";
                                break;
                            case "263": // Chaflan CHAMFER
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL64" : "AS64")].Value = "X";
                                break;
                            case "367": // Reflexion   REFLECTION
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL65" : "AS65")].Value = "X";
                                break;
                            case "324": // BANDA NEGRA INTERNA BLACK BAND INTERNAL
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL66" : "AS66")].Value = "X";
                                break;
                            case "325": // BANDA NEGRA EXTERNA BLACK BAND EXTERNAL
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL67" : "AS67")].Value = "X";
                                break;
                            case "262": // BRILLIANT ROUNDED / Tipo de borde vidrio pintura
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL64" : "AS64")].Value = "X";

                                break;
                            case "419": // DIVIDING BLACK MASKING VERTICAL FRINGES ALIGNED / Alineación bandas negras
                                xlWorkSheetDatos.Cells[(item.Valor == "OK" ? "AL68" : "AS68")].Value = "X";

                                break;


                            default:
                                break;
                        }
                    }


                    // Cargar la inspeccion dimensional

                    foreach (var item in LstDimensional)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {
                            case "397": // banda negra externa A  LESS - BLACK BAND EXTERNAL - RECUADRO A  - E23 
                                    xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 22, 4, 24, 12, 0, -10);
                                    // cargarShapeHoja(xlWorkSheetBN, 23, 4, 10, item.Val1);
                                break;
                            case "398": // banda negra externa B LESS -  BLACK BAND EXTERNAL - RECUADRO C  - M24 
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 23, 12, 24, 12,-5,45);
                                //cargarShapeHoja(xlWorkSheetBN, 17, 10, -2, item.Val1, -2);
                                break;
                            case "399": // banda negra externa D LESS -  BLACK BAND EXTERNAL - RECUADRO B - J17
                                   // cargarShapeHoja(xlWorkSheetBN, 24, 13, 35, item.Val1, -4);
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 16, 9, 24, 12, -5, 0);
                                break;

                            case "400": // banda negra interna A LESS-  BLACK BAND NTERNAL - RECUADRO A - E42
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 41, 4, 24, 12, -10, -10);
                                //cargarShapeHoja(xlWorkSheetBN, 41, 4, 16, item.Val1, 5);
                                break;
                            case "401": // banda negra interna B LESS-  BLACK BAND INTERNAL - RECUADRO C - N41
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 40, 13, 24, 12, 3, -40);
                                //cargarShapeHoja(xlWorkSheetBN, 36, 11, -10, item.Val1);
                                break;
                            case "402": // banda negra interna C  LESS-  BLACK BAND INTERNAL - RECUADRO B K36
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 35, 10, 24, 12, 3, -25);
                                //cargarShapeHoja(xlWorkSheetBN, 41, 13, 35, item.Val1, 4);
                                break;
                            default:
                                break;
                        }
                    }


                    // Cargar Inspeccion optica

                    string strArchivo = "";
                    foreach (var item in LstOpticos)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {

                            // HOJA REFLEXION
                            case "273": // Reflexion 1 -  inspeccion optico   - D27
                                xlWorkSheetReflexion = _HelpExcel.AddImageToSheet(xlWorkSheetReflexion, 26, 3, 15, 15, 290, 150, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetReflexion, 180, 320, 290, 150, strArchivo);
                                break;
                            case "336": // Reflexion 2 -  inspeccion optico  - D43
                                xlWorkSheetReflexion = _HelpExcel.AddImageToSheet(xlWorkSheetReflexion, 42, 3, 15, 15,290, 150, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetReflexion, 180, 530, 290, 150, strArchivo);
                                break;

                            // HOJA ACCESORIES
                            case "342": // Acero 1 - STEELS - E13
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 12, 4, 15, 15, 250, 190, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 210, 210, 250, 190, strArchivo);
                                break;
                            case "343": // Acero 2 - STEELS - E32
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 31, 4, 15, 15, 250, 200, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetAccesories, 210, 450, 250, 200, strArchivo);
                                break;

                            // HOJA SERIGRAFIA
                            case "353": // Serigrafia 1  -  B13
                                xlWorkSheetSerigrafia = _HelpExcel.AddImageToSheet(xlWorkSheetSerigrafia, 12, 1, 15, 15, 250, 195, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetSerigrafia, 30, 210, 250, 195, strArchivo);
                                break;
                            case "354": // Serigrafia 2  - K13
                                xlWorkSheetSerigrafia = _HelpExcel.AddImageToSheet(xlWorkSheetSerigrafia, 12, 10, 15, 15, 250, 195, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetSerigrafia, 375, 210, 250, 195, strArchivo);
                                break;
                            case "355": // Serigrafia 3 - B31
                                xlWorkSheetSerigrafia = _HelpExcel.AddImageToSheet(xlWorkSheetSerigrafia, 30, 1, 15, 15, 250, 205, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                ///cargarImagenHoja(xlWorkSheetSerigrafia, 30, 455, 250, 205, strArchivo);
                                break;
                            case "356": // Serigrafia 4 - K31
                                xlWorkSheetSerigrafia = _HelpExcel.AddImageToSheet(xlWorkSheetSerigrafia, 30, 10, 15, 15, 250, 200, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetSerigrafia, 375, 455, 250, 205, strArchivo);
                                break;

                            // HOJA PC
                            case "363": // PC 1  -  Image 1  - C13
                                xlWorkSheetPC = _HelpExcel.AddImageToSheet(xlWorkSheetPC, 12, 2, 15, 15, 350, 190, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetPC, 130, 212, 350, 190, strArchivo);
                                break;
                            case "364": // PC 2  -  Image 2 - C31
                                xlWorkSheetPC = _HelpExcel.AddImageToSheet(xlWorkSheetPC, 30, 2, 15, 15, 350, 205, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetPC, 130, 455, 350, 205, strArchivo);
                                break;

                            // HOJA STICKER
                            case "388": //  C13
                                xlWorkSheetSticker = _HelpExcel.AddImageToSheet(xlWorkSheetSticker, 12, 2, 15, 15, 350, 185, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                //cargarImagenHoja(xlWorkSheetSticker, 125, 210, 350, 185, strArchivo);
                                break;
                            default:
                                break;
                        }
                    }

                    // Guardamos el archivo de Excel con los cambios
                    FileInfo resultadoArchivo = new FileInfo(ResultadoRuta);
                    excelPackage.SaveAs(resultadoArchivo);
                }

                result = true;
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            /*
          
                */
            return result;
        }

        public bool CargarExcelFormato12(string PlantillaMacro, string ResultadoRuta, CertificadoIf Certificado, PiezaSap Pieza, List<CertificadoIfdimension> LstDimensional, List<CertificadoIfapariencias> LstApariencia, List<PiezaConcesion> LstDefecto, List<InspeccionOptica> LstOpticos)
        {
            bool result = false;
            FileInfo plantillaArchivo = new FileInfo(PlantillaMacro);
            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage(plantillaArchivo))
                {


                    string responsableApariencia = (LstApariencia.Count > 0) ? LstApariencia[0].UsuarioCrea : "";

                    // Obtenemos la hoja de cálculo del archivo Excel

                    
                    ExcelWorksheet xlWorkSheetDatos = excelPackage.Workbook.Worksheets["DATOS"];
                    ExcelWorksheet xlWorkSheetReflexion = excelPackage.Workbook.Worksheets["REFLEXION"];
                    ExcelWorksheet xlWorkSheetAccesories = excelPackage.Workbook.Worksheets["ACCESORIOS"];
                    ExcelWorksheet xlWorkSheetBN = excelPackage.Workbook.Worksheets["BN"];
                    ExcelWorksheet xlWorkSheetSerigrafia = excelPackage.Workbook.Worksheets["SERIGRAFIA"];
                    ExcelWorksheet xlWorkSheetPC = excelPackage.Workbook.Worksheets["PC"];
                    ExcelWorksheet xlWorkSheetSticker = excelPackage.Workbook.Worksheets["STICKER ADUANA"];


                    //xlWorkSheet = xlWorkBook.Sheets["DATOS"];


                    xlWorkSheetDatos.Cells["AE7"].Value = Pieza.LoteLogistico;
                    xlWorkSheetDatos.Cells["AR7"].Value = Pieza.Color;
                    xlWorkSheetDatos.Cells["AE8"].Value = Pieza.Formula;
                    xlWorkSheetDatos.Cells["N83"].Value = responsableApariencia;
                    xlWorkSheetDatos.Cells["AS84"].Value = DateTime.Now.Day.ToString();
                    xlWorkSheetDatos.Cells["AT84"].Value = DateTime.Now.Month.ToString();
                    xlWorkSheetDatos.Cells["AU84"].Value = DateTime.Now.Year.ToString();

                    xlWorkSheetReflexion.Cells["H6"].Value = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
                    xlWorkSheetReflexion.Cells["E62"].Value = responsableApariencia;
                    xlWorkSheetAccesories.Cells["F48"].Value = responsableApariencia;
                    xlWorkSheetSerigrafia.Cells["F48"].Value = responsableApariencia;
                    xlWorkSheetBN.Cells["F48"].Value = responsableApariencia;
                    xlWorkSheetSticker.Cells["F31"].Value = responsableApariencia;
                    xlWorkSheetPC.Cells["F48"].Value = responsableApariencia;



                    //CARGAR IMAGEN DEFECTO
                    if (Pieza.DefectoImagen != "" && Pieza.DefectoImagen != null)
                    {
                        _HelpExcel.AddImageToSheet(xlWorkSheetDatos, 12, 2, 10, 10, 790, 380, Pieza.DefectoImagen, Pieza.Id.ToString());
                        //cargarImagenHoja(xlWorkSheetDatos, 50, 200, 790, 380, Pieza.DefectoImagen); // todo:borrar
                    }


                    //Cargar los defectos
                    int intContador = 39;
                    foreach (var item in LstDefecto)
                    {
                        xlWorkSheetDatos.Cells["E" + intContador.ToString()].Value = item.Zona;
                        xlWorkSheetDatos.Cells["K" + intContador.ToString()].Value = item.DefectoMaestro.Defecto; 
                        xlWorkSheetDatos.Cells["AK" + intContador.ToString()].Value = item.Tamanio;
                        xlWorkSheetDatos.Cells["AP" + intContador.ToString()].Value = "";
                        xlWorkSheetDatos.Cells["AR" + intContador.ToString()].Value = item.Observacion;
                        ++intContador;
                    }


                    foreach (var item in LstApariencia)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {
                            case "266": //  Acabado y limpieza de bordes - K65
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "K65" : "V65")].Value = "X";
                                break;
                            case "297": //   Espesor X5 (PBS)    THICKNEES K67
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K67"].Value = item.Valor;
                                break;
                            case "298": //  Espesor X5 (LD) THICKNEES   K67
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K67"].Value = item.Valor;
                                break;
                            case "299": //  Espesor X5 (LE) THICKNEES
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K67"].Value = item.Valor;
                                break;
                            case "300": //  Espesor X5 (CABINA/PARTICIÓN)   THICKNEES
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K67"].Value = item.Valor;
                                break;
                            case "259": //  Apariencia serigrafia   SERIGRAPHY K68
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "K68" : "V68")].Value = "X";
                                break;
                            case "257": // Logo    LOGO (INTERNAL BLACK) AL63
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL63" : "AS63")].Value = "X";
                                break;
                            case "263": // Chaflan CHAMFER
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL64" : "AS64")].Value = "X";
                                break;
                            case "367": // Reflexion   REFLECTION
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL65" : "AS65")].Value = "X";
                                break;
                            case "324": // BANDA NEGRA INTERNA BLACK BAND INTERNAL
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL66" : "AS66")].Value = "X";
                                break;
                            case "325": // BANDA NEGRA EXTERNA BLACK BAND EXTERNAL
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL67" : "AS67")].Value = "X";
                                break;
                            case "262": // BRILLIANT ROUNDED / Tipo de borde vidrio pintura
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL64" : "AS64")].Value = "X";

                                break;
                            case "419": // DIVIDING BLACK MASKING VERTICAL FRINGES ALIGNED / Alineación bandas negras
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL68" : "AS68")].Value = "X";

                                break;
                            default:
                                break;
                        }
                    }




                    // Cargar la inspeccion dimensional

                    foreach (var item in LstDimensional)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {
                            case "397": // banda negra externa A  - BLACK BAND EXTERNAL - RECUADRO A  - E23
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 22, 3, 24, 12,0,25);
                                // cargarShapeHoja(xlWorkSheetBN, 23, 4, 16, item.Val1, 4);
                                break;
                            case "398": // banda negra externa B -  BLACK BAND EXTERNAL - RECUADRO B - J17
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 16, 9, 24, 12);
                                //cargarShapeHoja(xlWorkSheetBN, 17, 12, 13, item.Val1, 5);
                                break;
                            case "399": // banda negra externa C -  BLACK BAND EXTERNAL - RECUADRO C  - M24
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 23, 13, 24, 12,-5,-35);
                                //cargarShapeHoja(xlWorkSheetBN, 24, 13, 35, item.Val1, 6);
                                break;

                            case "400": // banda negra interna A -  BLACK BAND NTERNAL - RECUADRO A  -   E42
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 41, 4, 24, 12,-10,-20);
                                //cargarShapeHoja(xlWorkSheetBN, 41, 4, 20, item.Val1, 1);
                                break;
                            case "401": // banda negra interna B -  BLACK BAND INTERNAL - RECUADRO B -  K36
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 35, 10, 24, 12,0,-15);
                                //cargarShapeHoja(xlWorkSheetBN, 35, 12, 16, item.Val1, 14);
                                break;
                            case "402": // banda negra interna C -  BLACK BAND INTERNAL - RECUADRO C  - N42
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape", item.Val1, 41, 12, 24, 12,-10,50);
                                //cargarShapeHoja(xlWorkSheetBN, 41, 13, 32, item.Val1, 12);
                                break;
                            //case "352": // banda negra interna D -  BLACK BAND INTERNAL - RECUADRO B
                            //    cargarShapeHoja(xlWorkSheetBN, 36, 12, 0, item.Val1);
                            //   break;
                            default:
                                break;
                        }
                    }

                    /*
                    foreach (var item in LstDimensional)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {
                            case "411": // banda negra externa A  - BLACK BAND EXTERNAL - RECUADRO A  -  E23   -10
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape_A", item.Val1, 22, 4, 24, 12, -10, 0);
                                //cargarShapeHoja(xlWorkSheetBN, 23, 2, 57, item.Val1, -2);
                                // banda negra externa B -  BLACK BAND EXTERNAL - RECUADRO B  -  J17
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape_B", item.Val2, 16, 9, 24, 12, 0, 0);
                                //cargarShapeHoja(xlWorkSheetBN, 23, 15, 8, item.Val2, -2);
                                // banda negra externa C -  BLACK BAND EXTERNAL - RECUADRO C  - N24  -20
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape_C", item.Val3, 23, 13, 24, 12, -20, 0);
                                //cargarShapeHoja(xlWorkSheetBN, 24, 9, -5, item.Val3, 4);
                                break;
                            case "412": // banda negra interna A -  BLACK BAND NTERNAL - RECUADRO A  -  E42
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape_A", item.Val1, 41, 4, 24, 12, 0, 0);
                                //cargarShapeHoja(xlWorkSheetBN, 41, 2, 57, item.Val1, 0);
                                // banda negra interna B -  BLACK BAND INTERNAL - RECUADRO B  -  K36
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape_B", item.Val2, 35, 10, 24, 12, 0, 0);
                                //cargarShapeHoja(xlWorkSheetBN, 41, 15, 10, item.Val2, 1);
                                // banda negra interna C -  BLACK BAND INTERNAL - RECUADRO C  -  N41  -20
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape_C", item.Val3, 40, 13, 24, 12, -20, 0);
                                //cargarShapeHoja(xlWorkSheetBN, 42, 9, -5, item.Val3, 4);
                                break;


                            default:
                                break;
                        }
                    }*/


                    // Cargar Inspeccion optica

                    foreach (var item in LstOpticos)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {

                            // HOJA REFLEXION
                            case "273": // Reflexion 1 -  inspeccion optico  - D27
                                xlWorkSheetReflexion = _HelpExcel.AddImageToSheet(xlWorkSheetReflexion, 26, 3, 15, 15, 285, 150, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                break;
                            case "336": // Reflexion 2 -  inspeccion optico  -  D43
                                xlWorkSheetReflexion = _HelpExcel.AddImageToSheet(xlWorkSheetReflexion, 42, 3, 15, 15, 285, 150, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                break;

                            // HOJA ACCESORIES
                            case "342": // Acero 1 - STEELS - D32
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 12, 4, 15, 15, 250, 190, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                
                                break;
                            case "343": // Acero 2 - STEELS - I32
                                xlWorkSheetAccesories = _HelpExcel.AddImageToSheet(xlWorkSheetAccesories, 31,4, 15, 15, 250, 190, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                break;

                            // HOJA SERIGRAFIA
                            case "353": // Serigrafia 1  -  B13
                                xlWorkSheetSerigrafia = _HelpExcel.AddImageToSheet(xlWorkSheetSerigrafia, 12, 1, 15, 15, 255, 195, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                break;
                            case "354": // Serigrafia 2  K13
                                xlWorkSheetSticker = _HelpExcel.AddImageToSheet(xlWorkSheetSticker, 12,10, 15, 15, 255, 195, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                
                                break;
                            case "355": // Serigrafia 3  B31
                                xlWorkSheetSerigrafia = _HelpExcel.AddImageToSheet(xlWorkSheetSerigrafia, 30, 1, 15, 15, 255, 205, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                break;
                            case "356": // Serigrafia 4  K31
                                xlWorkSheetSerigrafia = _HelpExcel.AddImageToSheet(xlWorkSheetSerigrafia, 30, 10, 15, 15, 255, 205, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                break;

                            // HOJA PC
                            case "363": // PC 1  -  Image 1  - C13
                                xlWorkSheetPC = _HelpExcel.AddImageToSheet(xlWorkSheetPC, 12,2, 15, 15, 350, 190, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                
                                break;
                            case "364": // PC 2  -  Image 2 - C31
                                xlWorkSheetPC = _HelpExcel.AddImageToSheet(xlWorkSheetPC, 30, 2, 15, 15, 350, 205, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                break;

                            // HOJA STICKER
                            case "388": //  C13
                                xlWorkSheetSticker = _HelpExcel.AddImageToSheet(xlWorkSheetSticker, 12, 2, 15, 15, 355, 185, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                break;
                            default:
                                break;
                        }
                    }


                    // Guardamos el archivo de Excel con los cambios
                    FileInfo resultadoArchivo = new FileInfo(ResultadoRuta);
                    excelPackage.SaveAs(resultadoArchivo);
                }

                result = true;
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            /*
          
                */
            return result;
        }

        public bool CargarExcelFormato30(string PlantillaMacro, string ResultadoRuta, CertificadoIf Certificado, PiezaSap Pieza, List<CertificadoIfdimension> LstDimensional, List<CertificadoIfapariencias> LstApariencia, List<PiezaConcesion> LstDefecto, List<InspeccionOptica> LstOpticos)
        {
            bool result = false;
            FileInfo plantillaArchivo = new FileInfo(PlantillaMacro);
            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage(plantillaArchivo))
                {


                    string responsableApariencia = (LstApariencia.Count > 0) ? LstApariencia[0].UsuarioCrea : "";

                    // Obtenemos la hoja de cálculo del archivo Excel

                    ExcelWorksheet xlWorkSheetDatos = excelPackage.Workbook.Worksheets["DATOS"];
                    ExcelWorksheet xlWorkSheetBN = excelPackage.Workbook.Worksheets["BN"];
                    ExcelWorksheet xlWorkSheetSticker = excelPackage.Workbook.Worksheets["STICKER ADUANA"];
                    //xlWorkSheet = xlWorkBook.Sheets["DATOS"];


                    xlWorkSheetDatos.Cells["AE7"].Value = Pieza.LoteLogistico;
                    xlWorkSheetDatos.Cells["N79"].Value = responsableApariencia;
                    xlWorkSheetDatos.Cells["AS80"].Value = DateTime.Now.Day.ToString();
                    xlWorkSheetDatos.Cells["AT80"].Value = DateTime.Now.Month.ToString();
                    xlWorkSheetDatos.Cells["AU80"].Value = DateTime.Now.Year.ToString();

                    xlWorkSheetBN.Cells["M4"].Value = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
                    xlWorkSheetBN.Cells["F48"].Value = responsableApariencia;

                    xlWorkSheetSticker.Cells["F31"].Value = responsableApariencia;


                    //CARGAR IMAGEN DEFECTO
                    if (Pieza.DefectoImagen != "" && Pieza.DefectoImagen != null) // B11
                    {
                        _HelpExcel.AddImageToSheet(xlWorkSheetDatos, 12, 2, 10, 10, 650, 300, Pieza.DefectoImagen, Pieza.Id.ToString());
                        //cargarImagenHoja(xlWorkSheetDatos, 50, 200, 790, 380, Pieza.DefectoImagen); // todo:borrar
                    }


                    //Cargar los defectos
                    int intContador = 39;
                    foreach (var item in LstDefecto)
                    {
                        xlWorkSheetDatos.Cells["E" + intContador.ToString()].Value = item.Zona;
                        xlWorkSheetDatos.Cells["K" + intContador.ToString()].Value = item.DefectoMaestro.Defecto; 
                        xlWorkSheetDatos.Cells["AK" + intContador.ToString()].Value = item.Tamanio;
                        xlWorkSheetDatos.Cells["AP" + intContador.ToString()].Value = "";
                        xlWorkSheetDatos.Cells["AR" + intContador.ToString()].Value = item.Observacion;
                        ++intContador;
                    }


                    //Cargar la inspeccion de apariencia

                    foreach (var item in LstApariencia)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {
                            case "266": //  Acabado y limpieza de bordes - K65
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "K62" : "V62")].Value = "X";
                                break;
                            case "297": //   Espesor X5 (PBS)    THICKNEES K67
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K64"].Value = item.Valor;
                                break;
                            case "298": //  Espesor X5 (LD) THICKNEES   K67
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K64"].Value = item.Valor;
                                break;
                            case "299": //  Espesor X5 (LE) THICKNEES
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K64"].Value = item.Valor;
                                break;
                            case "300": //  Espesor X5 (CABINA/PARTICIÓN)   THICKNEES
                                if (item.Valor != "")
                                    xlWorkSheetDatos.Cells["K64"].Value = item.Valor;
                                break;
                            case "257": // Logo    LOGO (INTERNAL BLACK) AL63
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL60" : "AS60")].Value = "X";
                                break;
                            case "324": // BANDA NEGRA INTERNA BLACK BAND INTERNAL
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL63" : "AS63")].Value = "X";
                                break;
                            case "388": // BANDA NEGRA EXTERNA BLACK BAND EXTERNAL
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL64" : "AS64")].Value = "X";
                                break;
                            case "325": // BANDA NEGRA EXTERNA BLACK BAND EXTERNAL
                                xlWorkSheetDatos.Cells[((item.Valor.ToUpper() == "OK" || item.Valor.ToUpper() == "CUMPLE") ? "AL64" : "AS64")].Value = "X";
                                break;

                            default:
                                break;
                        }
                    }


                    // Cargar la inspeccion dimensional

                    foreach (var item in LstDimensional)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {
                            case "411": // banda negra externa A  - BLACK BAND EXTERNAL - RECUADRO A   - B32  60
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape_A", item.Val1, 31, 1, 24, 12, 60, 0);
                                //cargarShapeHoja(xlWorkSheetBN, 23, 2, 57, item.Val1, -2);
                                // banda negra externa B -  BLACK BAND EXTERNAL - RECUADRO B   -  O23  25
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape_B", item.Val2, 22, 14, 24, 12, 25, 0);
                                //cargarShapeHoja(xlWorkSheetBN, 23, 15, 8, item.Val2, -2);
                                // banda negra externa C -  BLACK BAND EXTERNAL - RECUADRO C   -  I24    8
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape_C", item.Val3, 23, 8, 24, 12, 8, 0);
                                //cargarShapeHoja(xlWorkSheetBN, 24, 9, -5, item.Val3, 4);
                                // banda negra externa C -  BLACK BAND EXTERNAL - RECUADRO D  -  I17  
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape_D", item.Val4, 16, 8, 24, 12, 0, 0);
                                //cargarShapeHoja(xlWorkSheetBN, 16, 9, -8, item.Val4, 8);
                                break;
                            case "412": // banda negra interna A -  BLACK BAND NTERNAL - RECUADRO A   B41  -  60
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape_A", item.Val1, 40, 1, 24, 12, 60, 0);
                                //cargarShapeHoja(xlWorkSheetBN, 41, 2, 57, item.Val1, 0);
                                // banda negra interna B -  BLACK BAND INTERNAL - RECUADRO B    - 041  20 
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape_B", item.Val2, 40, 14, 24, 12, 20, 0);
                                //cargarShapeHoja(xlWorkSheetBN, 41, 15, 10, item.Val2, 1);
                                // banda negra interna C -  BLACK BAND INTERNAL - RECUADRO C    - I42  8
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape_C", item.Val3, 41, 8, 24, 12, 8, 0);
                                //cargarShapeHoja(xlWorkSheetBN, 42, 9, -5, item.Val3, 4);
                                // banda negra interna D -  BLACK BAND INTERNAL - RECUADRO D    -  I35  
                                xlWorkSheetBN = _HelpExcel.AddShapeToExcel(xlWorkSheetBN, item.ParametroInspeccionId.ToString() + "_shape_D", item.Val4, 34, 8, 24, 12, 0, 0);
                                //cargarShapeHoja(xlWorkSheetBN, 35, 9, -8, item.Val4, 5);
                                break;
                            default:
                                break;
                        }
                    }

                    // Cargar Inspeccion optica

                    foreach (var item in LstOpticos)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {
                            // HOJA STICKER ADUANA  B13
                            case "388": // 
                                //cargarImagenHoja(xlWorkSheetSticker, 130, 210, 400, 190, strArchivo);
                                xlWorkSheetSticker = _HelpExcel.AddImageToSheet(xlWorkSheetSticker, 13, 2, 15, 15, 400, 190, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                break;

                        }
                    }


                    // Guardamos el archivo de Excel con los cambios
                    FileInfo resultadoArchivo = new FileInfo(ResultadoRuta);
                    excelPackage.SaveAs(resultadoArchivo);
                }

                result = true;
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            /*
          
                */
            return result;
        }


        #endregion

        public bool CargarExcelFormatoJSS(string PlantillaMacro, string ResultadoRuta, CertificadoIf Certificado, PiezaSap Pieza, List<CertificadoIfdimension> LstDimensional, List<CertificadoIfapariencias> LstApariencia, List<PiezaConcesion> LstDefecto, List<InspeccionOptica> LstOpticos)
        {
            bool result = false;
            FileInfo plantillaArchivo = new FileInfo(PlantillaMacro);
            try
            {
                using (ExcelPackage excelPackage = new ExcelPackage(plantillaArchivo))
                {


                    string responsableApariencia = (LstApariencia.Count > 0) ? LstApariencia[0].UsuarioCrea : "";

                    // Obtenemos la hoja de cálculo del archivo Excel

                    ExcelWorksheet xlWorkSheetPage1 = excelPackage.Workbook.Worksheets["Pag 1"];
                    ExcelWorksheet xlWorkSheetPage2 = excelPackage.Workbook.Worksheets["Pag 2"];
                    ExcelWorksheet xlWorkSheetPage3 = excelPackage.Workbook.Worksheets["Pag 3"];
                    ExcelWorksheet xlWorkSheetPage4 = excelPackage.Workbook.Worksheets["Pag 4"];
                    //Excel.Worksheet xlWorkSheetSticker = xlWorkBook.Sheets["STICKER ADUANA"];
                    //xlWorkSheet = xlWorkBook.Sheets["DATOS"];

                    /*
                      CARGAR INFORMACIÓN DE CABECERA
                     */
                    string Dia = DateTime.Now.Day.ToString();
                    string Mes = DateTime.Now.Month.ToString();
                    string Anio = DateTime.Now.Year.ToString();

                    xlWorkSheetPage1.Cells["G3"].Value = Pieza.Id.ToString();
                    xlWorkSheetPage1.Cells["B5"].Value = "CUSTOMER : " + Pieza.Cliente;
                    xlWorkSheetPage1.Cells["B6"].Value = "VEHICLE : " + Pieza.Vehiculo;
                    xlWorkSheetPage1.Cells["B7"].Value = "COLOR : " + Pieza.Color;
                    xlWorkSheetPage1.Cells["F5"].Value = "PROJECT NUM : " + Pieza.LoteLogistico;
                    xlWorkSheetPage1.Cells["F8"].Value = "THICKNESS : " + Pieza.Espesor;
                    xlWorkSheetPage1.Cells["F9"].Value = "FORMULA : " + Pieza.Formula;

                    xlWorkSheetPage2.Cells["G3"].Value = Pieza.Id.ToString();
                    xlWorkSheetPage2.Cells["B5"].Value = "CUSTOMER : " + Pieza.Cliente;
                    xlWorkSheetPage2.Cells["B6"].Value = "VEHICLE : " + Pieza.Vehiculo;
                    xlWorkSheetPage2.Cells["B7"].Value = "COLOR : " + Pieza.Color;
                    xlWorkSheetPage2.Cells["F5"].Value = "PROJECT NUM : " + Pieza.LoteLogistico;
                    xlWorkSheetPage2.Cells["F8"].Value = "THICKNESS : " + Pieza.Espesor;
                    xlWorkSheetPage2.Cells["F9"].Value = "FORMULA : " + Pieza.Formula;

                    xlWorkSheetPage3.Cells["G3"].Value = Pieza.Id.ToString();
                    xlWorkSheetPage3.Cells["B5"].Value = "CUSTOMER : " + Pieza.Cliente;
                    xlWorkSheetPage3.Cells["B6"].Value = "VEHICLE : " + Pieza.Vehiculo;
                    xlWorkSheetPage3.Cells["B7"].Value = "COLOR : " + Pieza.Color;
                    xlWorkSheetPage3.Cells["F5"].Value = "PROJECT NUM : " + Pieza.LoteLogistico;
                    xlWorkSheetPage3.Cells["F8"].Value = "THICKNESS : " + Pieza.Espesor;
                    xlWorkSheetPage3.Cells["F9"].Value = "FORMULA : " + Pieza.Formula;

                    xlWorkSheetPage4.Cells["G3"].Value = Pieza.Id.ToString();
                    xlWorkSheetPage4.Cells["B5"].Value = "CUSTOMER : " + Pieza.Cliente;
                    xlWorkSheetPage4.Cells["B6"].Value = "VEHICLE : " + Pieza.Vehiculo;
                    xlWorkSheetPage4.Cells["B7"].Value = "COLOR : " + Pieza.Color;
                    xlWorkSheetPage4.Cells["F5"].Value = "PROJECT NUM : " + Pieza.LoteLogistico;
                    xlWorkSheetPage4.Cells["F8"].Value = "THICKNESS : " + Pieza.Espesor;
                    xlWorkSheetPage4.Cells["F9"].Value = "FORMULA : " + Pieza.Formula;

                    xlWorkSheetPage1.Cells["B42"].Value = "Inspected By:  " + responsableApariencia + " Date: " + Dia + " / " + Mes + " / " + Anio;

                    xlWorkSheetPage2.Cells["B58"].Value = "Inspected By:  " + responsableApariencia + " Date: " + Dia + " / " + Mes + " / " + Anio;




                    //CARGAR IMAGEN DEFECTO
                    if (Pieza.DefectoImagen != "" && Pieza.DefectoImagen != null)
                    {
                        _HelpExcel.AddImageToSheet(xlWorkSheetPage1, 12, 2, 10, 10, 300, 200, Pieza.DefectoImagen, Pieza.Id.ToString());
                        //cargarImagenHoja(xlWorkSheetDatos, 50, 200, 790, 380, Pieza.DefectoImagen); // todo:borrar
                    }
                        else
                    {

                        _HelpExcel.AddImageToSheet(xlWorkSheetPage1, 11, 2, 10, 10, 300, 200, Pieza.DefectoImagen, Pieza.Id.ToString());
                    }



                    //Cargar los defectos
                    int intContador = 26;
                    foreach (var item in LstDefecto)
                    {
                        xlWorkSheetPage1.Cells["B" + intContador.ToString()].Value = intContador - 25;
                        xlWorkSheetPage1.Cells["C" + intContador.ToString()].Value = item.Zona;
                        xlWorkSheetPage1.Cells["D" + intContador.ToString()].Value = item.DefectoMaestro.Defecto; 
                        xlWorkSheetPage1.Cells["E" + intContador.ToString()].Value = item.Tamanio;
                        xlWorkSheetPage1.Cells["F" + intContador.ToString()].Value = item.Observacion;
                        //xlWorkSheetPage1.Range["AR" + intContador.ToString()].Value = item.Observacion;
                        ++intContador;
                    }


                    //Cargar la inspeccion de apariencia

                    foreach (var item in LstApariencia)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {
                            case "256": //  COLOR
                                xlWorkSheetPage1.Cells["D33"].Value = item.Valor;
                                break;
                            case "297": //   Edge internal package
                                xlWorkSheetPage1.Cells["D34"].Value = item.Valor;
                                break;
                            case "257": //  Logo
                                xlWorkSheetPage1.Cells["D35"].Value = item.Valor;
                                break;
                            case "322": //  Terminals insulation
                                xlWorkSheetPage1.Cells["D36"].Value = item.Valor;
                                break;
                            case "266": //  Finishing and clening edges
                                xlWorkSheetPage1.Cells["G33"].Value = item.Valor;
                                break;
                            /*     case "257": // Package Edge Type
                                         xlWorkSheetPage1.Range["G34"].Value = item.Valor;
                                     break;*/
                            case "324": // Serigraphy
                                xlWorkSheetPage1.Cells["G35"].Value = item.Valor;
                                break;
                            case "388": // Distortion (Mode ON) < 400 Mdop
                                xlWorkSheetPage1.Cells["G36"].Value = item.Valor;
                                break;
                            /*   case "325": // TERMOGRAPHY
                                   xlWorkSheetDatos.Range[(item.Valor == "OK" ? "AL64" : "AS64")].Value = "X";
                                   break;*/

                            default:
                                break;
                        }
                    }



                    // Cargar la inspeccion dimensional

                    foreach (var item in LstDimensional)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {
                            case "239": // PAGE 2 - Espesor - Thickness
                                xlWorkSheetPage2.Cells["D51"].Value = item.Val1;
                                xlWorkSheetPage2.Cells["E51"].Value = item.Val2;
                                xlWorkSheetPage2.Cells["F51"].Value = item.Val3;
                                xlWorkSheetPage2.Cells["G51"].Value = item.Val4;
                                break;
                            case "276": // PAGE 4 - Terminals
                                xlWorkSheetPage4.Cells["D23"].Value = item.Val1;
                                break;
                            case "275": //  PAGE 4 - Resistance Heating Matt (Ω)
                                xlWorkSheetPage4.Cells["D24"].Value = item.Val1;
                                break;
                            case "374": //  PAGE 2 -  SUPERIOR PLANIMETRY
                                xlWorkSheetPage2.Cells["D29"].Value = item.Val1;
                                xlWorkSheetPage2.Cells["D30"].Value = item.Val2;
                                xlWorkSheetPage2.Cells["D31"].Value = item.Val3;
                                xlWorkSheetPage2.Cells["D32"].Value = item.Val4;
                                xlWorkSheetPage2.Cells["D33"].Value = item.Val5;
                                xlWorkSheetPage2.Cells["D34"].Value = item.Val6;
                                xlWorkSheetPage2.Cells["D35"].Value = item.Val7;
                                xlWorkSheetPage2.Cells["D36"].Value = item.Val8;
                                xlWorkSheetPage2.Cells["D37"].Value = item.Val9;
                                xlWorkSheetPage2.Cells["D38"].Value = item.Val10;
                                xlWorkSheetPage2.Cells["D39"].Value = item.Val11;
                                xlWorkSheetPage2.Cells["D40"].Value = item.Val12;
                                xlWorkSheetPage2.Cells["D41"].Value = item.Val13;
                                xlWorkSheetPage2.Cells["D42"].Value = item.Val14;
                                xlWorkSheetPage2.Cells["D43"].Value = item.Val15;
                                xlWorkSheetPage2.Cells["D44"].Value = item.Val16;
                                xlWorkSheetPage2.Cells["D45"].Value = item.Val17;
                                xlWorkSheetPage2.Cells["D46"].Value = item.Val18;
                                xlWorkSheetPage2.Cells["D47"].Value = item.Val19;
                                xlWorkSheetPage2.Cells["D48"].Value = item.Val20;
                                break;
                            case "375": //  PAGE 2 -  CENTRAL PLANIMETRY
                                xlWorkSheetPage2.Cells["E29"].Value = item.Val1;
                                xlWorkSheetPage2.Cells["E30"].Value = item.Val2;
                                xlWorkSheetPage2.Cells["E31"].Value = item.Val3;
                                xlWorkSheetPage2.Cells["E32"].Value = item.Val4;
                                xlWorkSheetPage2.Cells["E33"].Value = item.Val5;
                                xlWorkSheetPage2.Cells["E34"].Value = item.Val6;
                                xlWorkSheetPage2.Cells["E35"].Value = item.Val7;
                                xlWorkSheetPage2.Cells["E36"].Value = item.Val8;
                                xlWorkSheetPage2.Cells["E37"].Value = item.Val9;
                                xlWorkSheetPage2.Cells["E38"].Value = item.Val10;
                                xlWorkSheetPage2.Cells["E39"].Value = item.Val11;
                                xlWorkSheetPage2.Cells["E40"].Value = item.Val12;
                                xlWorkSheetPage2.Cells["E41"].Value = item.Val13;
                                xlWorkSheetPage2.Cells["E42"].Value = item.Val14;
                                xlWorkSheetPage2.Cells["E43"].Value = item.Val15;
                                xlWorkSheetPage2.Cells["E44"].Value = item.Val16;
                                xlWorkSheetPage2.Cells["E45"].Value = item.Val17;
                                xlWorkSheetPage2.Cells["E46"].Value = item.Val18;
                                xlWorkSheetPage2.Cells["E47"].Value = item.Val19;
                                xlWorkSheetPage2.Cells["E48"].Value = item.Val20;
                                break;
                            case "376": //  PAGE 2 -  LOWER PLANIMETRY
                                xlWorkSheetPage2.Cells["F29"].Value = item.Val1;
                                xlWorkSheetPage2.Cells["F30"].Value = item.Val2;
                                xlWorkSheetPage2.Cells["F31"].Value = item.Val3;
                                xlWorkSheetPage2.Cells["F32"].Value = item.Val4;
                                xlWorkSheetPage2.Cells["F33"].Value = item.Val5;
                                xlWorkSheetPage2.Cells["F34"].Value = item.Val6;
                                xlWorkSheetPage2.Cells["F35"].Value = item.Val7;
                                xlWorkSheetPage2.Cells["F36"].Value = item.Val8;
                                xlWorkSheetPage2.Cells["F37"].Value = item.Val9;
                                xlWorkSheetPage2.Cells["F38"].Value = item.Val10;
                                xlWorkSheetPage2.Cells["F39"].Value = item.Val11;
                                xlWorkSheetPage2.Cells["F40"].Value = item.Val12;
                                xlWorkSheetPage2.Cells["F41"].Value = item.Val13;
                                xlWorkSheetPage2.Cells["F42"].Value = item.Val14;
                                xlWorkSheetPage2.Cells["F43"].Value = item.Val15;
                                xlWorkSheetPage2.Cells["F44"].Value = item.Val16;
                                xlWorkSheetPage2.Cells["F45"].Value = item.Val17;
                                xlWorkSheetPage2.Cells["F46"].Value = item.Val18;
                                xlWorkSheetPage2.Cells["F47"].Value = item.Val19;
                                xlWorkSheetPage2.Cells["F48"].Value = item.Val20;
                                break;
                            default:
                                break;
                        }
                    }



                    // Cargar Inspeccion optica
                    
                    foreach (var item in LstOpticos)
                    {
                        switch (item.ParametroInspeccionId.ToString())
                        {
                            // HOJA STICKER ADUANA
                            case "269": // Distortion without zebra  B12
                                        //Microsoft.Office.Interop.Excel.Range oRange = (Microsoft.Office.Interop.Excel.Range) ws.Cells[3, 1];

                                xlWorkSheetPage3 = _HelpExcel.AddImageToSheet(xlWorkSheetPage3, 11, 1, 15, 15, 200, 120, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                break;
                            case "270": // Distortion with zebra 0 grades   E12
                                xlWorkSheetPage3 = _HelpExcel.AddImageToSheet(xlWorkSheetPage3, 11, 4, 15, 15, 200, 120, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                break;
                            case "271": // Distortion with zebra 45 grades   B18
                                xlWorkSheetPage3 = _HelpExcel.AddImageToSheet(xlWorkSheetPage3, 17, 1, 15, 15, 200, 120, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                break;
                            case "366": // Distortion verification (Mode ON) - ISRA   E18
                                xlWorkSheetPage3 = _HelpExcel.AddImageToSheet(xlWorkSheetPage3, 17, 4, 15, 15, 200, 120, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                break;
                            case "388": // Double Vision  pendiente   B24
                                xlWorkSheetPage3 = _HelpExcel.AddImageToSheet(xlWorkSheetPage3, 23, 1, 15, 15, 200, 120, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                break;
                            case "260": // Inner Side   E24
                                xlWorkSheetPage3 = _HelpExcel.AddImageToSheet(xlWorkSheetPage3, 23, 4, 15, 15, 200, 120, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                break;
                            case "371": // Outer Side  B30
                                xlWorkSheetPage3 = _HelpExcel.AddImageToSheet(xlWorkSheetPage3, 29, 1, 15, 15, 200, 120, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                break;
                            case "372": // Edge  E30
                                xlWorkSheetPage3 = _HelpExcel.AddImageToSheet(xlWorkSheetPage3, 29, 4, 15, 15, 200, 120, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                break;
                            case "395": // Edge  B13
                                xlWorkSheetPage3 = _HelpExcel.AddImageToSheet(xlWorkSheetPage3, 29, 1, 15, 15, 400, 180, item.PathImage, item.ParametroInspeccionId.ToString() + "_Optica");
                                break;
                        }
                    }


                    // Guardamos el archivo de Excel con los cambios
                    FileInfo resultadoArchivo = new FileInfo(ResultadoRuta);
                    excelPackage.SaveAs(resultadoArchivo);
                }

                result = true;
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            /*
          
                */
            return result;
        }



    }
}
