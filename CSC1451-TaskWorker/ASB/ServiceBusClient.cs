using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CSC1451_TaskWorker.Handlers;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;

namespace CSC1451_TaskWorker.ASB
{
    public class ServiceBusClient
    {
        private readonly IServiceProvider _provider;
        private readonly Settings.Settings _settings;
        private IQueueClient _queueClient;

        public ServiceBusClient(IServiceProvider provider, Settings.Settings settings)
        {
            _provider = provider;
            _settings = settings;
        }

        public async Task<IQueueClient> StartAsync()
        {
            string endpointName;
#if DEBUG
            endpointName = $"Tak.TaskWorker.{Environment.MachineName}";
#else
            endpointName = "Tak.TaskWorker";
#endif

            await CreateQueueIfNotExist(_settings.ServiceBus, endpointName);

            _queueClient = new QueueClient(_settings.ServiceBus, endpointName);

            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };
            // Register the function that processes messages.
            _queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);

            return _queueClient;
        }

        private async Task CreateQueueIfNotExist(string connectionString, string endpointName)
        {
            var managementClient = new ManagementClient(connectionString);

            if (!await managementClient.QueueExistsAsync(endpointName))
            {

                var queue = await managementClient.CreateQueueAsync(new QueueDescription(endpointName));

            }
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            var type = Type.GetType($"CSC1451_TaskWorker.Handlers.{message.Label}Handler, CSC1451-TaskWorker");
            var handler = (IMessageHandler)_provider.GetService(type);

            var body = Encoding.ASCII.GetString(message.Body);

            await handler.Handle(body);

            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }

        public async Task StopAsync()
        {
            if (!_queueClient.IsClosedOrClosing)
            {
                await _queueClient.CloseAsync();
            }
        }
    }
}
