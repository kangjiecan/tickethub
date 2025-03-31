using Azure.Storage.Queues;
using System.Text.Json;
using TicketHubApi.Models;

namespace TicketHubApi.Services
{
    public class QueueService
    {
        private readonly QueueClient _queueClient;

        public QueueService(string connectionString, string queueName)
        {
            // Create a QueueClient instance
            _queueClient = new QueueClient(connectionString, queueName);

            // Create the queue if it doesn't exist
            _queueClient.CreateIfNotExists();
        }

        public async Task SendMessageAsync(TicketPurchase ticketPurchase)
        {
            // Serialize the ticket purchase object to JSON
            string messageJson = JsonSerializer.Serialize(ticketPurchase);

            // Send the message to the queue
            await _queueClient.SendMessageAsync(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(messageJson)));
        }
    }
}