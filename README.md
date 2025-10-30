## Ticket Event System – Azure Service Bus Integration

# 1. Overview

The Ticket Event System demonstrates an event-driven architecture using .NET 8, Azure Service Bus Topics, and Azure Functions.
It simulates a ticketing workflow where messages are published to a Service Bus topic and processed selectively based on their properties.
All authentication is handled through Azure Active Directory (Azure AD) instead of connection strings.

# 2. Architecture
Components

TicketSender – .NET 8 console app that publishes Ticket messages to an Azure Service Bus topic named logtopic.

TicketProcessor – .NET 8 isolated Azure Function triggered by filtered messages from a topic subscription.

Topic & Subscriptions

Topic: logtopic

Subscriptions:

GeneralSubscriber – receives all messages.

ErrorSubscriber – receives only messages where:

ticketStatus = "error"

eventPriority < 10

The Azure Function listens only to ErrorSubscriber.

Message Flow
TicketSender  →  Service Bus Topic (logtopic)
                   ├─ GeneralSubscriber (no filter)
                   └─ ErrorSubscriber (filtered)
                                ↓
                        TicketProcessor (Function)

# 3. Authentication

Local Development: Uses the signed-in Azure user (az login).

Azure Deployment: Uses the Function App’s system-assigned managed identity.

No connection strings are stored in code or configuration.

# 4. Local Setup
Requirements

Azure subscription

Service Bus namespace

.NET 8 SDK

Azure CLI and Functions Core Tools

Steps

Sign in to Azure:

az login


Assign Azure Service Bus Data Receiver role to your Azure user on the Service Bus namespace.

Update TicketProcessor/local.settings.json:

{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
    "servicebusnamespace1071_SERVICEBUS__fullyQualifiedNamespace": "servicebusnamespace1071.servicebus.windows.net"
  }
}


Run the Function:

cd TicketProcessor
func start


Send test messages:

cd TicketSender
dotnet run


Only error tickets with low priority will trigger the Function.

# 5. Deployment to Azure

Enable System-Assigned Managed Identity on the Function App.

Assign Azure Service Bus Data Receiver role to that identity on the Service Bus namespace.

Add the following App Setting:

servicebusnamespace1071_SERVICEBUS__fullyQualifiedNamespace = servicebusnamespace1071.servicebus.windows.net


Redeploy — no code changes required.

# 6. Technologies

.NET 8

Azure Service Bus (Topics & Subscriptions)

Azure Functions (Isolated Worker)

Azure Identity (Managed Identity / AAD)

Visual Studio Code

Azure CLI & Functions Core Tools

# 7. Author

Ozan Onder