using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OfficeOpenXml.Drawing;
using System.Drawing;

namespace AGP.Gordon.CommonLayer
{
    public class HelpExcel
    {
        private readonly HelpImage _HelpImage;
        // GET: STable
        public HelpExcel()
        {
            _HelpImage = new HelpImage();
        }
        public ExcelWorksheet AddShapeToExcel(ExcelWorksheet Worksheet,string ShapeName, string ShapeText,int Row, int Column, int SizeWidth, int SizeHeight,int marginTop= 0, int marginLeft = 0)
        {
            SizeWidth = SizeWidth + 15;
            SizeHeight = SizeHeight + 5;
            var shape = Worksheet.Drawings.AddShape(ShapeName, eShapeStyle.Rect);
            shape.SetPosition(Row, marginTop, Column, marginLeft);
            shape.SetSize(SizeWidth, SizeHeight);
            shape.Text = ShapeText;
            shape.Fill.Color = System.Drawing.Color.Transparent;
            shape.Font.Size = 7;
            shape.Font.Color = System.Drawing.Color.Black;
            return Worksheet;
        }

        public ExcelWorksheet AddImageToSheet(ExcelWorksheet WorkSheet, int Row,int Column,int Left,int Top ,int WidthSize, int HeightSize, string ImageRuta, string ImagenName)
        {
            // Cargamos la imagen y la redimensionamos antes de insertarla en el archivo Excel
            if (!string.IsNullOrEmpty(ImageRuta))
            {
                FileInfo imagenArchivo = new FileInfo(ImageRuta);
                System.Drawing.Image image = System.Drawing.Image.FromFile(ImageRuta);
                System.Drawing.Image resizedImage = _HelpImage.ResizeImage(image, WidthSize, HeightSize);

                string imagenTemporal = ImageRuta.Replace(".jpg", "_tmp.jpg");
                resizedImage.Save(imagenTemporal);

                FileInfo imagenTemporalArchivo = new FileInfo(imagenTemporal);

                ExcelPicture excelPicture = WorkSheet.Drawings.AddPicture(ImagenName, imagenTemporalArchivo);
                excelPicture.SetPosition(Row, Left, Column, Top);

                /*
                if (!string.IsNullOrEmpty(imagenTemporal))
                {
                    File.Delete(imagenTemporal);
                }
                */
                //ExcelPicture excelPicture = WorkSheet.Drawings.AddPicture(ImagenName,, imagenArchivo);

            }

          
            return WorkSheet;
        }
    }
}
