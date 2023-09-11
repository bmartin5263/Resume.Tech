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
  - Classes include: `ImageRef`, `VideoRef`
- Experience Composition
  - Handles organizing past experiences into presentable forms, like Resumes and Portfolios
  - Classes include: `Resume`, `Portfolio`
- Website Building
  - Handles user website building, using their Resumes and Portfolios as page content
  - Classes include: `Website`, `IPage`, `ResumePage`
