# Resume.Tech
A tool for building resumes, portfolios, and websites for your professional history

## Technical Details

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

### Object Types
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
  - Must be immutable
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
  - Examples: `CreateJob`, `PatchJob`
- **Query**
  - A service that represents a query that can read state but not mutate it
  - Examples: `GetJobById`
- **Workflow (maybe)**
  - A service that represents a sequence of commands to be executed, each within their own DB transaction
  - If a command fails, the others should be rolled back
- **POCO**
  - Anything that doesn't follow into the above categories
  - DTOs and request objects fall into this category
  - Can be a `class`, `record`, or a `struct` depending on whatever makes sense for that usecase
