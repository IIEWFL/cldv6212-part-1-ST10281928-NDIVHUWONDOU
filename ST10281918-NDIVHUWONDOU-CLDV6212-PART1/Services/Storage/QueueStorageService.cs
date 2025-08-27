using Azure.Storage.Queues;
using System.Text.Json;

namespace ST10281918_NDIVHUWONDOU_CLDV6212_PART1.Services.Storage
{
    public class QueueStorageService
    {
        private readonly QueueClient _queueClient;

        public QueueStorageService(string storageAccount, string queueName)
        {
            var serviceClient = new QueueServiceClient(storageAccount);
            _queueClient = serviceClient.GetQueueClient(queueName);
            _queueClient.CreateIfNotExists();
        }
        public async Task SendMessagesAsync(object message)
        {
            var messageJson = JsonSerializer.Serialize(message);
            await _queueClient.SendMessageAsync(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(messageJson)));
        }
    }
}
