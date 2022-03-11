using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;

namespace CloudLabs.Shared.Extensions
{
    public static class Messaging
    {
        static string connectionString = "";

        static string queueName = "";

        public static IServiceCollection RegisterServiceBus(this IServiceCollection services)
        {
            var client = new ServiceBusClient(connectionString);
            var sender = client.CreateSender(queueName);
            var processor = client.CreateProcessor(queueName, new ServiceBusProcessorOptions());

            services.AddSingleton(client);
            services.AddSingleton(sender);
            services.AddSingleton(processor);

            return services;
        }
    }
}
