using System.Text;
using System.Text.Json;
using Golio.Application.Commands.CreateProduct.CreateUser;
using Golio.Core.DTOs;
using Golio.Core.Entities;
using Golio.Core.Repositories;
using Golio.Infrastructure.CacheService;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Golio.Messaging.Consumers
{
    public class SuggestionQueueConsumer : IHostedService
    {
        private readonly IConfiguration _configuration;
        private IQueueClient queueClient;
        private readonly string? connectionString;
        private readonly string? suggestionQueueName;
        private readonly ISuggestionRepository _suggestionRepository;
        private readonly ICacheService _cacheService;

        public SuggestionQueueConsumer(IConfiguration configuration, ISuggestionRepository suggestionRepository, ICacheService cacheService)
        {
            _configuration = configuration;
            suggestionQueueName = _configuration["ServiceBus:SuggestionQueueName"];
            connectionString = _configuration["ServiceBus:ConnectioString"];
            queueClient = new QueueClient(connectionString, suggestionQueueName);

            _suggestionRepository = suggestionRepository;
            _cacheService = cacheService;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Iniciando Consumer da fila de Sugestões");
            ProcessMessageHandler();
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Finalizando Consumer da fila de Sugestões");
            await queueClient.CloseAsync();
            await Task.CompletedTask;
        }

        private void ProcessMessageHandler()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            Console.WriteLine($"Processando mensagem: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");

            var suggestionDTO = JsonSerializer.Deserialize<SuggestionDTO>(message.Body);

            var createSuggestionCommand = new CreateSuggestionCommand()
            {
                AutorName = suggestionDTO!.AutorName,
                AutorEmail = suggestionDTO!.AutorEmail,
                PriceId = suggestionDTO.PriceId,
                Value = suggestionDTO.Value
            };
            var createSuggestionCommandHandler = new CreateSuggestionCommandHandler(_suggestionRepository, _cacheService);
            await createSuggestionCommandHandler.Handle(createSuggestionCommand, new CancellationToken());

            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Erro ao processar evento: {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine($"Endpoint: {context.Endpoint}");
            Console.WriteLine($"Entity Path: {context.EntityPath}");
            Console.WriteLine($"Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
    }
}