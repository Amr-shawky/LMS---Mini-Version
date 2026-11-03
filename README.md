# LMS - Mini Version

##  Overview

The **LMS - Mini Version** is an educational .NET 8 Web API project designed for mentorship and bootcamp environments. It serves as a practical playground for students to explore **RESTful APIs**, **Entity Framework Core**, and common architectural patterns.

Unlike "perfect" production code, this repository is intentionally built with "legacy-style" practices—such as synchronous blocking calls and tight coupling—to provide a hands-on refactoring experience for junior developers.

---

##  Tech Stack

* **Framework:** .NET 8.0.
* **Database:** SQL Server via Entity Framework Core.
* **Architecture:** Monolithic (Folder-based separation).
* **Documentation:** Swagger/OpenAPI.

---

##  Project Structure

Following the structure of the original "CarRentalApp" reference, this project is organized into clear domains:

* **Domain/Entities:** Contains core models like `Track` and `Intern`.
* **Persistence:** Handles database context (`AppDbContext`) and data seeding.
* **Infrastructure/Repositories:** Implements a generic repository (currently with intentional design flaws for learning).
* **Controllers:** Traditional API controllers using direct dependency injection.

---

## 📝 Key Features (Learning Modules)

1. **Track Management:** Manage training paths (e.g., .NET, Cyber Security, Angular).
2. **Intern Management:** Handle student enrollments and status tracking.
3. **Data Seeding:** Includes a `DbInitializer` with pre-configured data to test filtering and business logic.

---

## ⚠️ Educational Note: Intentional Flaws

This project contains several "Bad Practices" by design to facilitate mentor-led discussions:

* **Synchronous Methods:** All DB operations are currently synchronous (`ToList`, `Find`, `SaveChanges`) to demonstrate thread-pool starvation.
* **Direct Context Access:** Controllers originally communicated directly with the `DbContext`.
* **Generic Repository Issues:** The repository currently returns `IQueryable`, leaking database logic into the API layer.
* **Lack of DTOs:** The API exposes raw database entities, showcasing security and over-posting risks.

---

## ⚙️ Getting Started

1. **Clone the repository.**
2. **Configure Database:** Update the connection string in `appsettings.json`.

3. Provide secrets locally (never commit them), e.g. with user-secrets:
   ```powershell
   dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<sql-connection-string>"
   dotnet user-secrets set "RabbitMq:Host" "localhost"
   dotnet user-secrets set "RabbitMq:Username" "<user>"
   dotnet user-secrets set "RabbitMq:Password" "<password>"
   ```
4. Create the RabbitMQ virtual host and grant permissions:
   ```bash
   rabbitmqctl add_vhost Elevate
   rabbitmqctl set_permissions -p Elevate <user> ".*" ".*" ".*"
   ```
5. Apply migrations and run:
   ```powershell
   Update-Database
   ```

---


4. **Run Project:** Press `F5` to launch the API and explore via Swagger.

---

## Implementation Summary (Enrollment Refactoring)
### Structure
The Enrollment module was refactored into a **vertical-slice / feature-first** layout while keeping the existing Domain and Infrastructure boundaries:

```
Features/Enrollments/
  ListEnrollments/        query + handler + endpoint
  GetEnrollment/          query + handler + endpoint
  EnrollIntern/           command + handler + endpoint
  CancelEnrollment/       command + handler + endpoint
  TransferEnrollment/     orchestrator + UpdateEnrollmentTrack command + endpoint
  Refunds/                RefundPayment command, CreateRefundHistory command, consumers
  GetEnrollmentsByIntern/ relocated, behavior unchanged
```

### Endpoints (Minimal API, all return `EndpointResponse`)
| Operation | Route |
|---|---|
| List (paginated) | `GET /api/Enrollment/v2` |
| Get by id | `GET /api/Enrollment/v2/{id}` |
| Enroll intern | `POST /api/Enrollment/v2` → `EndpointResponse<int>.Created(id)` |
| Cancel | `POST /api/Enrollment/v2/{id}/cancel` |
| Transfer | `POST /api/Enrollment/v2/{id}/transfer/{newTrackId}` |

