using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Image = System.Drawing.Image;
using System.Drawing;
using System.Net;
using System.IO;
using System.Drawing.Drawing2D;

using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System.IO;
using ImageSL = SixLabors.ImageSharp.Image;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Jpeg;
using static System.Net.Mime.MediaTypeNames;

namespace AGP.Gordon.CommonLayer
{
    public class HelpImage
    {
        public byte[] ResizeImage(byte[] imageData, int newWidth, int newHeight)
        {
            using var ms = new MemoryStream(imageData);
            using var image = ImageSL.Load(ms);

            var resizeOptions = new ResizeOptions
            {
                Size = new SixLabors.ImageSharp.Size(newWidth, newHeight),
                Mode = ResizeMode.Stretch
            };

            image.Mutate(x => x.Resize(resizeOptions));

            using var outputMs = new MemoryStream();
            image.Save(outputMs, new JpegEncoder());

            return outputMs.ToArray();
        }
        public  byte[] ResizeImage__(byte[] imageData, int newWidth, int newHeight)
        {
            using var ms = new MemoryStream(imageData);
            using var image = Image.FromStream(ms);

            var resizedImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(resizedImage))
            {
                /*
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;*/
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

                graphics.DrawImage(image, new System.Drawing.Rectangle(0, 0, newWidth, newHeight));
            }

            using var outputMs = new MemoryStream();
            resizedImage.Save(outputMs, image.RawFormat);

            return outputMs.ToArray();
        }

        public byte[] ResizeImage_(byte[] imageData, int newWidth, int newHeight)
        {
            using var ms = new MemoryStream(imageData);
            using var image = System.Drawing.Image.FromStream(ms);

            // Crea un nuevo bitmap con las dimensiones redimensionadas
            using var resizedImage = new Bitmap(newWidth, newHeight);

            // Dibuja la imagen redimensionada en el nuevo bitmap
            using var graphics = Graphics.FromImage(resizedImage);
            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            // Convierte la imagen redimensionada a un arreglo de bytes
            using var outputMs = new MemoryStream();
            resizedImage.Save(outputMs, image.RawFormat);

            return outputMs.ToArray();
        }

        public async Task<byte[]> ConvertImageUrlToByte(string imageUrl)
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
            catch (Exception ex)
            {
                return null;
            }


            return byteArray;
        }

        public async Task<bool> SaveImageFromUrl(string ImageUrl,string Destino)
        {
            bool result = false;
            try
            {
                WebClient client = new WebClient();
                Stream stream = await client.OpenReadTaskAsync(ImageUrl);
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);
                img.Save(Destino);
                img.Dispose();
                result = true;
            }
            catch (Exception ex)
            {
                return result;
            }


            return result;
        }

        public async Task<bool> SaveImageFromUrlAndResize(string ImageUrl, string Destino,int NewWidth, int NewHeight)
        {
            bool result = false;
            try
            {
                WebClient client = new WebClient();
                Stream stream = await client.OpenReadTaskAsync(ImageUrl);
                System.Drawing.Image img = System.Drawing.Image.FromStream(stream);

                img = ResizeImage(img, NewWidth, NewHeight);

                img.Save(Destino);
                img.Dispose();
                result = true;
            }
            catch (Exception ex)
            {
                return result;
            }


            return result;
        }

        public System.Drawing.Image ResizeImage(System.Drawing.Image image, int newWidth, int newHeight)
        {
            Bitmap resizedImage = new Bitmap(newWidth, newHeight);
            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return resizedImage;
        }

        public static System.Drawing.Image ResizeImage(string pathImage, int newWidth, int newHeight)
        {
            System.Drawing.Image image = System.Drawing.Image.FromFile(pathImage);
            Bitmap resizedImage = new Bitmap(newWidth, newHeight);
            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return resizedImage;
        }

        public void EmptyFolder(DirectoryInfo directoryInfo)
        {
            
            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }

            foreach (DirectoryInfo subfolder in directoryInfo.GetDirectories())
            {
                EmptyFolder(subfolder);
                subfolder.Delete();
            }
        }

        public static void DeleteFile(string pathFile)
        {
            if(!File.Exists(pathFile))
                File.Delete(pathFile);

        }

        public static byte[] GetFileContent(string FilePath)
        {
            
            // Lee el contenido del archivo en un arreglo de bytes
            byte[] fileBytes;
            using (FileStream fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            {
                fileBytes = new byte[fileStream.Length];
                fileStream.Read(fileBytes, 0, (int)fileStream.Length);
            }

            return fileBytes;
        }

    }
}
