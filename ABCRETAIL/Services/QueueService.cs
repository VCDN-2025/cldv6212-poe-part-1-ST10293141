using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ABCRETAIL.Services
{

    public class QueueMessageDto
    {
        public string MessageId { get; }
        public string PopReceipt { get; }
        public string MessageText { get; }

        public QueueMessageDto(string messageId, string popReceipt, string messageText)
        {
            MessageId = messageId;
            PopReceipt = popReceipt;
            MessageText = messageText;
        }
    }

    public class QueueService
    {
        private readonly QueueClient _queue;

        public QueueService(QueueServiceClient svc)
        {
            _queue = svc.GetQueueClient("ops-logs");
            _queue.CreateIfNotExists();
        }

        public Task EnqueueAsync(string message) => _queue.SendMessageAsync(message);

        // Now returning full message info for display + deletion
        public async Task<List<QueueMessageDto>> PeekAsync(int max = 10)
        {
            var messages = await _queue.ReceiveMessagesAsync(maxMessages: max);

            return messages.Value.Select(msg => new QueueMessageDto(
                       msg.MessageId,
                       msg.PopReceipt,
                       msg.MessageText
                   )).ToList();
        }

        public async Task DeleteMessageAsync(string messageId, string popReceipt)
        {
            await _queue.DeleteMessageAsync(messageId, popReceipt);
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
