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
        private readonly string connectionString;
        public MessageBusService(IConfiguration configuration)
        {
            _configuration = configuration;
            suggestionQueueName = _configuration["ServiceBus:SuggestionQueueName"];
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
                default:
                    break;
            }

            if (string.IsNullOrEmpty(queueName))
            {
                Console.WriteLine("Erro ao obter nome da fila");
                return;
            }

            try
            {
                var client = new QueueClient(connectionString, queueName);
                var messageBody = JsonSerializer.Serialize(message);
                var serielizedMessage = new Message(Encoding.UTF8.GetBytes(messageBody));

                await client.SendAsync(serielizedMessage);
                await client.CloseAsync();
                Console.WriteLine($"Mensagem enviada com sucesso para a fila {queueName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao enviar mensagem para a fila {queueName}: {ex.Message}");
            }
        }
    }
}