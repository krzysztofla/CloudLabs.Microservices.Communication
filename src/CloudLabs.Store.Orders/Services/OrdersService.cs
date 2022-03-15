using Azure.Messaging.ServiceBus;
using CloudLabs.Shared.Models;
using CloudLabs.Store.Orders.Processor;
using System.Text.Json;

namespace CloudLabs.Store.Orders.Services
{
    public class OrdersService : BackgroundService
    {
        private readonly ServiceBusClient _client;
        private readonly ServiceBusProcessor _ordersProcessor;
        private readonly ServiceBusSender _sender;

        public OrdersService(ServiceBusClient client)
        {
            _client = client;
            _ordersProcessor = _client.CreateProcessor("CloudLabs.Store.Orders");
            _sender = _client.CreateSender("CloudLabs.Store.Orders");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _ordersProcessor.ProcessMessageAsync += OrderMessageHandler;
            _ordersProcessor.ProcessErrorAsync += OrderErrorHandler;
            await _ordersProcessor.StartProcessingAsync();
        }

        async Task OrderMessageHandler(ProcessMessageEventArgs args)
        {
            var body = args.Message.Body.ToString();
            var message = JsonSerializer.Deserialize<CreateOrder>(body);

            await _sender.SendMessageAsync(new ServiceBusMessage(JsonSerializer.Serialize(new CreateOrderStatus("InProgress", message.Id, "Resolving dependencies"))));

            await _ordersProcessor.StopProcessingAsync();
        }

        Task OrderErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}
