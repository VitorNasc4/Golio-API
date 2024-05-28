using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Golio.Core.DTOs;
using Golio.Core.Services;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;

namespace Golio.Infrastructure.MessageBusService
{
    public class MessageBusService : IMessageBusService
    {
        private readonly IConfiguration _configuration;
        private readonly string suggestionQueueName;
        private readonly string suggestionVoteQueueName;
        private readonly string connectionString;
        public MessageBusService(IConfiguration configuration)
        {
            _configuration = configuration;
            suggestionQueueName = _configuration["ServiceBus:SuggestionQueueName"];
            suggestionVoteQueueName = _configuration["ServiceBus:VotesQueueName"];
            connectionString = _configuration["ServiceBus:ConnectioString"];
        }
        public async Task SendMessageQueueAsync<T>(T message)
        {
            string queueName = null;
            switch (typeof(T))
            {
                case Type when typeof(T) == typeof(SuggestionDTO):
                    queueName = suggestionQueueName;
                    break;
                case Type when typeof(T) == typeof(SuggestionVoteMessage):
                    queueName = suggestionVoteQueueName;
                    break;
                default:
                    break;
            }

            if (string.IsNullOrEmpty(queueName))
            {
                Console.WriteLine("Error retrieving queue name");
                return;
            }

            try
            {
                var client = new QueueClient(connectionString, queueName);
                var messageBody = JsonSerializer.Serialize(message);
                var serielizedMessage = new Message(Encoding.UTF8.GetBytes(messageBody));

                await client.SendAsync(serielizedMessage);
                await client.CloseAsync();
                Console.WriteLine($"Message successfully sent to the queue {queueName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending message to the queue {queueName}: {ex.Message}");
            }
        }
    }
}