### CQRS / MediatR
- Queries return `RequestResponse<TDto>` / `RequestResponse<PaginatedResult<TDto>>` (DTOs only, never entities).
- `CreateEnrollment` returns `RequestResponse<int>`; other writes return `RequestResponse<bool>` / `RequestResponse`.
- Each handler injects exactly one `IGeneralRepository<TEntity>`.
- `TransferEnrollmentOrchestratorHandler` coordinates steps through `IMediator` only (no repository/DbContext access). Cancel and Enroll workflows also run inside `IUnitOfWork.ExecuteAsync`.

### SaveInclude
`GeneralRepository<T>.SaveInclude(entity, params string[] Includedproperties)` attaches the entity and marks **only the listed scalar properties** as modified (no full-entity overwrite). It is used by `UpdateEnrollmentTrackCommandHandler` (updates `TrackId` only) and makes the existing `UpdateTrack` functional again.

### Messaging (MassTransit v8 + RabbitMQ)
- Virtual host: **`Elevate`**; credentials come from configuration/user-secrets.
- Transfer publishes **`EnrollmentTransferred`** (fanout) to **two independent queues**:
  - **Refund consumer** – dispatches refund commands via `IMediator`.
  - **Notification consumer** – logs the transfer (enrollment, intern, old/new track ids).
- The API responds as soon as the transfer commits; it never waits for consumers.
- **Retry:** exponential/interval retry configured on the Refund consumer's receive endpoint. A configuration flag can throw a temporary exception to demonstrate retries.

### EF Core Transactional Bus Outbox
- MassTransit SQL Server EF Core outbox registered on `AppDbContext` (outbox tables + migration).
- The `EnrollmentTransferred` event is stored in the **same SQL transaction** as the enrollment update; the bus outbox delivery service forwards it to RabbitMQ after commit.
- A failed/rolled-back transaction produces no event; a RabbitMQ outage does not roll back a committed transfer (the outbox message stays pending for later delivery).

### Refunds, RefundHistory & Savepoint
- Eligibility is based on the **original enrollment's Payment** and only `PaymentStatus.Completed` is refunded (missing, pending, failed, or already-refunded payments are skipped, which also guards against repeated messages).
- Refund = `RefundPaymentCommand` (Payment → `Refunded`) + `CreateRefundHistoryCommand` (new `RefundHistory` entity inheriting `baseEntity`, with migration), executed as one atomic UOF transaction.
- **Savepoint:** in the Refund consumer, a savepoint is added after the refund command and before the history write. If history creation fails, the consumer rolls back to the savepoint (to discard the partial history work), then **rethrows** so the whole transaction rolls back and MassTransit retry kicks in. The failure is never swallowed.
- Cancel Enrollment performs eligible refunds synchronously inside its own UOF transaction using the same eligibility rules and writes `RefundHistory` too.

### Manual Verification Checklist
- [ ] Five endpoints reachable and return `EndpointResponse` wrappers
- [ ] List is paginated; single-item read returns correct DTO
- [ ] Create returns new id and enforces validations
- [ ] Cancel / Transfer enforce business rules (existence, cancelled state, active track, same track, capacity)
- [ ] Valid transfer → one event delivered to both queues
- [ ] Completed payment → `Refunded` + `RefundHistory` row; non-eligible payments untouched
- [ ] Forced transient failure triggers retry
- [ ] Outbox row persisted with enrollment update and delivered after commit; rolled-back transfer emits nothing
- [ ] `SaveInclude` updates only intended column(s)
- [ ] `GetEnrollmentsByIntern` and `UpdateTrack` still work

---

## Project Structure (overview)

* **Domain/Entities** – core models (`Track`, `Intern`, `Enrollment`, `Payment`, `RefundHistory`).
* **Persistence** – `AppDbContext`, migrations, seeding.
* **Infrastructure** – `GeneralRepository<T>`, `UnitOfWork`, messaging configuration.
* **Features** – feature-first vertical slices (see above).

## Mentorship

Maintained by **Amr Mohamed**. The goal is to evolve this code from a "dirty" state to a clean architecture throughout the bootcamp.
