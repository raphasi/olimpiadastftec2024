using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using CRM.Application.Interfaces;
using CRM.Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.Application.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _blobContainerName;
    private BlobContainerClient _blobContainerClient;
    private readonly ILogger<BlobStorageService> _logger;

    public BlobStorageService(BlobServiceClient blobServiceClient, IOptions<ConfigurationImagens> options, ILogger<BlobStorageService> logger)
    {
        _blobServiceClient = blobServiceClient;
        _blobContainerName = options.Value.RepositorioBlob;
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient(_blobContainerName);
        _logger = logger;
    }
    
    public async Task<List<Uri>> ListBlobsAsync()
    {
        var blobs = new List<Uri>();
        try
        {
            await _blobContainerClient.CreateIfNotExistsAsync();

            foreach (BlobItem blobItem in _blobContainerClient.GetBlobs())
            {
                if (blobItem.Properties.BlobType == BlobType.Block)
                {
                    var sasUri = GetBlobSasUri(blobItem.Name, BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddHours(1));
                    blobs.Add(sasUri);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar blobs no contêiner {ContainerName}", _blobContainerName);
        }

        return blobs;
    }

    public async Task UploadBlobAsync(string blobName, Stream stream)
    {
        var blobClient = _blobContainerClient.GetBlobClient(blobName);
        await blobClient.UploadAsync(stream, overwrite: true);
    }

    public async Task DeleteBlobAsync(string blobName)
    {
        var blobClient = _blobContainerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync();
    }

    public async Task DeleteAllBlobsAsync()
    {
        await foreach (BlobItem blobItem in _blobContainerClient.GetBlobsAsync())
        {
            if (blobItem.Properties.BlobType == BlobType.Block)
            {
                await _blobContainerClient.DeleteBlobIfExistsAsync(blobItem.Name);
            }
        }
    }

    public Uri GetBlobSasUri(string blobName, BlobSasPermissions permissions, DateTimeOffset expiration)
    {
        var blobClient = _blobContainerClient.GetBlobClient(blobName);

        if (blobClient.CanGenerateSasUri)
        {
            var sasBuilder = new BlobSasBuilder(permissions, expiration)
            {
                BlobContainerName = _blobContainerClient.Name,
                BlobName = blobName
            };

            return blobClient.GenerateSasUri(sasBuilder);
        }
        else
        {
            throw new InvalidOperationException("O cliente de blob não pode gerar um URI SAS.");
        }
    }
}
