using Azure.Messaging.ServiceBus;
using CloudLabs.Shared.Models;
using System.Text.Json;

namespace CloudLabs.Bank.Funds.Services
{
    public class FundsService : BackgroundService
    {
        private readonly ServiceBusClient _client;
        private readonly ServiceBusProcessor _processor;
        private readonly ServiceBusSender _sender;
        public FundsService(ServiceBusClient client)
        {
            _client = client;
            _processor = _client.CreateProcessor("CloudLabs.Bank.Funds");
            _sender = _client.CreateSender("CloudLabs.Bank.Orders"); 
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _processor.ProcessMessageAsync += MessageHandler;
            _processor.ProcessErrorAsync += ErrorHandler;
            await _processor.StartProcessingAsync();
        }

        async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            var message = JsonSerializer.Deserialize<CreateOrder>(body);

            var canBuy = CheckIfUserHasFunds(message);
            
            await _sender.SendMessageAsync(new ServiceBusMessage(JsonSerializer.Serialize(new CreateOrderStatus(canBuy.Item1, message.Id, canBuy.Item2))));

            await args.CompleteMessageAsync(args.Message);
            await _processor.StopProcessingAsync();
        }

        Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        private Tuple<bool, string> CheckIfUserHasFunds(CreateOrder? message)
        {
            if (message == null) return Tuple.Create(false, "message processing exception");

            var mockedUserFunds = 1500;
            if (message.ItemsCost > mockedUserFunds)
            {
                return new Tuple<bool, string>(false, "not enough money");
            }
            return new Tuple<bool, string>(true, "order placed");
        }
    }
}
