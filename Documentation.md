### Architecture
The architecture and component functions will be the same as the architecture given in the assignment specs.



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
Simply running docker compose file from the root folder should be enough.

`docker-compose up -d`

