using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace ABCRETAIL.Services
{
    public class BlobStorageService
    {
        private readonly BlobContainerClient _container;

        public BlobStorageService(BlobServiceClient svc)
        {
            _container = svc.GetBlobContainerClient("media");
            _container.CreateIfNotExists();
        }

        public async Task<string> UploadAsync(Stream file, string fileName, string contentType)
        {
            var blob = _container.GetBlobClient(fileName);
            await blob.UploadAsync(file, overwrite: true);
            await blob.SetHttpHeadersAsync(new Azure.Storage.Blobs.Models.BlobHttpHeaders { ContentType = contentType });
            return blob.Name;
        }

        public Uri GetReadSasUri(string blobName, int minutes = 30)
        {
            var blob = _container.GetBlobClient(blobName);
            if (!blob.CanGenerateSasUri)
                throw new InvalidOperationException("SAS generation not supported without account key.");

            return blob.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.AddMinutes(minutes));
        }

        public async Task<List<string>> ListAsync()
        {
            var list = new List<string>();
            await foreach (var item in _container.GetBlobsAsync())
                list.Add(item.Name);
            return list;
        }

        // Delete a blob by name
        public async Task DeleteAsync(string blobName)
        {
            var blob = _container.GetBlobClient(blobName);
            await blob.DeleteIfExistsAsync();
        }
    }
}


// REFERENCES:
// ASP.NET Core MVC Documentation:
// https://learn.microsoft.com/en-us/aspnet/core/mvc/overview?view=aspnetcore-8.0

// ASP.NET Core Forms and Model Binding:
// https://learn.microsoft.com/en-us/aspnet/core/mvc/models/model-binding?view=aspnetcore-8.0

// Azure Storage Queues (if you're integrating queues like in earlier code):
// https://learn.microsoft.com/en-us/azure/storage/queues/storage-dotnet-how-to-use-queues

// Working with Forms in Razor Views:
// https://learn.microsoft.com/en-us/aspnet/core/mvc/views/overview?view=aspnetcore-8.0

// Handling File Uploads in ASP.NET Core:
// https://learn.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-8.0

// Bootstrap for responsive styling (used in views):
// https://getbootstrap.com/docs/5.3/getting-started/introduction/

// Errors within this implementation were assisted by ChatGPT (OpenAI) for help
// https://openai.com/chatgpt
