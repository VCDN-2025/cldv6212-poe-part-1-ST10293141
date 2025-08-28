using Azure.Data.Tables;
using ABCRETAIL.Models;

namespace ABCRETAIL.Services
{
    public class TableStorageService
    {
        private readonly TableClient _customers;
        private readonly TableClient _products;

        public TableStorageService(TableServiceClient svc)
        {
            _customers = svc.GetTableClient("Customers");
            _products = svc.GetTableClient("Products");
            _customers.CreateIfNotExists();
            _products.CreateIfNotExists();
        }

        
        // CUSTOMER OPERATIONS
       

        public async Task<List<CustomerEntity>> GetCustomersAsync()
        {
            var list = new List<CustomerEntity>();
            await foreach (var c in _customers.QueryAsync<CustomerEntity>())
                list.Add(c);
            return list;
        }

        public Task AddCustomerAsync(CustomerEntity entity) =>
            _customers.AddEntityAsync(entity);

        public Task DeleteCustomerAsync(string partitionKey, string rowKey) =>
            _customers.DeleteEntityAsync(partitionKey, rowKey);

        
        // PRODUCT OPERATIONS
       

        public async Task<List<ProductEntity>> GetProductsAsync()
        {
            var list = new List<ProductEntity>();
            await foreach (var p in _products.QueryAsync<ProductEntity>())
                list.Add(p);
            return list;
        }

        public Task AddProductAsync(ProductEntity entity) =>
            _products.AddEntityAsync(entity);

        public Task DeleteProductAsync(string partitionKey, string rowKey) =>
            _products.DeleteEntityAsync(partitionKey, rowKey);
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
