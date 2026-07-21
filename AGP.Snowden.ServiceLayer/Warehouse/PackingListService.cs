using AGP.Snowden.DataAccessLayer.SAP;
using AGP.Snowden.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;
using QRCoder;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AGP.Snowden.ServiceLayer.Warehouse
{
    public class PackingListService
    {
        public async Task<List<PackingList>> GetAll(int Skip = 0, int Take = 30, string? Observation = "",string? FechaInicio="", string? Fechafin = "", string? Status = "")
        {
            List<PackingList> lista = new List<PackingList>();
            using (var db = new DbSnowdenContext())
            {
                var Query = db.PackingLists.AsQueryable();


                if (Observation != null && Observation != "")
                {
                    Query = Query.Where(d => (d.Observation).Contains(Observation));
                }

                if (FechaInicio != null && FechaInicio != "" && Fechafin != null && Fechafin != "")
                {
                    Query = Query.Where(d => Convert.ToInt32(d.CreatedAt) >= Convert.ToInt32(FechaInicio) && Convert.ToInt32(d.CreatedAt) >= Convert.ToInt32(Fechafin));
                }


                lista = await Query.OrderByDescending(x => x.Id).Skip(Skip).Take(Take).ToListAsync();

            }

            return lista;
        }
        public async Task<PackingList> GetOne(int? id)
        {
            PackingList record = new PackingList();
            using (var db = new DbSnowdenContext())
            {
                record = await db.PackingLists.FindAsync(id);

            }

            return record;
        }

        public async Task<PackingList> Add(PackingList packingList)
        {
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    packingList.Active = true;
                    db.PackingLists.AddAsync(packingList);
                    db.SaveChanges();

                    //Agregamos el status en el historial
                    PackingListStatusHistory statusHistory = new PackingListStatusHistory();
                    statusHistory.CreatedAt = DateTime.Now;
                    statusHistory.CreatedBy = packingList.CreatedBy;
                    statusHistory.Status = packingList.Status;
                    statusHistory.PackageId = packingList.Id;
                    db.PackingListStatusHistories.Add(statusHistory);
                    db.SaveChanges();
                }
            }

            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }
          
            return packingList;
        }

        public async Task<PackingList> Update(int id,PackingList packingList)
        {
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    //PackingList currentPacking = await db.PackingLists.FindAsync(id);
                    db.PackingLists.Update(packingList);
                    db.SaveChanges();

                    /*
                    //Agregamos el status en el historial
                    if(currentPacking != null)
                    {
                        if(currentPacking.Status!= packingList.Status)
                        {
                            PackingListStatusHistory statusHistory = new PackingListStatusHistory();
                            statusHistory.CreatedAt = DateTime.Now;
                            statusHistory.CreatedBy = packingList.CreatedBy;
                            statusHistory.Status = packingList.Status;
                            statusHistory.PackageId = packingList.Id;
                            db.PackingListStatusHistories.Add(statusHistory);
                            db.SaveChanges();
                        }
                            
                    }*/
                    
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }
            return packingList;
        }

        public async Task<PackingList> DeletePackingList(PackingList packingList)
        {
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    packingList.Active = false;
                    db.PackingLists.Update(packingList);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }
            return packingList;
        }

        public async Task<PackingListItem> GetOnePackingItemByKey(string CentroSap,string DocumentoCompra,string NroPosicion, string Mblnr)
        {
            PackingListItem packingListItem = new PackingListItem();
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    packingListItem=await db.PackingListItems.Where(x=>x.CentroSap+x.DocumentoCompra+x.NroPosicion+x.Mblnr== CentroSap+ DocumentoCompra+ NroPosicion+ Mblnr).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }
            return packingListItem;
        }

        public async Task<PackingListItem> GetOnePackingItemB(int? PackingListId, string CentroSap,string DocumentoCompra, string NroPosicion, string Mblnr)
        {
            PackingListItem packingListItem = new PackingListItem();
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    packingListItem = await db.PackingListItems.Where(x => x.PackingListId==PackingListId && ( x.CentroSap + x.DocumentoCompra + x.NroPosicion + x.Mblnr == CentroSap+DocumentoCompra + NroPosicion + Mblnr)).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }
            return packingListItem;
        }
        public async Task<PackingListItem> AddPackingItem(PackingListItem packingListItem)
        {
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    db.PackingListItems.Add(packingListItem);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }
            return packingListItem;
        }
        public async Task<PackingListItem> UpdatePackingItem(PackingListItem packingListItem)
        {
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    db.PackingListItems.Update(packingListItem);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }
            return packingListItem;
        }

      
        public async Task<List<PackingListItem>> GetAllPackingListItems(int id)
        {
            List<PackingListItem> lista = new List<PackingListItem>();
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    lista = await db.PackingListItems.Where(x => x.PackingListId == id).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }
            return lista;
        }

        public async Task<PackingListItem> GetOnePackingListItem(int id)
        {
            PackingListItem item = new PackingListItem();
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    item = await db.PackingListItems.FindAsync(id);
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }
            return item;
        }
        public async Task<List<PackingListItem>> DeletePackingListItem(PackingListItem packingListItem)
        {
            List<PackingListItem> lista = new List<PackingListItem>();
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    db.PackingListItems.Remove(packingListItem);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }
            return lista;
        }

        public async Task<ImputadoStatusHistory> AddImputadoStatusHistory(ImputadoStatusHistory imputado)
        {
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    db.ImputadoStatusHistories.Add(imputado);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }
            return imputado;
        }

        public string GenerarEtiquetaZebraPDF(byte[] QR, PackingList packing, int Copias = 1)
        {

            string fileName = "Reports/Results/label.pdf";

            try
            {
                var document = QuestPDF.Fluent.Document.Create(container =>
                {
                    // page 1 content  size 15 x 10 cm
                    for (int c = 0; c < Copias; c++)
                    {
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

                                row.ConstantItem(388).Background(Colors.White).Border(1).AlignMiddle().AlignCenter().Text(
                                    text =>
                                    {
                                        text.Span("PACKING LIST N° "+ packing.Id.ToString()).FontFamily(Fonts.Arial).FontSize(18).FontColor(Colors.Black).Bold();
                                    });
                                fs.Dispose();
                                fs.Close();
                                row.ConstantItem(50).Background(Colors.White).Border(0).AlignMiddle().AlignCenter().Text(text =>
                                {
                                    text.Span(packing.CreatedAt.ToString().Substring(0,10)).FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Black);
                                    text.Span("Version 1").FontFamily(Fonts.Arial).FontSize(8).FontColor(Colors.Black);
                                }); //.Image("logo.jpg");
                            });

                            page.Content().PaddingVertical(3, Unit.Millimetre)
                           .Column(column =>
                           {
                               column.Item().Row(row =>
                               {
                                   row.ConstantItem(538).AlignMiddle().Border(0).Table(table =>
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
                                           columns.ConstantColumn(118);
                                           columns.ConstantColumn(420);
                                           //for (int i = 0; i < 2; i++)
                                           //{
                                           //    columns.ConstantColumn(538 / 2); // Dividiendo el ancho total entre 5
                                           //}


                                       });
                                       /*
                                       table.Header(header =>
                                       {
                                           // please be sure to call the 'header' handler!
                                           header.Cell().Element(CellStyle).Text("OC").FontFamily(Fonts.Arial).FontSize(10).Bold();
                                           header.Cell().Element(CellStyle).Text("MATERIAL").FontFamily(Fonts.Arial).FontSize(9).Bold();

                                           // you can extend existing styles by creating additional methods
                                       });*/

                                       table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text("Destino:").FontFamily(Fonts.Arial).FontSize(14).Bold();
                                       table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignLeft().Text(packing.PlantDestination).FontFamily(Fonts.Arial).FontSize(14);
                                       table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text("Guia:").FontFamily(Fonts.Arial).FontSize(14).Bold();
                                       table.Cell().Border(0).BorderColor(Colors.Grey.Lighten1).AlignLeft().Text(packing.GuideNumber).FontFamily(Fonts.Arial).FontSize(14);

                                   });
                               });
                                   column.Item().Row(row =>
                               {

                                   row.ConstantItem(538).AlignMiddle().Border(0).Table(table =>
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
                                           for (int i = 0; i < 5; i++)
                                           {
                                               columns.ConstantColumn(538 / 5); // Dividiendo el ancho total entre 5
                                           }

                                           
                                       });

                                       table.Header(header =>
                                       {
                                           // please be sure to call the 'header' handler!
                                           header.Cell().Element(CellStyle).Text("OC").FontFamily(Fonts.Arial).FontSize(10).Bold();
                                           header.Cell().Element(CellStyle).Text("MATERIAL").FontFamily(Fonts.Arial).FontSize(9).Bold();
                                           header.Cell().Element(CellStyle).Text("DESCRIPCIÓN").FontFamily(Fonts.Arial).FontSize(9).Bold();
                                           header.Cell().Element(CellStyle).Text("CANT").FontFamily(Fonts.Arial).FontSize(9).Bold();
                                           header.Cell().Element(CellStyle).Text("UM").FontFamily(Fonts.Arial).FontSize(9).Bold();
                                           
                                           // you can extend existing styles by creating additional methods
                                       });

                                       foreach (PackingListItem item in packing.Imputados)
                                       {
                                           table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(item.DocumentoCompra).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                           table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(item.TrackingImputadosSap.NumeroMaterial).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                           table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(item.TrackingImputadosSap.DescripcionMaterial).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                           table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(item.TrackingImputadosSap.CntPedido).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                           table.Cell().Border(1).BorderColor(Colors.Grey.Lighten1).AlignCenter().Text(item.TrackingImputadosSap.Umb).FontFamily(Fonts.Arial).FontSize(7).Bold();
                                       }
                                       QuestPDF.Infrastructure.IContainer CellStyle(QuestPDF.Infrastructure.IContainer container) => DefaultCellStyle(container, Colors.Grey.Lighten3);
                                   });
                               });

                               column.Spacing(10);

                               column.Item().AlignCenter().Row(row =>
                               {
                                   row.ConstantItem(538).Background(Colors.White).Border(0).Table(table =>
                                   {


                                       table.ColumnsDefinition(columns =>
                                       {
                                           columns.ConstantColumn(120);
                                       });

                                       table.Cell().Background(Colors.White).Border(0).AlignMiddle().Image(QR).FitWidth();
                                   });
                               });

                           });
                           #endregion
                           /*

                           page.Content().Border(0).PaddingVertical(0, Unit.Millimetre)
                          .Column(column =>
                          {


                              column.Item().AlignCenter().Row(row =>
                              {
                                  row.ConstantItem(520).Background(Colors.White).Border(0).Table(table =>
                                  {
                                      table.ColumnsDefinition(columns =>
                                      {
                                          // Definiendo 5 columnas
                                          for (int i = 0; i < 5; i++)
                                          {
                                              columns.ConstantColumn(520 / 5); // Dividiendo el ancho total entre 5
                                          }
                                      });

                                      // Encabezado de la tabla
                                      table.Header(header =>
                                      {
                                          // Añadiendo encabezados para cada columna
                                          for (int i = 0; i < 5; i++)
                                          {
                                              header.Cell().Text($"Encabezado {i + 1}").FontFamily(Fonts.Arial).FontSize(9).Bold();
                                          }
                                      });

                                      // Contenido de la tabla
                                      // Aquí debes añadir las celdas de la tabla como sea necesario
                                      // Ejemplo: Añadiendo una fila de celdas
                                      // Añadir más filas según sea necesario...
                                  });

                              });
                          });
                           */
                       });
                    }
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

        public async Task<List<PackingListStatusHistory>> GetHistoryStatus(int id)
        {
            List<PackingListStatusHistory> list = new List<PackingListStatusHistory>();
            using (var db = new DbSnowdenContext())
            {
                list = await db.PackingListStatusHistories.Where(x=>x.PackageId==id).OrderByDescending(x=>x.CreatedAt).ToListAsync();

            }

            return list;
        }

        public async Task<List<ImputadoStatusHistory>> GetHistoryStatusImputado(string CentroSap, string DocumentoCompra, string NroPosicion, string Mblnr)
        {
            List<ImputadoStatusHistory> list = new List<ImputadoStatusHistory>();
            using (var db = new DbSnowdenContext())
            {
                list = await db.ImputadoStatusHistories.Where(x => x.PlantSap + x.PurchaseOrder + x.NroPosition + x.MBLNR == CentroSap + DocumentoCompra + NroPosicion + Mblnr).OrderBy(x => x.CreatedAt).ToListAsync();

            }

            return list;
        }


    }
}
