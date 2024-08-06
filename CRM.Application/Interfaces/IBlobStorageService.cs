using Azure.Storage.Sas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Interfaces
{
    public interface IBlobStorageService
    {
        Task<List<Uri>> ListBlobsAsync();
        Task UploadBlobAsync(string blobName, Stream stream);
        Task DeleteBlobAsync(string blobName);
        Task DeleteAllBlobsAsync();
        Uri GetBlobSasUri(string blobName, BlobSasPermissions permissions, DateTimeOffset expiration);
    }
}
