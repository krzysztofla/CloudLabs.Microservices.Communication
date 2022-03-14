using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.DependencyInjection;

namespace CloudLabs.Shared.Extensions
{
    public static class Messaging
    {
        static string connectionString = "";

        public static IServiceCollection RegisterServiceBus(this IServiceCollection services)
        {
            var client = new ServiceBusClient(connectionString);

            services.AddSingleton(client);

            return services;
        }
    }
}
