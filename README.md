# CleanArchitecture-CQRS-MediatR

# Products API — Clean Architecture

A RESTful API for managing products, built with **ASP.NET Core**, following **Clean Architecture** principles and using **CQRS** and **MediatR** to separate application responsibilities.

## 🚀 Overview

This project demonstrates how to build a maintainable and scalable ASP.NET Core Web API by separating business logic, application logic, infrastructure, and presentation into independent layers.

The API supports creating, retrieving, updating, and deleting products, with asynchronous database operations and structured logging using Serilog.

## 🛠️ Tech Stack

- **C# & ASP.NET Core Web API**
- **Clean Architecture**
- **CQRS (Command Query Responsibility Segregation)**
- **MediatR**
- **Entity Framework Core**
- **SQL Server**
- **Data Transfer Objects (DTOs)**
- **Dependency Injection**
- **Asynchronous Programming (async/await)**
- **Serilog — Structured Logging**
- **Swagger / OpenAPI**

## 🏗️ Architecture

The solution is organized into four main layers:

```text
Products/
├── Products.Domain/
│   └── Entities/
│
├── Products.Application/
│   ├── DTOs/
│   ├── Interfaces/
│   └── Products/
│       ├── Commands/
│       └── Queries/
│
├── Products.Infrastructure/
│   ├── Data/
│   └── Repositories/
│
└── Products.API/
    ├── Controllers/
    ├── Middleware/
    ├── appsettings.json
    └── Program.cs
```

### Layer Responsibilities

- **Domain:** Contains core business entities and business rules.
- **Application:** Contains commands, queries, handlers, DTOs, and repository interfaces.
- **Infrastructure:** Implements data access using Entity Framework Core and SQL Server.
- **API:** Exposes HTTP endpoints and configures dependency injection, middleware, and Serilog.

### Request Flow

```text
HTTP Request
    ↓
API Controller
    ↓
MediatR
    ↓
Command / Query Handler
    ↓
Repository Interface
    ↓
Infrastructure Repository
    ↓
Entity Framework Core
    ↓
SQL Server
```

## ✨ Features

- Create a new product.
- Retrieve all products.
- Retrieve a product by ID.
- Update an existing product.
- Delete a product.
- Separate read operations from write operations using CQRS.
- Keep application logic independent of database implementation.
- Use `async` and `await` for asynchronous database operations.
- Use `CancellationToken` where supported to allow cancellation of ongoing operations.
- Apply DTOs to control API request and response models.
- Use dependency injection to manage application dependencies.
- Implement structured logging with Serilog.
- Log HTTP requests, execution status, and request duration.
- Record application events using appropriate log levels.
- Handle business exceptions and unexpected errors through centralized exception-handling middleware.
- Configure logging through `appsettings.json`.
- Write logs to the console and rolling log files.

## 📊 Logging with Serilog

The project uses **Serilog** for structured logging, making application events and errors easier to monitor and investigate.

### Logging Capabilities

- **Structured Logging:** Store contextual properties such as product IDs and operation details.
- **HTTP Request Logging:** Record incoming requests, response status codes, and execution duration.
- **Exception Logging:** Capture business exceptions and unexpected application errors.
- **Multiple Log Levels:** Use `Information`, `Warning`, and `Error` according to the event's severity.
- **Console Logging:** Display logs during development.
- **File Logging:** Store logs in rolling files for later inspection.
- **Configuration-Based Logging:** Manage minimum log levels and output destinations through `appsettings.json`.

### Example

```csharp
logger.LogInformation(
    "Product created successfully. ProductId: {ProductId}",
    product.Id);
```

This uses structured logging, allowing `ProductId` to be stored as a separate property rather than only as part of a formatted message.

## ⚡ Asynchronous Programming

The API uses C# `async` and `await` to perform database operations asynchronously.

### Example

```csharp
public async Task<ProductDTO?> Handle(
    GetProductByIdQuery request,
    CancellationToken cancellationToken)
{
    var product = await _repository.GetByIdAsync(request.Id);

    if (product is null)
        throw new BusinessException("Product Not Found", 404);

    return new ProductDTO
    {
        Name = product.Name,
        Price = product.Price
    };
}
```

### Benefits

- Avoids blocking request-processing threads while awaiting asynchronous I/O.
- Improves scalability when handling multiple concurrent requests.
- Supports responsive request processing during database operations.
- Allows cancellation to be propagated through supported operations using `CancellationToken`.

