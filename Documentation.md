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

*Health check for RabbitMQ (compose):* This ensures that RabbitMQ is running healthy before starting the other application, so that they don't crash while attempting to connect.

*Dependency Injection (DI) (server a+b):* This pattern is used by default in asp.net where services are register in a central place. DI is applying `Inversion of control` principle which is part of the S.O.L.I.D principles. The main benefit of DI is that it provide a central place in which all parts of the system are defined which is useful if we need to do integration testing with different configuration or mock services instead of the actual implementation. 

*State management Stores (frontend):* using Pinia

*Dead letter Queue:* A queue where bad messages are sent(messages that server b can't process). This is very important for real application, because bad letter could cripple a system.


### Additional Features

- Nice and responsive frontend UI with Bootstrap
- Loading indicator (frontend)
- Action notifications (frontend)
- RTC communication (server a - frontend) (SignalR library was used, which uses WebSocket by default)


### Technology Stack
#### Server A
For this server, asp.net 8 with C# was used, mainly because developer familiarity with it. Here some reasons for using dotnet:
- Great performance, C# is one of the best performing web frameworks. 
- Out-of-the-box support for many modern web application patterns. for example:
    - Support for Logging providers.
    - Dependency Injection pattern making applications easier to maintain. 
    - Support for OpenAPI specification documentation (swagger).
    - Tooling for making setting up authentication and authorization.
- Wide selection of community developed packages (just like NPM) like Rabbit MQ drivers, database drivers and so on.
- It is worth noting that modern asp.net 8 is cross-platform, cloud native and open source framework.
- Strongly types programming language offers the ability to catch error early.
- [Opinion] Strongly typed languages are easier to work with.


#### Server B
Dotnet 8 with C# will also be used, because the code will be similar to Server A and hence enhance productivity while the framework choice reasons is pretty similar.


#### Frontend
I will use vue.Js with TS for the frontend. The choice is easy as it has a lot of advantages compared to React and other frameworks:
- Light-weight and better performance.
- More flexible features with native extensions.
- [Opinion] More intuitive, less boilerplate code, easier to avoid mistakes.
- [Opinion] React is more popular than Vue.JS not because it is better, it is just the timing of the release.
 
### How to try the system
Ensure latest version of docker desktop is installed and run docker compose commands:

`docker-compose up -d`

