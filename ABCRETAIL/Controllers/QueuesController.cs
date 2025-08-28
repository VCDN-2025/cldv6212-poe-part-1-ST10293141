using ABCRETAIL.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ABCRETAIL.Controllers
{
    public class QueuesController : Controller
    {
        private readonly QueueService _queue;

        public QueuesController(QueueService queue) => _queue = queue;

        public async Task<IActionResult> Index()
        {
            var messages = await _queue.PeekAsync();
            return View(messages);
        }

        [HttpPost]
        public async Task<IActionResult> Enqueue(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
                await _queue.EnqueueAsync(text);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string messageId, string popReceipt)
        {
            if (!string.IsNullOrEmpty(messageId) && !string.IsNullOrEmpty(popReceipt))
                await _queue.DeleteMessageAsync(messageId, popReceipt);

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
