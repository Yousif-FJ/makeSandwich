### Introduction
Make a sandwich is a system with a frontend and a distributed backend. The system enables end users to order sandwiches and track the state of their orders.

### Technology Stack
#### Server A
ASP.NET (Core) 8 with C#.

Using Controller Web API template.

Reasons for using the technology:
- The developer is familiar with the framework.
- Great performance, C# is one of the best performing web frameworks. 
- Out-of-the-box support for many modern web application patterns. for example:
    - Support for Logging providers.
    - Dependency Injection pattern making applications easier to maintain. 
    - Support for OpenAPI specification documentation (swagger).
    - Tooling for setting up authentication and authorization.
- Wide selection of community developed packages (just like NPM) like Rabbit MQ drivers, database drivers and so on.
- It is worth noting that modern asp.net 8 is cross-platform, cloud native and open source framework.
- Strongly types programming language offers the ability to catch error early.

##### Other libraries
- **AspNetCore.OpenApi**: Libraries that facilitate work with open API specs.
- **Swashbuckle.AspNetCore**: ASP.NET library for swagger UI.
- **RabbitMQ.Client**: Official RabbitMQ client.
- **SignalR**: Built-in with ASP.NET, allow for easy real time communication using Websocket or other methods.
- **Microsoft.AspNetCore.Identity.EntityFrameworkCore**: for Authentication and user store.
- **Microsoft.EntityFrameworkCore.SqlServer**: Database ORM for MS SQL Server.


#### Server B
Dotnet 8 service worker C#.

Using service worker template. Which is like a console app but with IHost for long running service which includes tooling for DI and other services like configuration, logging, etc..

Reasons for using the technology:
- Same reasons as server A

##### Other libraries
- **RabbitMQ.Client**: Official RabbitMQ client.


#### Frontend
vue.Js with TS for the frontend. 

Reasons for using the technology::
- Light-weight and good performance.
- More flexible features with native extensions.
 
##### Other libraries
- **Vue-Router**: Multi page app.
- **Bootstrap**: css framework.
- **Pinia**: State Management.
- **SignalR**: Real-time communication with the backend.
- **axios**: Making HTTP requests.

#### Database
The app use MS SQL server database. Other databases could be used but this one was chosen because the developer is familiar with it.

The database is only used for storing users in the system.

### How to try the system
Ensure latest version of docker desktop is installed and running then run docker compose commands:

`docker-compose up -d`

- access the frontend on http://localhost:12346/
    - Email : admin@localhost, password: admin123
- access backend swagger API page on http://localhost:12345/
- access RabbitMQ management page on http://localhost:15672/#/
    - user: guest, password: guest
- access SQL server using Server Management Studio on localhost (default port 1433)
    - user: sa, password: localHostPassword123

The apps have some basic logging which can be accessed on docker logs.


### Architecture
Here is diagram describing the overall architecture:
![sandwich maker diagram](other-files/sandwich-maker-diagram.png)
[Figma view link](https://www.figma.com/file/rZEwcRM8uRmPTwKIJSSEDr/sandwich-maker-diagram?type=whiteboard&node-id=0%3A1&t=X3tzOmvYt47fWMCK-1)

We go over each component in the system:
#### Server A 
Acts as the main server in our system which server the API. 

Here some things to note about this server:
- The app **entry point** is on program.cs
- The app is using **dependency injection** where the services are being registers in program.cs
- The **Request pipeline** is also configured in program.cs  
- The app use controller attribute **routing**: 
    - Controller are named API in the code
    - The routes are defined as attributes on each endpoint in the APIs
- The app is hosting **Swagger UI** on root directory
- The app is hosting **real time communication hub**:
    - SignalR is being used.
    - SignalR uses WebSocket by default and may fall back to other methods if web socket not available.
    - The purpose of real time communication is to notify the frontend about order status changes.
- The app is hosting a "**hosted service**", things to note about dotnet hosted services:
    - Hosted services are singleton objects that are held in the app host
    - Hosted services trigger start function on the application startup 
    - Hosted services trigger stop function on application shutdown
    - Hosted services support dependency injection 
- The app is hosting `OrderStatusUpdater`, which do the following:
    - Subscribe to `orderStatusUpdates` Queue.
    - Update the state of orders
    - Publish real time update notification on the Real-time communication hub.
- The app implement CRUD operation on Sandwich API:
    - Write operation require login. 
    - The sandwiches information is stored in memory collections.
- The app is implementing authentication and authorization:
    - The auth system is using Microsoft Identity library 
    - The store users in MS SQL Server database
    - The authentication is using cookie authentication with session


#### Server B
Act as order processor and emulate making sandwiches.
- The app **entry point** is on program.cs
- The app is using **dependency injection** where the services are being registers in program.cs
- The app is hosting a **hosted service** (more about that was mentioned on server A)
- The app is hosting `OrderProcessor` hosted service, which do the following:
    - Subscribe to orders from `orders` queue
    - Publish order received message on `orderStatusUpdates`
    - Publish order read after 5s delay on `orderStatusUpdates`

#### Rabbit MQ
RabbitMQ was used for asynchronous communication between server A and server B; The Queue system is very simple in this case as direct exchanges are used, which binds on a single queue; Therefore we can say that, Server A publish orders on `orders` queue and then server B updates server A about the order status on `orderStatusUpdates` queue.  

One important feature of the system is the dead letter queue. Server A could send message to which server B can handle; This could jam the system as server B get stuck on trying to process a bad message. To avoid that, message that have bad format are sent to dead letter queue, which then needs manual investigation from the developer.

#### Frontend
The frontend is following vue standard architecture. 

Here are some things to note:
- The app is using Pinia for **state management** 
- The app uses axios to make **HTTP requests** from the state store.
- The app is also using **vue router** to route to different views like Orders page.
- The app is using "**bootstrap**" for **css**
- The app has **`notification`** component and store, which do the following:
    - Show message to the user when successful requests are made.
    - Show red error message when request fail.
    - show loading indicator when waiting for response from the API
    - Note: best way to test this, is to shut down the main server and try to use the app 
- The app using **real-time communication**:
    - The app is utilizing SignalR client library.  
    - The app connection to Hub when entering Orders page.
    - The app fetches the orders again when it receives order update signal.
- The app has auth implementation and integration with the backend


### Summary for base features
- Docker Compose hosting/deployment 
- Server to server communication with RabbitMQ
- SPA app
- API CRUD


### Summary for Additional Features and Technologies
- Health check for RabbitMQ (Compose) (ensure RabbitMQ is working before starting server A and B)
- State Management Store with Pinia (frontend) 
- Dead Letter Queue (server B)
- Nice and responsive frontend UI with Bootstrap
- Production grade frontend Dockerfile (with Nginx)
- Loading indicator (frontend)
- Action notifications (frontend)
- Login (Authentication)
- Require authentication for Create sandwich (Full CRUD API only because no time)
- RTC communication (server A - frontend) 