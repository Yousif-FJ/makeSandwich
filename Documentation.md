### Course information 
Author information:
- Student Name: Yousif Al-Baghdadi
- Student Number: 152106946
- Student Email: yousif.al-baghdadi@tuni.fi 

Group Information (single person group):
- Group Name: oneteam
- GitLab repo URL: https://course-gitlab.tuni.fi/compcs510-spring2024/oneteam


### Architecture
The architecture and component functions will be the same as the architecture given in the assignment specs.

Here is diagram describing the overall architecture:
![sandwich maker diagram](other-files/sandwich-maker-diagram.png)
[Figma view link](https://www.figma.com/file/rZEwcRM8uRmPTwKIJSSEDr/sandwich-maker-diagram?type=whiteboard&node-id=0%3A1&t=X3tzOmvYt47fWMCK-1)

### Patterns used

**Health check for RabbitMQ (compose):** This ensures that RabbitMQ is running healthy before starting the other application, so that they don't crash while attempting to connect.

**Dependency Injection (DI) (server a+b):** This pattern is used by default in asp.net where services are register in a central place. DI is applying `Inversion of control` principle which is part of the S.O.L.I.D principles. The main benefit of DI is that it provide a central place in which all parts of the system are defined which is useful if we need to do integration testing with different configuration or mock services instead of the actual implementation. 

**State management Stores (frontend):** using Pinia. This pattern is very useful to ensure that component are only concerned with the actual data rather than state.

**Dead letter Queue (server b):** A queue where bad messages are sent(messages that server b can't process). This is very important for real application, because bad letter could cripple a system.


### Additional Features

- Nice and responsive frontend UI with Bootstrap
- Loading indicator (frontend)
- Action notifications (frontend)
- RTC communication (server a - frontend) (SignalR library was used, which uses WebSocket by default)


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
    - Tooling for making setting up authentication and authorization.
- Wide selection of community developed packages (just like NPM) like Rabbit MQ drivers, database drivers and so on.
- It is worth noting that modern asp.net 8 is cross-platform, cloud native and open source framework.
- Strongly types programming language offers the ability to catch error early.

##### Other libraries
- **AspNetCore.OpenApi**: Libraries that facilitate work with open API specs.
- **Swashbuckle.AspNetCore**: ASP.NET library for swagger UI.
- **RabbitMQ.Client**: Official RabbitMQ client.
- **SignalR**: Built-in with ASP.NET, allow for easy real time communication using Websocket or other methods.



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


### How to try the system
Ensure latest version of docker desktop is installed and run docker compose commands:

`docker-compose up -d`

- access the frontend on http://localhost:12346/
- access backend swagger API page on http://localhost:12345/
- access RabbitMQ management page on http://localhost:15672/#/
    - user: guest, password: guest

The apps have some basic logging which can be accessed on docker logs.
