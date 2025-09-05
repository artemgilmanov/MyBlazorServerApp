# MyBlazorServerApp

This application, named MyBlazorServerApp, is a microservice built with ASP.NET Core that handles financial calculations. It is structured to be modular and maintainable, using several common design patterns and architectural approaches. The service exposes a REST API for performing calculations and saving them to a database.

## Key Architectural Patterns and Approaches

The application's design is based on several key architectural and design patterns that promote a clean, maintainable, and scalable codebase.

* **Layered Architecture**: The solution is divided into distinct projects, each representing a different layer:
    * **MyBlazorServerApp.Domain**: This is the core layer that contains the business logic and entities (e.g., `InstallmentEntity`, `DurationEntity`). It's the heart of the application, independent of any data access or presentation concerns.
    * **MyBlazorServerApp.Infrastructure**: This layer handles data persistence and external services. It contains the **PostgreSQLRepository** for database operations and a separate **CalculationRepository** for calculation logic, effectively decoupling the business logic from data access details.
    * **MyBlazorServerApp.Resources**: This project likely contains Data Transfer Objects (DTOs) or API resource models (`Installment`, `Duration`) that represent the data exchanged with the client.
    * **MyBlazorServerApp.Shared**: This layer holds shared contracts, interfaces, and models that are used across multiple projects. This prevents code duplication and enforces consistency.
    * **MyBlazorServerApp.Test**: This is the testing layer, showcasing a commitment to quality and providing a separation of test code from the main application code. 
* **Command Query Responsibility Segregation (CQRS)**: The application separates the actions that change state (commands) from the actions that retrieve state (queries). This is implemented using the **MediatR** library.
    * **Commands**: Classes like `CalculateDurationCommand` encapsulate the intent of an action.
    * **Command Handlers**: Classes like `CalculateDurationCommandHandler` contain the logic to execute the command. This separation simplifies the handling of business logic and allows for distinct modeling of read and write operations.

* **Dependency Injection (DI)**: The `Program.cs` file shows extensive use of Dependency Injection. Services like **MediatR**, **AutoMapper**, and various repositories (`ICalculationRepository`, `PostgreSQLRepository`) are registered in the service container. The controllers and other classes then receive these dependencies through their constructors, reducing coupling and making the code easier to test and maintain.

---

## Implemented Design Patterns

The codebase demonstrates the use of several specific design patterns to solve common software design problems.

* **Repository Pattern**: This pattern abstracts data access logic. The `PostgreSQLRepository` and `ICalculationRepository` act as intermediaries between the business logic and the data source (PostgreSQL). This allows you to swap out the underlying database technology (e.g., from PostgreSQL to an in-memory database) without changing the business logic.

* **Pipeline Pattern**: The MediatR library's `IPipelineBehavior` implementation creates a pipeline of behaviors that a request passes through before reaching its final handler. This is demonstrated by `ValidationBehavior<TRequest, TResponse>`, which acts as a middleware, ensuring all commands are validated using FluentValidation before execution.

* **FluentValidation**: The `AbstractValidator` classes (`CalculateInstallmentCommandValidator`) define validation rules for commands. This approach separates validation logic from the business logic, making it reusable, declarative, and easy to read. The **ValidationBehavior** ensures this validation is automatically applied to all commands, enforcing a consistent and robust validation strategy.

* **Middleware Pattern**: The `ExceptionMiddleware` intercepts unhandled exceptions in the request pipeline. This pattern provides a centralized, consistent way to handle errors and return a standardized error response to the client, improving API reliability and developer experience.

* **Observer Pattern (via MediatR)**: While not explicitly shown, MediatR often facilitates a form of the Observer pattern. A command handler "observes" and reacts to a specific command. This decouples the sender (the controller) from the receiver (the handler), allowing for a more flexible and modular design.

* **Factory Pattern (Implicit)**: Dependency Injection containers often use an implicit form of the Factory pattern. When you request an instance of an interface like `IMediator`, the container internally uses a "factory" to create and provide the correct implementation, abstracting the object creation process.

* **Data Mapper Pattern (via AutoMapper)**: **AutoMapper** is used to map between different object types, specifically between domain entities (`DurationEntity`) and resource models (`Duration`). This pattern keeps the domain models clean and free from presentation or serialization concerns.

---

## Technical Components and Libraries

* **ASP.NET Core**: The application framework, providing a robust environment for building web APIs and Blazor applications.
* **Blazor Server**: The application uses Blazor for its front-end UI, running on the server and communicating with the client via SignalR.
* **PostgreSQL**: The chosen relational database, accessed via **Entity Framework Core** and the **Npgsql** provider.
* **Serilog**: The logging library used for structured logging. It captures and enriches log events, which can be sent to various sinks like the console, files, or services like Seq.
* **xUnit, Moq, FluentAssertions**: These are the testing frameworks and libraries used to write unit tests, mock dependencies, and perform fluent assertions, indicating a test-driven or test-friendly development approach.

The combination of these patterns and tools results in a robust, maintainable, and scalable application architecture. The clear separation of concerns, automated validation, and standardized error handling make the system reliable and easy to evolve.

# User Interface (UI) Structure

---

The application's user interface is built using **Blazor Server**.

---

**Calculator Page (`/`)**

The `Calculator.razor` page is the main interactive component where users perform calculations.

* **Mode Selection**: Users can switch between calculating **Installments** and **Duration** using a tab-like interface. This is controlled by the `currentMode` enum.
* **Input Fields**: Depending on the selected mode, the component dynamically displays the relevant input fields:
    * **Installment Mode**: Requires **Total Amount** and **Number of Installments**.
    * **Duration Mode**: Requires **Total Amount** and **Monthly Installment**.
