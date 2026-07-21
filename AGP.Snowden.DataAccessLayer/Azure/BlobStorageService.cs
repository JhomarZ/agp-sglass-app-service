using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;


namespace AGP.Snowden.DataAccessLayer.Azure
{
    public class BlobStorageService
    {
        private readonly CloudBlobClient _blobClient;

        public BlobStorageService(string connectionString)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            _blobClient = storageAccount.CreateCloudBlobClient();
        }

        public async Task UploadFileAsync(string containerName, string folderName, string  blobName, Stream fileStream)
        {
            try
            {
                var container = _blobClient.GetContainerReference(containerName);
                await container.CreateIfNotExistsAsync();

                var fullBlobName = folderName + "/" + blobName;

                var blockBlob = container.GetBlockBlobReference(fullBlobName);

                string contentType = GetContentType(blobName);
                blockBlob.Properties.ContentType = contentType;

                // Configurar la propiedad Content-Disposition para que el archivo se visualice en el navegador
                //   blockBlob.Properties.ContentDisposition = "inline";
                //blockBlob.Properties.ContentType = "image/jpeg";

                // Reiniciar el puntero del flujo al principio
                fileStream.Seek(0, SeekOrigin.Begin);

                await blockBlob.UploadFromStreamAsync(fileStream);
            }
            catch(Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }
          
        }

        public async Task<bool> FileExistsAsync(string containerName, string sourceBlobName)
        {
            try
            {
                var container = _blobClient.GetContainerReference(containerName);
                var sourceBlob = container.GetBlockBlobReference(sourceBlobName);

                return await sourceBlob.ExistsAsync();
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción y registrarla
                Console.WriteLine($"Error al verificar la existencia del archivo en Azure Blob Storage: {ex.Message}");
                throw; // Propagar la excepción para que pueda ser manejada por el código que llama a este método
            }
        }

        public async Task CopyFileAsync(string containerName, string sourceBlobName, string destinationBlobName)
        {
            try
            {
                var container = _blobClient.GetContainerReference(containerName);
                var sourceBlob = container.GetBlockBlobReference(sourceBlobName);
                var destinationBlob = container.GetBlockBlobReference(destinationBlobName);

                // Copiar el blob al nuevo nombre
                await destinationBlob.StartCopyAsync(sourceBlob);

                // Esperar a que se complete la copia
                while (destinationBlob.CopyState.Status == CopyStatus.Pending)
                {
                    await Task.Delay(1000);
                    await destinationBlob.FetchAttributesAsync();
                }

                // Verificar si la copia se completó correctamente
                if (destinationBlob.CopyState.Status != CopyStatus.Success)
                {
                    throw new InvalidOperationException("Failed to copy the blob.");
                }

            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción y registrarla
                Console.WriteLine($"Error al mover el archivo en Azure Blob Storage: {ex.Message}");
                throw; // Propagar la excepción para que pueda ser manejada por el código que llama a este método
            }
        }

        public async Task DeleteFileAsync(string containerName, string sourceBlobName)
        {
            try
            {
                var container = _blobClient.GetContainerReference(containerName);
                var sourceBlob = container.GetBlockBlobReference(sourceBlobName);
               
                // Eliminar el blob original si es necesario
                await sourceBlob.DeleteIfExistsAsync();
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }
        }

        public async Task<Stream> DownloadFileAsync(string containerName, string blobName)
        {
            var container = _blobClient.GetContainerReference(containerName);
            var blockBlob = container.GetBlockBlobReference(blobName);
            MemoryStream memoryStream = new MemoryStream();
            await blockBlob.DownloadToStreamAsync(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        private string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            var mapping = new Dictionary<string, string>
            {
                {".pdf", "application/pdf"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".png", "image/png"},
                {".doc", "application/msword"},
                {".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
                {".txt", "text/plain"},
                {".zip", "application/zip"}
            };

            return mapping.TryGetValue(extension, out string contentType) ? contentType : "application/octet-stream";
        }
    }
}
