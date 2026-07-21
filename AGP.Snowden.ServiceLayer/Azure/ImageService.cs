using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AGP.Snowden.DataAccessLayer.Azure;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;

namespace AGP.Snowden.ServiceLayer.Azure
{
    public class ImageService
    {
        private readonly BlobStorageService _blobStorageService;

        private string _containerSnowden= "snowden";
        private string _folderTemp = "temp/";
        private string _packingListFolderPE = "packinglist/peru/";
        public ImageService(BlobStorageService blobStorageService)
        {
            _blobStorageService = blobStorageService;
        }

        public async Task<string> UploadImageAsync(Stream imageStream, string fileName,string folder="temp/")
        {
            var containerName = _containerSnowden;
            var folderTempName = folder;
            var blobName = $"{Guid.NewGuid().ToString()}-{fileName}"; // snowden

            await _blobStorageService.UploadFileAsync(containerName, folderTempName, blobName, imageStream);

            // Construct the URL to the blob
            var blobUrl = $"https://azsasharepointfiles.blob.core.windows.net/{containerName}/{folderTempName}/{blobName}";
            return blobName;
        }

        public async Task<bool> FileExistsAsync(string sourceFileName)
        {
            try
            {
                //await _blobStorageService.CopyFileAsync(_containerSnowden, _folderTemp + sourceFileName, _packingListFolderPE + destinationFileName);
                return await _blobStorageService.FileExistsAsync(_containerSnowden, _folderTemp + sourceFileName);
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción y registrarla
                Console.WriteLine($"Error al verificar la existencia del archivo en Azure Blob Storage: {ex.Message}");
                throw; // Propagar la excepción para que pueda ser manejada por el código que llama a este método
            }
        }

        public async Task CopyImgeFileAsync(string sourceFileName, string destinationFileName)
        {
            try
            {
                string folderTemp = _folderTemp;

                await _blobStorageService.CopyFileAsync(_containerSnowden, _folderTemp + sourceFileName, _packingListFolderPE+ destinationFileName);
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }
        }

        public async Task DeleteImageFileAsync(string sourceFileName)
        {
            try
            {
                await _blobStorageService.DeleteFileAsync(_containerSnowden, _folderTemp + sourceFileName);
            }
            catch (Exception ex)
            {
                throw new System.ArgumentException(ex.Message, "");
            }
        }

        
    }
}