* **Live Validation**: The component handles client-side validation errors by checking the `validationErrors` dictionary. If the API returns a `400 BadRequest` status code with validation errors, these are displayed directly beneath the corresponding input fields, giving immediate feedback to the user.
* **API Interaction**: The page uses `IHttpClientFactory` to make HTTP calls to the back-end API endpoints (`/api/Calculator/calculate-installment` and `/api/Calculator/calculate-duration`). This demonstrates the **API Consumption Pattern**, where the front-end acts as a client to the back-end microservice.
* **Save Functionality**: After a successful calculation, a "Save Calculation" button appears. Clicking it triggers a new API call (`/api/Calculator/save-installment` or `/api/Calculator/save-duration`) to persist the result in the database.

---

**Saved Calculations Page (`/saved-calculations`)**

This page is dedicated to displaying the historical data retrieved from the database.

* **Data Retrieval**: The component makes two separate API calls to `/api/Calculator/get-all-installments` and `/api/Calculator/get-all-durations` in the `OnInitializedAsync` lifecycle method. This pattern ensures data is loaded as soon as the page is accessed.
* **State Management**: The page uses a simple state machine with variables like `isLoading`, `installmentError`, and `durationError` to manage the UI's state during data fetching. It displays "Loading..." messages, error messages, or the final data tables based on the current state.
* **Data Presentation**: The fetched data for both installments and durations is displayed in clean, responsive tables using **Bootstrap** CSS classes (`table`, `table-striped`). This provides a clear, organized view of the saved calculations.

# Future Improvements

---

**1. Database and Data Persistence**

The current use of an in-memory database (`Microsoft.EntityFrameworkCore.InMemory`) is excellent for rapid prototyping and testing but is not suitable for a production environment. The application is already configured to use **PostgreSQL**, but this feature is commented out.

* **Action:** Enable the PostgreSQL configuration by uncommenting the relevant code in `Program.cs`. This will ensure all saved calculation data persists across application restarts.
* **Action:** Implement and configure database migrations to manage schema changes over time. This will allow for seamless updates to the database structure as new features are added.
* **Benefit:** Provides **data durability** and integrity, ensuring that critical business data is not lost when the service is restarted or deployed.

---

**2. Comprehensive Test Coverage**

The existing test project (`MyBlazorServerApp.Test`) demonstrates a solid approach to testing, but a production-grade application requires more comprehensive coverage, including edge cases.

* **Action:** Expand the unit and integration tests to cover all possible scenarios and edge cases. This includes testing for:
    * Invalid inputs (e.g., zero or negative values for amount and installment).
    * Boundary conditions (e.g., very large or very small numbers).
    * Error handling for external dependencies (e.g., database connection failures, network timeouts).
* **Action:** Introduce a dedicated test suite for the `PostgreSQLRepository` to ensure that data is correctly saved, retrieved, and managed in the actual database.
* **Benefit:** Increases the **reliability** and stability of the application. Comprehensive testing provides a safety net for future refactoring and feature development, catching bugs before they reach production.

---

**3. Enhanced Security and Observability**

A production microservice requires robust security and monitoring capabilities.

* **Action:** Implement proper authentication and authorization to secure API endpoints, ensuring only authorized users can perform actions like saving calculations.
* **Action:** Enhance the logging to include more granular details about request processing, especially for failed requests. Configure Serilog to send logs to a centralized logging system like **Seq** or **ELK Stack**, which is already referenced in the dependencies.
* **Benefit:** Improves the **security posture** of the application and provides better **observability**, making it easier to monitor performance, diagnose issues, and respond to incidents in a production environment.

---
 
**4.  Alternative Libraries for AutoMapper and MediatR

Due to potential policy changes requiring subscriptions for certain libraries, it's prudent to consider open-source alternatives to **AutoMapper** and **MediatR** to ensure the project remains free to use and maintain.

#### **Alternatives to AutoMapper**

While AutoMapper is a powerful tool for object-to-object mapping, several alternatives offer similar functionality with varying approaches.

* **Manual Mapping**: For simpler DTOs, manual mapping is often the most straightforward and performant option. It involves writing explicit code to transfer data from one object to another. This approach eliminates a dependency and provides complete control over the mapping process, which is especially useful when business logic is involved.
* **Mapster**: A convention-based mapping library that is often faster than AutoMapper because it uses expression trees to generate code at compile time. It is a lightweight and high-performance alternative that requires minimal configuration.
* **TinyMapper**: Another fast, lightweight, and convention-based object mapper. While less feature-rich than AutoMapper, it provides all the core functionality needed for most mapping scenarios.

#### **Alternatives to MediatR**

If a subscription is required for MediatR, you could replace it with a more basic, hand-rolled **Command/Handler pattern** or with a similar, free library.

* **Hand-Rolled Command/Handler Pattern**: You can build a custom, lightweight version of the Command/Handler pattern. This involves creating a `Mediator` class that resolves and executes handlers based on the command type. This provides a clear separation of concerns without an external dependency. This approach would require a bit more boilerplate code but offers complete control and no licensing concerns.
* **Simple Injector's Decorator Pattern**: If you are already using a dependency injection container like Simple Injector, you can leverage its decorator pattern to implement a similar pipeline behavior as MediatR, where you wrap the main handler with validation or logging decorators. This provides a flexible and powerful way to build the pipeline without a dedicated library.
* **MassTransit**: While primarily a message bus, **MassTransit** can also be used as an in-process mediator. It is a more robust, but complex, alternative that is well-suited for applications that might need to scale to a distributed message-based architecture in the future.

The choice of alternative depends on the project's specific needs, complexity, and performance requirements. For many applications, a simple, custom-built solution or a lightweight library like Mapster would be a suitable replacement.