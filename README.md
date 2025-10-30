🎟️ Ticket Event System – Azure Service Bus Demo

This project demonstrates an end-to-end event-driven system built with .NET 8 and Azure Functions, using Azure Service Bus Topics for reliable messaging and Azure AD (Managed/User Identity) for secure authentication — no connection strings required.

🏗️ Architecture
TicketSender (Console App)
    ↓  sends Ticket messages (JSON)
Azure Service Bus
 ├── Topic: logtopic
 └── Subscriptions:
        • ErrorSubscriber (used by Function)
TicketProcessor (Azure Function)
    ↑  triggered by messages on subscription

🧩 Components
Project	Type	Purpose
TicketSender	.NET 8 Console App	Publishes Ticket messages to Azure Service Bus Topic (logtopic).
TicketProcessor	.NET 8 Isolated Azure Function	Listens to messages from subscription (ErrorSubscriber) and logs their content.
🔒 Authentication

The system uses Azure Active Directory identities via DefaultAzureCredential().

Local Development → authenticates with your signed-in Azure user (az login)

Azure Deployment → authenticates using the Function App’s Managed Identity


⚙️ Local Setup

Sign in

az login


Grant RBAC

In the Service Bus namespace → Access control (IAM)

Role: Azure Service Bus Data Receiver → assign to your user

Run Function

cd TicketProcessor
func start


Send Test Messages

cd TicketSender
dotnet run


You’ll see the Function log each received message.

🚀 Deploying to Azure

Enable System-assigned Managed Identity on the Function App.

Assign Azure Service Bus Data Receiver role to that identity on the namespace.

In Function App Configuration → set

servicebusnamespace1071_SERVICEBUS__fullyQualifiedNamespace = servicebusnamespace1071.servicebus.windows.net


Redeploy (no code changes needed).

📁 Project Structure
ticketeventsystem/
├─ TicketSender/
│  ├─ Program.cs
│  └─ TicketSender.csproj
├─ TicketProcessor/
│  ├─ ServiceBusLogTopicTrigger.cs
│  ├─ host.json
│  └─ TicketProcessor.csproj
└─ ticketeventsystem.sln

🧰 Technologies

.NET 8

Azure Service Bus (Topics & Subscriptions)

Azure Functions (Isolated Process)

Azure Identity / Managed Identity

Visual Studio Code