using ABCRETAIL.Models;
using ABCRETAIL.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRETAIL.Controllers
{
    public class ProductsController : Controller
    {
        private readonly TableStorageService _tables;

        public ProductsController(TableStorageService tables) => _tables = tables;

        // Display all products
        public async Task<IActionResult> Index() => View(await _tables.GetProductsAsync());

        // Deprecated: Remove if inline form is used
        [HttpGet]
        public IActionResult Create() => View(new ProductEntity());

        // Create new product from inline form
        [HttpPost]
        public async Task<IActionResult> Create(ProductEntity model)
        {
            if (string.IsNullOrWhiteSpace(model.Name) || model.Price <= 0)
                return RedirectToAction(nameof(Index));

            model.PartitionKey = "PRODUCT";
            model.RowKey = Guid.NewGuid().ToString();

            await _tables.AddProductAsync(model);

            return RedirectToAction(nameof(Index));
        }

        // Delete product
        [HttpPost]
        public async Task<IActionResult> Delete(string partitionKey, string rowKey)
        {
            if (string.IsNullOrWhiteSpace(partitionKey) || string.IsNullOrWhiteSpace(rowKey))
                return RedirectToAction(nameof(Index));

            await _tables.DeleteProductAsync(partitionKey, rowKey);

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
