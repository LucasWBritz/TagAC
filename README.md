# Tag AC - Tag Access Control
A TAG Access Control system build using microservice architecture. 

### Project structure

    .
    ├── ...
    ├── 01 Api Gateways                    # Basic Api Gateway implementation using Ocelot.
    ├── 02 Building Blocks                 # Shared code used to build and integrate the services
    ├── 03 Domain                          # Domain layer
    ├── 04 Services                        # Available APIS
    │   ├── Access Control                 # API Designed to only take care of allowing or denying access
    │   ├── Identity                       # API to manage authentication. Generates JWT 
    │   ├── Management                     # API responsible to manage access control. Authorize or revoke access to credentials
    │   │   ├── Management.Domain          # The specialized domain focused on management business rules
    │   │   ├── Management.Tests           # Tests

### Conventions

* `RFID`:  In this project many times I use the term "RFID" for variables or even in method names. In this project RFID refers to to any code that might be used to uniquely identify a user. 
* `SmartLockId`: A GUID used to identify any security device. That can be a smart lock or whatever other lock that can be identified and controlled remotely.

### Technologies and Patterns

#### Databases
  * `SQL Server`: Data persistency
  * `Redis`: Caching
  * `ElasticSearch` + `Kibana`: Store logs into a centralized place

#### Infra + Messaging
  * `RabbitMq`: PubSub and Request/Response patterns to allow async communication between the Access Control API and the Management API
  * `Ocelot`: Api Gateway. Rate Limit (for Authorization process)

#### Patterns
  * `Pub Sub`: Allow us to send messages from the Management Api to Access Control Api. Ex: When we Grant access to someone and we need to update the cache.
  * `Request Response`: Using RPC, make a call from the Access Control Api to fetch updated data from the Management Api.
  * `CQRS`: Command Query Responsability Segregation. Split queries from other operations. Allow us to scale different parts of the application without having to rewrite it entirely. Also helps us to organize our business logic withing command handlers.
  * `Domain Events`: Triggers actions to be executed depending on what happened in the domain. For instance, every time we Grant Access to someone we raise an AccessGrantedEvent which publishes a message to the Access Control API update the cache.
  * `Repository Pattern` + `UnitOfWork`: Centralized interface for data access and manipulation.

#### Testing
  * `xUnit`: Integration tests.

#### Async Communication and Events
  * Every time we Grant or Revoke access to a SmartLockId for an RFID we raise events. Those events publish messages through rabbitMQ which will be executed by the Access Control API to store the credentials on Redis cache. Ex:  AccessGrantedEvent and AccessRevokedEvent.
  * Every time someone is authorized or denied on the Access Control API we also publish a message wich will be executed on the management API to persist logs.
  * Every time we hit the Access Control Api and we don't have the credentials cached we request this information from the management api using request/response pattern.

### HowTos

* How to add a new migration to the Management api:
  ```c#
  dotnet ef migrations add "Migration Name" --project src\TagAC.Management.Data.EFCore --startup-project src\TagAC.Apis.Management --output-dir Migrations 
  ```
 * How to run this?
   - Using Visual Studio you can set the docker-compose project as startup and then just press F5.
   - Or, `docker compose up --build -d`

  * What is the default user/password? 
    - `User`: admin@admin.com
    - `Pass`: 123@qwe
  You can change this on the ApplicationDbInitializer.cs from the TasAC.Apis.Identity project before running the application for the first time.


