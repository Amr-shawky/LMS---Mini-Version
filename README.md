# LMS - Mini Version

## Overview

The **LMS - Mini Version** is an educational .NET 8 Web API project designed for mentorship and bootcamp environments. It serves as a practical playground for students to explore **RESTful APIs**, **Entity Framework Core**, and common architectural patterns.

Unlike "perfect" production code, this repository is intentionally built with "legacy-style" practices—such as synchronous blocking calls and tight coupling—to provide a hands-on refactoring experience for junior developers.

---

## Tech Stack

- **Framework:** .NET 8.0.
- **Database:** SQL Server via Entity Framework Core.
- **Architecture:** Monolithic (Folder-based separation).
- **Documentation:** Swagger/OpenAPI.

---

## Project Structure

Following the structure of the original "CarRentalApp" reference, this project is organized into clear domains:

- **Domain/Entities:** Contains core models like `Track` and `Intern`.
- **Persistence:** Handles database context (`AppDbContext`) and data seeding.
- **Infrastructure/Repositories:** Implements a generic repository (currently with intentional design flaws for learning).
- **Controllers:** Traditional API controllers using direct dependency injection.

---

## 📝 Key Features (Learning Modules)

1. **Track Management:** Manage training paths (e.g., .NET, Cyber Security, Angular).
2. **Intern Management:** Handle student enrollments and status tracking.
3. **Data Seeding:** Includes a `DbInitializer` with pre-configured data to test filtering and business logic.

---

## ⚠️ Educational Note: Intentional Flaws

This project contains several "Bad Practices" by design to facilitate mentor-led discussions:

- **Synchronous Methods:** All DB operations are currently synchronous (`ToList`, `Find`, `SaveChanges`) to demonstrate thread-pool starvation.
- **Direct Context Access:** Controllers originally communicated directly with the `DbContext`.
- **Generic Repository Issues:** The repository currently returns `IQueryable`, leaking database logic into the API layer.
- **Lack of DTOs:** The API exposes raw database entities, showcasing security and over-posting risks.

---

## ⚙️ Getting Started

1. **Clone the repository.**
2. **Configure Database:** Update the connection string in `appsettings.json`.
3. **Run Migrations:**

```powershell
Add-Migration InitialCreate
Update-Database

```

4. **Run Project:** Press `F5` to launch the API and explore via Swagger.

---

## 👨‍🏫 Mentorship

This project is maintained by **Amr Mohamed** . The goal is to evolve this code from a "Dirty" state to a "Clean Architecture" state throughout the bootcamp.

---

## 🚀 Assignment Refactoring & Messaging Summary

### 1. Architectural Refactoring (Vertical Slicing / Feature-First)

The Enrollment module was refactored from horizontal folders (`Commands`, `Handlers`, `Queries`) into feature-first vertical slices under `Feature/enrollmentFeature/`:

- **`ListEnrollments/`**: `GET /api/Enrollment/v2` — Paginated DTO query returning `EndpointResponse<PaginatedResult<EnrollmentViewModel>>`.
- **`GetEnrollmentById/`**: `GET /api/Enrollment/v2/{id}` — Single-record DTO query returning `EndpointResponse<EnrollmentViewModel>`.
- **`GetEnrollmentsByIntern/`**: `GET /api/Enrollment/v2/intern/{internId}` — Preserved outside endpoint returning paginated DTOs.
- **`EnrollIntern/`**: `POST /api/Enrollment/v2` — Coordinates intern, track, duplicate, and capacity validation via MediatR; atomically creates Enrollment and Payment via `IUnitOfWork.ExecuteAsync()`; returns created ID `EndpointResponse<int>.Created(enrollmentId)`.
- **`CancelEnrollment/`**: `POST /api/Enrollment/v2/{id}/cancel` — Validates state, cancels enrollment, checks refund eligibility (`PaymentStatus.Completed`), refunds payment, and logs `RefundHistory` synchronously inside UOF transaction.
- **`TransferEnrollment/`**: `POST /api/Enrollment/v2/{id}/transfer/{newTrackId}` — Validates enrollment and destination track; updates track via `SaveInclude`; atomically writes outbox event within the same SQL transaction.
- **`Consumers/`**: MassTransit RabbitMQ consumers for Fanout messaging.

### 2. SaveInclude Implementation

Implemented `GeneralRepository<T>.SaveInclude(T entity, params string[] Includedproperties)` using selective EF Core property tracking (`entry.Property(p).IsModified = true` for specified scalar fields only). Applied to `updateEnrollmentTrackCommandHandler` (and restores `UpdateTrackCommand`), preventing redundant `SELECT` queries and full-entity overwrites.

### 3. Messaging & Transactional EF Core Bus Outbox Setup

- **Broker & Virtual Host**: Integrated MassTransit v8 with RabbitMQ connecting strictly to the virtual host **`Elevate`**.
- **EF Core Transactional Bus Outbox**: Configured `AddEntityFrameworkOutbox<AppDbContext>` with `UseBusOutbox()`. When `TransferEnrollmentOrchestratorHandler` publishes `EnrollmentTransferredEvent`, the event record is saved in SQL Server (`OutboxMessage` table) within the same database transaction as the enrollment update.
- **Fanout Topology**: The `EnrollmentTransferredEvent` exchange broadcasts to two independent receive endpoints/queues:
  - `transfer-refund-queue` $\rightarrow$ `RefundConsumer`
  - `transfer-notification-queue` $\rightarrow$ `TransferNotificationConsumer`
    Neither consumer competes for messages on the same queue.

### 4. RefundHistory, UOF, and Savepoint Location

- **`RefundHistory`**: Created domain entity inheriting `baseEntity` with fields `PaymentId`, `EnrollmentId`, `PreviousStatus`, `NewStatus`, `CreatedAt`.
- **Savepoint Location & Rationale**: Inside `RefundConsumer.cs`, a real savepoint `BeforeRefundHistory` is established after the payment refund command and before writing the `RefundHistory` record:
  ```csharp
  await _unitOfWork.AddSavePointAsync("BeforeRefundHistory");
  ```
  If creating `RefundHistory` fails, the consumer rolls back to the savepoint and rethrows the exception, ensuring the entire consumer transaction rolls back atomically without partial writes, allowing MassTransit retry policy to kick in.
