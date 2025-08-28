using ABCRETAIL.Models;
using ABCRETAIL.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRETAIL.Controllers
{
    public class CustomersController : Controller
    {
        private readonly TableStorageService _tables;

        public CustomersController(TableStorageService tables) => _tables = tables;

        // Display all customers and the add form
        public async Task<IActionResult> Index()
        {
            var customers = await _tables.GetCustomersAsync();
            return View(customers);
        }

        // Handle form submission from Index page
        [HttpPost]
        public async Task<IActionResult> Create(CustomerEntity model)
        {
            if (!ModelState.IsValid)
            {
                var customers = await _tables.GetCustomersAsync();
                return View("Index", customers);
            }

            await _tables.AddCustomerAsync(model);
            return RedirectToAction(nameof(Index));
        }

        // Handle delete request
        [HttpPost]
        public async Task<IActionResult> Delete(string partitionKey, string rowKey)
        {
            if (!string.IsNullOrEmpty(partitionKey) && !string.IsNullOrEmpty(rowKey))
            {
                await _tables.DeleteCustomerAsync(partitionKey, rowKey);
            }

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
