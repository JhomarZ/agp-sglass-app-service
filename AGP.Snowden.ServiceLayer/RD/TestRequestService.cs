using AGP.Snowden.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGP.Snowden.ServiceLayer.RD
{
    public class TestRequestService
    {

        public DataTable GetDataMeasurementPivot(string TestRequestIds)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (var db = new DbSnowdenContext())
                {
                    DbConnection connection = db.Database.GetDbConnection();
                    DbProviderFactory dbFactory = DbProviderFactories.GetFactory(connection);

                    
                    using (var cmd = dbFactory.CreateCommand())
                    {
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "exec [dbo].[SP_TEST_REQUEST_DATA_PBI_UNPIVOT] 'PE02','"+ TestRequestIds + "','','','',null,null,null";
                        /*
                        if (parameters != null)
                        {
                            foreach (var item in parameters)
                            {
                                cmd.Parameters.Add(item);
                            }
                        }*/
                        using (DbDataAdapter adapter = dbFactory.CreateDataAdapter())
                        {
                            adapter.SelectCommand = cmd;
                            adapter.Fill(dataTable);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }

            /*
          
                */
            return dataTable;
        }

        public bool LoadExcelDataMeasurement(string PlantillaMacro, string ResultadoRuta, DataTable Data)
        {
            bool result = false;
            FileInfo plantillaArchivo = new FileInfo(PlantillaMacro);
            try
            {
                
                using (ExcelPackage excelPackage = new ExcelPackage(plantillaArchivo))
                {

                    ExcelWorksheet xlWorkSheetData = excelPackage.Workbook.Worksheets["DATA"];
                    //xlWorkSheet = xlWorkBook.Sheets["DATOS"];
                    xlWorkSheetData.Cells["A2"].LoadFromDataTable(Data);
                    xlWorkSheetData.Cells["U1:U25000"].Style.Numberformat.Format = "#,##0.00";
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
