using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace MoviesApi.Services
{
    public class AzureStorage : IStorage


    {
        private readonly string connectionString;

        public AzureStorage(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("AzureStorage");
        }   


        public async Task DeleteFile(string container, string route)
        {
           if( string.IsNullOrEmpty(route) )
            {
                return;
            }

            var client = new BlobContainerClient(connectionString, container);
            await client.CreateIfNotExistsAsync();
            var file = Path.GetFileName(route);
            var blob = client.GetBlobClient(file);
            await blob.DeleteIfExistsAsync();

        }

        public async Task<string> EditFile(byte[] content, string extension, string container, string contentType, string route)
        {
            await DeleteFile(route, container);
            return await StoreFile(content, extension, contentType, route);

        }

        public async Task<string> StoreFile(byte[] content, string extension, string container, string contentType)
        {
            var client = new BlobContainerClient(connectionString, container);
            await client.CreateIfNotExistsAsync();
            client.SetAccessPolicy(PublicAccessType.Blob);

            var file = $"{Guid.NewGuid()}{extension}";
            var blob = client.GetBlobClient(file) ;

            var blobUploadOptions = new BlobUploadOptions();
            var blobHttpHeaders = new BlobHttpHeaders();

            blobHttpHeaders.ContentType = contentType;
            blobUploadOptions.HttpHeaders = blobHttpHeaders;

            await blob.UploadAsync(new BinaryData(content), blobUploadOptions);
            return blob.Uri.ToString();

        }
    }
}
