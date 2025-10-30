This repository demonstrates an end-to-end event-driven architecture built with .NET 8, Azure Service Bus Topics, and Azure Functions.
The solution showcases how to publish and process messages securely using Azure Active Directory (Azure AD) authentication instead of connection strings.

Overview

The system consists of two main components:

TicketSender (Console Application) – Publishes Ticket objects to an Azure Service Bus Topic named logtopic.

TicketProcessor (Azure Function) – Listens for specific messages on a Service Bus Topic Subscription and processes them.

The topic logtopic contains two subscriptions:

GeneralSubscriber – receives all messages.

ErrorSubscriber – applies a filter to receive only messages where:

ticketStatus equals "error", and

eventPriority is less than 10.

The Azure Function is configured to listen exclusively to the ErrorSubscriber subscription.

Architecture
TicketSender (Console Application)
       |
       v
Azure Service Bus (Topic: logtopic)
   ├── GeneralSubscriber  → receives all messages
   └── ErrorSubscriber    → filtered (status = "error", priority < 10)
       |
       v
TicketProcessor (Azure Function)

Authentication and Security

This project uses Azure Active Directory authentication via DefaultAzureCredential from the Azure Identity library.

Local development uses the signed-in Azure user identity (az login).

Azure deployment uses the Function App’s system-assigned managed identity.

No connection strings or secrets are stored in code or configuration files.

Local Development
Prerequisites

Azure subscription

.NET 8 SDK

Azure CLI (az)

Azure Functions Core Tools

Access to an Azure Service Bus namespace

Steps

Sign in to Azure:

az login


Assign the Azure Service Bus Data Receiver role to your user account on the Service Bus namespace.

Update local.settings.json in the TicketProcessor project:

{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "servicebusnamespace1071_SERVICEBUS__fullyQualifiedNamespace": "servicebusnamespace1071.servicebus.windows.net"
  }
}


Run the Azure Function locally:

cd TicketProcessor
func start


In another terminal, run the console sender:

cd TicketSender
dotnet run


Messages meeting the filter conditions (status = error and priority < 10) will trigger the function.

Deployment to Azure

Enable System-assigned Managed Identity for the Function App.

Assign the Azure Service Bus Data Receiver role to that managed identity on the Service Bus namespace.

In the Function App configuration, add the following setting:

servicebusnamespace1071_SERVICEBUS__fullyQualifiedNamespace = servicebusnamespace1071.servicebus.windows.net


Deploy the Function App and Console application as required.

Project Structure
ticketeventsystem/
│
├── TicketSender/
│   ├── Program.cs
│   ├── TicketSender.csproj
│   └── Ticket.cs
│
├── TicketProcessor/
│   ├── ServiceBusLogTopicTrigger.cs
│   ├── TicketProcessor.csproj
│   ├── host.json
│   └── local.settings.json
│
└── ticketeventsystem.sln

Technologies Used

.NET 8

Azure Service Bus (Topics and Subscriptions)

Azure Functions (Isolated Process)

Azure Identity (Managed Identity / AAD authentication)

Azure CLI and Functions Core Tools

Visual Studio Code

Author

Ozan Onder
GitHub: ozimax