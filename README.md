
# Potty Trainer

![Demo](/Resources/Skjermbilde%202022-08-24%20123637.png?raw=true)

This is a web app used to reenforce positive behavior. It consists of the following main parts:
- A Blazor WebAssembly app
- .Net API
- Azure Functions integration platform
- Azure Storage Tables
- Azure Storage Queues

Various sensors and activity platforms are integrated:
- Withings, including (but not limited to)
  - Body scale
  - BPM Core
- The Things Network, Custom LoRaWan devices
  - [Mailbox](https://community.element14.com/challenges-projects/project14/rf/b/blog/posts/got-mail-lorawan-mail-box-sensor)
  - [Soil moisture](https://community.element14.com/challenges-projects/project14/theholidayspecial19/b/blog/posts/deck-the-halls-with-holiday-flowers---and-keep-them-alive)
  - [Dosette (Pill box)](https://www.hackster.io/eivholt/microchip-digital-dosette-e6a8c8)
  - [Snow depth](https://www.hackster.io/eivholt/low-power-snow-depth-sensor-using-lora-e5-b8e7b8)

```mermaid
graph TD
    subgraph Azure
    style Azure fill:#DE5089

    subgraph Azure Static Web App
    StaticClient["Client (Blazor WebAssembly)"] --> WebAppAPI("API (Azure Functions)")
    end

    subgraph Azure Functions
    IntegrationAPI["Integration API"]
    QueueTrigger["Queue Trigger"]
    
    end
    AzureStorageTables[(Azure Storage Tables)]
    AzureStorageQueue[(Azure Storage Queue)]
    
    WebAppAPI --> AzureStorageTables
    IntegrationAPI <--> AzureStorageQueue
    IntegrationAPI --> AzureStorageTables
    AzureStorageQueue --> QueueTrigger
    QueueTrigger --> AzureStorageTables
    end

    subgraph TTN
    TTNMQTT --> TTNWebHook
    TTNWebHook --> IntegrationAPI
    end
    
    Dosette --> TTNMQTT
    GotMail --> TTNMQTT
    SnowMonitor --> TTNMQTT
    HousePlants --> TTNMQTT

    subgraph Withings
    WithingsNotify[Notify] --> IntegrationAPI
    StaticClient --> WithingsAuth[OAuth2] --> IntegrationAPI
    WithingsAPI[API] --> WithingsNotify
    IntegrationAPI <--> WithingsAPI
    IntegrationAPI --> WithingsSubscribe[Subscribe]
    WithingsSubscribe --> WithingsNotify
    end

    WithingsBPM[BPM] --> WithingsAPI
    WithingsCore[Weight] --> WithingsAPI

    subgraph GitHub
    style GitHub fill:#7F9E31
    Repo([Code Repository]) -.-> WebAppBuildAction[[Web App Build Action]]
    WebAppBuildAction -.-> StaticClient
    WebAppBuildAction -.-> WebAppAPI
    Repo -.-> IntegrationAPIBuildAction[[Integration API Build Action]]
    IntegrationAPIBuildAction -.-> IntegrationAPI
    end
```

Follow the hands-on tutorial to [publish a Blazor WebAssembly app and .NET API with Azure Static Web Apps](https://docs.microsoft.com/learn/modules/publish-app-service-static-web-app-api-dotnet/?WT.mc_id=mslearn_staticwebapp-github-aapowell).

![Main page](/Resources/Screenshot_20220817-234614_Chrome.png?raw=true)
![Choose assignment](/Resources/Screenshot_20220811-091332_Chrome.png?raw=true)
