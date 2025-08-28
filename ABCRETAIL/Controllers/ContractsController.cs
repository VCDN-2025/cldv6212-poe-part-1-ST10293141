using ABCRETAIL.Models;
using ABCRETAIL.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRETAIL.Controllers
{
    public class ContractsController : Controller
    {
        private readonly FileShareService _files;

       
        private static List<Order> Orders = new List<Order>();

        public ContractsController(FileShareService files) => _files = files;

        // /Contracts
        public async Task<IActionResult> Index()
        {
            var files = await _files.ListAsync();
            ViewBag.Files = files;

            return View(Orders); // Pass orders as model
        }

        // POST: /Contracts/PlaceOrder
        [HttpPost]
        public IActionResult PlaceOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                Orders.Add(order);
            }

            return RedirectToAction(nameof(Index));
        }

        // /Contracts/DeleteOrder
        [HttpPost]
        public IActionResult DeleteOrder(int index)
        {
            if (index >= 0 && index < Orders.Count)
            {
                Orders.RemoveAt(index);
            }

            return RedirectToAction(nameof(Index));
        }

        // /Contracts/Download?name=...
        public async Task<IActionResult> Download(string name)
        {
            var stream = await _files.DownloadAsync(name);
            return File(stream, "application/octet-stream", name);
        }

        // (Optional) legacy upload page — no longer needed for your current design
        [HttpGet]
        public IActionResult Upload() => View();

        // POST: /Contracts/Upload
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return View();

            var name = $"{Guid.NewGuid()}_{file.FileName}";
            using var s = file.OpenReadStream();
            await _files.UploadAsync(s, name);

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
