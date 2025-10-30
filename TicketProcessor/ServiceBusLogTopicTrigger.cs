using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Company.Function;

public class ServiceBusLogTopicTrigger
{
    private readonly ILogger<ServiceBusLogTopicTrigger> _logger;

    public ServiceBusLogTopicTrigger(ILogger<ServiceBusLogTopicTrigger> logger)
    {
        _logger = logger;
    }

    [Function(nameof(ServiceBusLogTopicTrigger))]
    public async Task Run([ServiceBusTrigger("logtopic", "ErrorSubscriber", Connection = "servicebusnamespace1071_SERVICEBUS")] ServiceBusReceivedMessage message, ServiceBusMessageActions messageActions)
    {
        Console.WriteLine($"Message Body: {message.Body}");

        await messageActions.CompleteMessageAsync(message);
    }
}