using System.Text.Json;
using Azure.Identity;
using Azure.Messaging.ServiceBus;

string topicName = "logtopic";
string nameSpaceHostName = "servicebusnamespace1071.servicebus.windows.net";

List<Ticket> tickets = new List<Ticket>();
/*tickets.Add(new Ticket { Id = 1, Name = "Computer", Status="inProgress", PriorityLevel=200});
tickets.Add(new Ticket { Id = 2,  Name = "Reports", Status="done", PriorityLevel=200});
tickets.Add(new Ticket { Id = 3, Name = "Sales", Status = "error", PriorityLevel = 20 });*/
tickets.Add(new Ticket { Id = 4, Name = "Hardware55555", Status = "error", PriorityLevel = 8 });

await SendMessages(tickets);


async Task SendMessages(List<Ticket> messages)
{
    var credential = new DefaultAzureCredential();
    ServiceBusClient serviceBusClient = new ServiceBusClient(nameSpaceHostName, credential);
    ServiceBusSender serviceBusSender = serviceBusClient.CreateSender(topicName);

    using ServiceBusMessageBatch serviceBusMessageBatch = await serviceBusSender.CreateMessageBatchAsync();
    {

        foreach (Ticket ticket in messages)
        {
            ServiceBusMessage serviceBusMessage = new ServiceBusMessage(JsonSerializer.Serialize<Ticket>(ticket));
            serviceBusMessage.ContentType = "application/json";
            serviceBusMessage.ApplicationProperties.Add("ticketStatus", ticket.Status);
            serviceBusMessage.ApplicationProperties.Add("eventPriority", ticket.PriorityLevel);
            serviceBusMessageBatch.TryAddMessage(serviceBusMessage);
        }
    }

    await serviceBusSender.SendMessagesAsync(serviceBusMessageBatch);
    Console.WriteLine("ALL MESSAGES SENT!!!");

    await serviceBusSender.DisposeAsync();
    await serviceBusClient.DisposeAsync();
}