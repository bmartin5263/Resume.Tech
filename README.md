# Resume.Tech
A tool for building resumes, portfolios, and websites for your professional history

## Subdomains
Resume.Tech roughly follows Domain Driven Design principals. The overall application is divided into the following subdomains:
- Identity Verification
  - Handles everything related to making sure the right people have access to the right data. 
  - Classes include: `User`
- Experience Management
  - Handles recording prior Jobs, Projects, Schools, Certifications, and more
  - Very CRUD-like, domain model is very simple
  - Classes include: `Profile`, `Job`, `IProject`, `Film`
- Media Storage
  - Handles storing media, or more likely storing references to media that is actually stored on an external system, like S3
  - Also very CRUD-like
  - Classes include: `ImageRef`, `VideoRef`
- Experience Composition
  - Handles organizing past experiences into presentable forms, like Resumes and Portfolios
  - Classes include: `Resume`, `Portfolio`
- Website Building
  - Handles user website building, using their Resumes and Portfolios as page content
  - Classes include: `Website`, `IPage`, `ResumePage`

![Subdomains](https://github.com/bmartin5263/Resume.Tech/blob/master/Wiki/Subdomains.png?raw=true)

In general, subdomain boundaries are considered based on where logical microservices _could_ be drawn.

## Object Categories
All objects/values within the system fall into 1 or more of the following categories

- **Entity**
  - Any business object that uses identity-based equality
  - Should use a stongly-typed id. For example, `Job` will have a `JobId`
  - May store references to other entities
    - If referring to an entity owned by an aggregate root, it must be a read-only reference
    - If referring to an entity in another subdomain, it must be referenced by Id
  - Examples: `JobPosition`, `ResumeSection`
- **Aggregate Root**
  - Any entity that manages child entities where all operations mutating the children go through the root
  - The only entities accessed through a `Repository` class
  - Examples: `Profile`, `Job`, `Website`
- **Value Object**
  - Any business object that uses value-based equality
  - Immutable
  - Usually a `sealed record`, but can be a `readonly record struct` when a parameterless default constructor is permissable
  - Examples: `int`, `JobId`, `DateTime`, `EmailAddress`
- **Wrapper**
  - The value object that conceptually wraps another value object, giving it additional type-information or validation
  - Interface for declaring wrappers is `IWrapper<T>`
    - Must implement `T Value { get; }` and provide a constructor that takes a single argument of type `T`
  - When serialized, wrapper instances will be unwrapped. They will be re-wrapped on deserialization
  - Examples: `JobId` (Wraps a `Guid`), `EmailAddress` (Wraps a `string`)
- **Domain Event**
  - Any business object that represents something that changed within the system
  - Emitted by entities to notify other parts of the system of change
- **Service**
  - Any stateless business object that performs some sort of business operation
  - Usually a singleton due to its stateless nature
  - Examples: `JobManager`
- **Command**
  - A service that represents a single transactional operation that mutates the state of the application
  - Should not invoke another command during execution
  - Examples: `CreateJob`, `PatchJob`
- **Query**
  - A service that represents a query that can read state but not mutate it
  - Examples: `GetJobById`
- **Workflow (maybe)**
  - A service that represents a sequence of commands to be executed, each within their own DB transaction
  - If a command fails, the others should be rolled back
    - However, if Events get published they cannot be rolled back, either need to remember to publish undo events or delay event publishing until after workflow finishes
- **POCO**
  - Anything that doesn't follow into the above categories
  - DTOs and request objects fall into this category
  - Can be a `class`, `record`, or a `struct` depending on whatever makes sense for that usecase

## Events
Events are classified as either Domain or Integration events, and Integration events can either be Local or Global (best names I could come up with)

- **Domain Events**
  - Do not leave the subdomain that published them
    - For example, Identity Domain events cannot be consumed by Experience Management
  - Are executed _in the same Unit of Work_ as the action that produced them
    - This means that if any event handlers fail, the whole operation fails
  - Allows for decoupling components within an individual Subdomain while maintaining atomicity
- **Integration Events**
  - _Leave_ the subdomain so that other subdomains can react to change
  - Asynchronously processed, outside of the Unit of Work that produced the event
  - **Local**
    - To consume the consuming service must directly depend on the producing service
    - Events will generally contain more domain-specific data
  - **Global**
    - Live in the common subdomain, which is used by every other subdomain
    - Every subdomain can consume the event without directly depending on the producer

## Concepts & Idioms
The following are various small patterns and ideas that are spread throughout the project that any new developer to the project should familiarize themselves with

### Unit of Work
Represents a single, atomic change to the state of a single subdomain.

### Soft Deleting
It is sometimes impractical to actually delete an entity from the database in case you need to undo the operation or want to use deleted data for analysis.
To remedy this, entities can be soft-deletable, which when "deleted" will merely set a flag in the database saying the entity no longer exists.

#### Implementation
To mark an entity as soft-deletable, have it should implement the interface `ISoftDeletable`.

Implementations of `IRepository<ID, TEntity>` should check if `TEntity` implements `ISoftDeletable` and take appropriate action.
When soft-deleted, the entity should not be returned from any standard query method like `FindById`, `FindAll`, etc.

### Validation
Validation can be found in 3 layers of the system. It may seem redundant to validate in 3 layers but each one adds some sort of benefit

#### Service-level Validation
This is validation that occurs on requests that get fed into Commands and Queries. 
These validations can potentially be documented automatically in the OpenAPI Schema if the API maps directly to a Command or Query
All errors in the request should be returned at once, with pathname equivalent to the request structure

#### Domain-level Validation
This is validation that occurs on Entities and Value Objects, during construction and while updating.
These validations ensure that at no point during processing will an object be in an invalid state
These should occur automatically when setting illegal values to properties

#### Database-level Validation
This is validation that occurs on the database.
These validations ensure database integrity when doing manual revisions.