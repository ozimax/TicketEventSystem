Ticket Event System – Azure Service Bus Integration
1. Introduction

The Ticket Event System demonstrates a distributed, event-driven architecture built with .NET 8, Azure Service Bus Topics, and Azure Functions.
It simulates a ticketing workflow where events are published by one service and selectively processed by another, based on message properties.

The system emphasizes:

Secure, connectionless authentication using Azure Active Directory (Azure AD).

Message filtering through Service Bus Topic Subscriptions.

Separation of responsibilities between publishers and subscribers.

2. System Architecture
Components Overview
Component	Type	Description
TicketSender	.NET 8 Console Application	Publishes Ticket messages to an Azure Service Bus topic named logtopic.
TicketProcessor	.NET 8 Isolated Azure Function	Listens to filtered messages from the ErrorSubscriber subscription and processes them.
Message Routing Design

The Azure Service Bus topic logtopic has two subscriptions:

Subscription	Description	Filter Criteria
GeneralSubscriber	Receives all messages published to the topic.	None
ErrorSubscriber	Receives only messages where the ticket has an error state.	ticketStatus = "error" and eventPriority < 10
Logical Flow
TicketSender (Console Application)
        │
        ▼
Azure Service Bus Namespace
   ├── Topic: logtopic
   │    ├── GeneralSubscriber  (no filters)
   │    └── ErrorSubscriber    (filtered: status="error", priority<10)
        │
        ▼
TicketProcessor (Azure Function)
   - Triggered by ErrorSubscriber
   - Processes and logs relevant messages

3. Authentication and Security

This project uses Azure AD–based authentication via DefaultAzureCredential from the Azure Identity library.
Connection strings are not used or stored anywhere.

Environment	Authentication Method
Local Development	Authenticates using the signed-in Azure user from az login.
Azure Deployment	Authenticates using the Function App’s system-assigned managed identity.
4. Local Development Setup
Prerequisites

Azure subscription

Azure Service Bus namespace

.NET 8 SDK

Azure CLI (az)

Azure Functions Core Tools

Visual Studio Code

Configuration Steps

Sign in to Azure

az login


Assign RBAC Role

Navigate to your Service Bus namespace in the Azure portal.

Go to Access control (IAM) → Add role assignment.

Role: Azure Service Bus Data Receiver

Assign to: your signed-in Azure user.

Update Local Function Settings

TicketProcessor/local.settings.json

{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "servicebusnamespace1071_SERVICEBUS__fullyQualifiedNamespace": "servicebusnamespace1071.servicebus.windows.net"
  }
}


Run the Function Locally

cd TicketProcessor
func start


Run the Message Publisher

cd TicketSender
dotnet run


Messages with Status = "error" and PriorityLevel < 10 will trigger the function through the ErrorSubscriber subscription.

5. Deployment to Azure

Enable Managed Identity

Portal → Function App → Identity → System-assigned = On.

Grant Permissions

Portal → Service Bus Namespace → Access control (IAM).

Add role assignment → Azure Service Bus Data Receiver → assign to the Function App’s managed identity.

Configure Application Setting

servicebusnamespace1071_SERVICEBUS__fullyQualifiedNamespace = servicebusnamespace1071.servicebus.windows.net


Redeploy the Function App
The same code runs in Azure without modification.

6. Project Structure
ticketeventsystem/
│
├── TicketSender/
│   ├── Program.cs
│   ├── Ticket.cs
│   └── TicketSender.csproj
│
├── TicketProcessor/
│   ├── ServiceBusLogTopicTrigger.cs
│   ├── host.json
│   ├── local.settings.json
│   └── TicketProcessor.csproj
│
└── ticketeventsystem.sln

7. Technologies Used

.NET 8

Azure Service Bus (Topics and Subscriptions)

Azure Functions (Isolated Worker Model)

Azure Identity (Managed Identity & User Authentication)

Azure CLI and Functions Core Tools

Visual Studio Code

8. Author

Ozan Onder