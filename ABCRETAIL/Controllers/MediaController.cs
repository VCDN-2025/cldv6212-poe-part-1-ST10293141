using ABCRETAIL.Models;
using ABCRETAIL.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRETAIL.Controllers
{
    public class MediaController : Controller
    {
        private readonly BlobStorageService _blobs;
        private readonly QueueService _queue;

        public MediaController(BlobStorageService blobs, QueueService queue)
        {
            _blobs = blobs;
            _queue = queue;
        }

        public async Task<IActionResult> Index()
        {
            var names = await _blobs.ListAsync();
            var files = names.Select(name => new MediaViewModel
            {
                Name = name,
                Url = _blobs.GetReadSasUri(name).ToString()
            }).ToList();

            return View(files);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return RedirectToAction(nameof(Index));

            var blobName = $"{Guid.NewGuid()}_{file.FileName}";
            using var stream = file.OpenReadStream();

            await _blobs.UploadAsync(stream, blobName, file.ContentType);
            await _queue.EnqueueAsync($"Uploaded image {blobName}");

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
                await _blobs.DeleteAsync(name);

            return RedirectToAction(nameof(Index));
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