**Note:** Asynchronous programming does not automatically make an individual database query faster. Its main benefit for I/O-bound operations is improved resource utilization and scalability under concurrent workloads.

## 🔗 API Endpoints

Base URL: `/api/products`

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/products` | Get all products |
| `GET` | `/api/products/{id}` | Get a product by ID |
| `POST` | `/api/products` | Create a new product |
| `PUT` | `/api/products/{id}` | Update an existing product |
| `DELETE` | `/api/products/{id}` | Delete a product |

### Example: Create a Product

**Request**

```http
POST /api/products
Content-Type: application/json
```

```json
{
  "name": "Laptop",
  "price": 25000
}
```

### Example: Update a Product

**Request**

```http
PUT /api/products/1
Content-Type: application/json
```

```json
{
  "name": "Updated Laptop",
  "price": 28000
}
```

*The request body should match the properties defined in the application's command.*

## ⚙️ Getting Started

### Prerequisites

- .NET SDK compatible with the project target framework.
- SQL Server.
- Visual Studio or Visual Studio Code.

### 1. Clone the Repository

```bash
git clone https://github.com/AhmedFaragFekryDahy/Products-Clean-Architecture.git
cd Products-Clean-Architecture
```

Ensure the repository URL and solution directory match your actual GitHub repository.

### 2. Configure the Database

Update the `DefaultConnection` connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=ProductsDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

Use the appropriate connection string for your SQL Server setup.

### 3. Configure Serilog

Add the Serilog configuration to your existing `appsettings.json`:

```json
{
  "Serilog": {
    "Using": [
      "Serilog.Sinks.Console",
      "Serilog.Sinks.File"
    ],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft.AspNetCore": "Warning",
        "Microsoft.EntityFrameworkCore.Database.Command": "Warning"
      }
    },
    "Enrich": [
      "FromLogContext"
    ],
    "WriteTo": [
      {
        "Name": "Console"
      },
      {
        "Name": "File",
        "Args": {
          "path": "logs/log-.txt",
          "rollingInterval": "Day",
          "retainedFileCountLimit": 7
        }
      }
    ]
  }
}
```

Merge this section with your existing configuration instead of replacing the entire file.

Ensure the required Serilog packages are installed, including:

- `Serilog.AspNetCore`
- `Serilog.Settings.Configuration`
- `Serilog.Sinks.Console`
- `Serilog.Sinks.File`

Configure Serilog in `Program.cs` and enable HTTP request logging:

```csharp
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));
```

```csharp
app.UseSerilogRequestLogging();
```

### 4. Apply Database Migrations

Run the migration commands from the solution directory, adjusting project paths to match your setup:

```bash
dotnet ef migrations add InitialCreate \
  --project Products.Infrastructure \
  --startup-project Products.API
```

```bash
dotnet ef database update \
  --project Products.Infrastructure \
  --startup-project Products.API
```

Ensure the required EF Core tooling and design packages are installed and that the actual `DbContext` is configured correctly.

If migrations already exist, do not create another `InitialCreate` migration unnecessarily. Apply the existing migrations instead.

### 5. Run the API

```bash
dotnet run --project Products.API
```

Open the Swagger URL displayed in the console to explore and test the endpoints.

Check the console output and the generated `logs` directory to verify that logging is working.

## 🎯 Learning Objectives

This project focuses on understanding and applying:

- Clean Architecture and separation of concerns.
- Dependency Inversion Principle.
- CQRS using MediatR.
- Repository Pattern.
- Entity Framework Core database operations.
- DTOs and request validation.
- Dependency Injection in ASP.NET Core.
- Asynchronous programming using `async` and `await`.
- Cancellation tokens in asynchronous operations.
- Structured logging with Serilog.
- Centralized exception handling and HTTP request logging.

## 📌 Future Improvements

- FluentValidation with MediatR pipeline behaviors.
- Pagination, filtering, and sorting.
- Unit and integration testing.
- API versioning.
- Docker support.
- Correlation IDs for request tracing.
- Health checks and monitoring.

## 👨‍💻 Author

**Ahmed Farag Fekry Dahy**

Full Stack Developer | .NET & Angular

- GitHub: [AhmedFaragFekryDahy](https://github.com/AhmedFaragFekryDahy)
- LinkedIn: [Ahmed Farag](https://www.linkedin.com/in/ahmedfaragfekry/)

---

*Built as a hands-on project to practice clean, maintainable, and testable backend development with ASP.NET Core.*
