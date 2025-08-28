using Azure.Storage.Files.Shares;

namespace ABCRETAIL.Services
{
    public class FileShareService
    {
        private readonly ShareClient _share;

        public FileShareService(ShareServiceClient svc)
        {
            _share = svc.GetShareClient("contracts");
            _share.CreateIfNotExists();
        }

        public async Task UploadAsync(Stream file, string fileName)
        {
            var root = _share.GetRootDirectoryClient();
            await root.CreateIfNotExistsAsync();

            var fc = root.GetFileClient(fileName);
            await fc.CreateAsync(file.Length);
            await fc.UploadRangeAsync(
                new Azure.HttpRange(0, file.Length),
                file);
        }

        public async Task<List<string>> ListAsync()
        {
            var list = new List<string>();
            var root = _share.GetRootDirectoryClient();
            await foreach (var i in root.GetFilesAndDirectoriesAsync())
                if (!i.IsDirectory) list.Add(i.Name);
            return list;
        }

        public async Task<Stream> DownloadAsync(string name)
        {
            var root = _share.GetRootDirectoryClient();
            var fc = root.GetFileClient(name);
            var dl = await fc.DownloadAsync();
            var ms = new MemoryStream();
            await dl.Value.Content.CopyToAsync(ms);
            ms.Position = 0;
            return ms;
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
