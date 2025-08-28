using Azure.Data.Tables;
using Azure.Storage.Blobs;
using Azure.Storage.Queues;
using Azure.Storage.Files.Shares;
using ABCRETAIL.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Get the storage connection string from configuration
var storageConn = builder.Configuration.GetValue<string>("StorageConnection");

// Register Azure Storage clients
builder.Services.AddSingleton(new TableServiceClient(storageConn));
builder.Services.AddSingleton(new BlobServiceClient(storageConn));
builder.Services.AddSingleton(new QueueServiceClient(storageConn));
builder.Services.AddSingleton(new ShareServiceClient(storageConn));

// Register your custom services (scoped for DI)
builder.Services.AddScoped<TableStorageService>();
builder.Services.AddScoped<BlobStorageService>();
builder.Services.AddScoped<QueueService>();
builder.Services.AddScoped<FileShareService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization(); // Keep this if you're using authentication/authorization

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();




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
