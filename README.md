﻿# Tag AC - Tag Access Control
A TAG Access Control system build using microservice architecture. 

### Project structure

    .
    ├── ...
    ├── 01 Api Gateways                    # Basic Api Gateway implementation using Ocelot.
    ├── 02 Building Blocks                 # Shared code used to build and integrate the services
    ├── 03 Domain                          # Domain layer
    ├── 04 Services                        # Available APIS
    │   ├── Access Control                 # API Desinged to only take care of allowing or denying access.
    │   ├── Identy                         # API to manage authentication. Generates JWT 
    │   ├── Management                     # API responsible to manage access control. Authorize or revoke access to credentials
    │   │   ├── Management.Domain          # The specialized domain focused on management business rules

### Conventions

* `RFID`:  In this project many times I use the term "RFID" for variables or even in method names. In this project RFID refers to to any code that might be used to uniquely identify a user. 
* `SmartLockId`: A GUID used to identify any security device. That can be a smart lock or whatever other lock that can be identified and controlled remotely.

### Technologies and Patterns

#### Databases
  * `SQL Server`: Data persistency
  * `Redis`: Caching
  * `ElasticSearch` + `Kibana`: Store logs into a centralized place

#### Infra + Messaging
  * `RabbitMq`: PubSub and Request/Response patterns to allow async communication between the AccessControl api and the ManagementApi
  * `Ocelot`: Api Gateway. Rate Limit (for Authorization process)

#### Patterns
  * `Pub Sub`: Allow us to send messages from the Management Api to Access Control Api. Ex: When we Grant access to someone and we need to update the cache.
  * `Request Response`: Using RPC, make a call from the Access Control Api to fetch updated data from the Management Api.
  * `CQRS`: Command Query Responsability Segregation. Split queries from other operations. Allow us to scale different parts of the application without having to rewrite our application entirely. Also helps us to organize our business logic withing command handlers.
  * `Domain Events`: Triggers actions to be executed depending on what happened in the domain. For instance, every time we Grant Access to someone we raise an AccessGrantedEvent which publishes a message to the AccessControl API update the cache.
  * `Repository Pattern` + `UnitOfWork`: Centralized interface for data access and manipulation.

#### Testing
  * `xUnit`: Integration tests.


### HowTos

* How to add a new migration to the Management api:
  ```c#
  dotnet ef migrations add "Migration Name" --project src\TagAC.Management.Data.EFCore --startup-project src\TagAC.Apis.Management --output-dir Migrations 
  ```

* How to run this on your machine?
  ```bash
    #clone this repo
    ~-> git clone git@github.com:LucasWBritz/TagAC.git

    ~-> cd TagAC/
    ~-> docker compose up
  ```

* How to make requests to the access control api?
  ```
    [GET] -> http://localhost:5002/AccessControl
  ```
  You'll have to add below parameters in the HEADER. (See Conventions for more info)
  - `RFID`: ID. 
  - `SmartLockId`: Device id. 

 Responses: 200 OK, 401 Unauthorized. 