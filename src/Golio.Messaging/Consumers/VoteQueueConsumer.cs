using System.Text;
using System.Text.Json;
using Golio.Application.Commands.CreateProduct.CreateUser;
using Golio.Application.Commands.CreateProduct.SendSuggestion;
using Golio.Core.DTOs;
using Golio.Core.Entities;
using Golio.Core.Repositories;
using Golio.Infrastructure.CacheService;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Golio.Messaging.Consumers
{
    public class VoteQueueConsumer : IHostedService
    {
        private readonly IConfiguration _configuration;
        private IQueueClient queueClient;
        private readonly string? connectionString;
        private readonly string? priceQueueName;
        private readonly IPriceRepository _priceRepository;
        private readonly ISuggestionRepository _suggestionRepository;
        private readonly ICacheService _cacheService;

        public VoteQueueConsumer(IConfiguration configuration, IPriceRepository priceRepository, ICacheService cacheService, ISuggestionRepository suggestionRepository)
        {
            _configuration = configuration;
            priceQueueName = _configuration["ServiceBus:ReceivedVoteQueueName"];
            connectionString = _configuration["ServiceBus:ConnectioString"];
            queueClient = new QueueClient(connectionString, priceQueueName);

            _priceRepository = priceRepository;
            _suggestionRepository = suggestionRepository;
            _cacheService = cacheService;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Starting Consumer from the votes queue");
            ProcessMessageHandler();
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Finishing Consumer from the votes queue");
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
            Console.WriteLine($"Processing message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");

            var suggestionVoteDTO = JsonSerializer.Deserialize<SuggestionVoteDTO>(message.Body);
            if (suggestionVoteDTO!.IsValid)
            {
                var updatePriceCommand = new UpdatePriceCommand()
                {
                    PriceId = suggestionVoteDTO!.PriceId,
                    Value = suggestionVoteDTO.Value
                };
                var updatePriceCommandHandler = new UpdatePriceCommandHandler(_priceRepository, _cacheService);
                await updatePriceCommandHandler.Handle(updatePriceCommand, new CancellationToken());
            }
            else
            {
                var updatePriceCommand = new RemoveSuggestionCommand()
                {
                    SuggestionId = suggestionVoteDTO!.SuggestionId,
                };
                var updatePriceCommandHandler = new RemoveSuggestionCommandHandler(_suggestionRepository, _cacheService);
                await updatePriceCommandHandler.Handle(updatePriceCommand, new CancellationToken());
            }


            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Error processing event: {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine($"Endpoint: {context.Endpoint}");
            Console.WriteLine($"Entity Path: {context.EntityPath}");
            Console.WriteLine($"Executing Action: {context.Action}");
            return Task.CompletedTask;
        }
    }
}