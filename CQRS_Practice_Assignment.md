# 🎯 CQRS Practice Assignment — MediatR Query/Handler Migration

> **Welcome, Intern!** Your mission is to migrate 7 Read operations to MediatR Query/Handler pairs.
> Each endpoint currently throws `NotImplementedException`. Your job is to:
> 1. Create the **Query record** class
> 2. Create the **Handler** class (with the DB query logic)
> 3. Wire the **Controller** endpoint using `_mediator.Send(...)`
> 4. Test via **Swagger** to verify it returns 200 OK

---

## 🏗️ Architecture Overview

```
Controller  →  Query + IMediator.Send(Query)  →  Handler  →  Repository  →  Database
     ↑                ↑                       ↑
  STEP 3           STEP 1                  STEP 2
```

**You need to build all 3 pieces for each task.**

---

## 📚 Key Files to Reference

| File | Purpose |
|---|---|
| `Domain/Entities/` | Entity classes (`Track`, `Intern`, `Enrollment`, `Payment`) |
| `Domain/Repositories/IGeneralRepository.cs` | Repository interface — `GetTable()`, `GetByIdAsync()` |
| `DTOs/` | DTO classes — `TrackDto`, `InternDto`, `EnrollmentDto`, `PaymentDto` |
| `Mapping/MappingExtensions.cs` | Mapping methods — `.ToDto()` |
| `Domain/Enums/PaymentStatus.cs` | Payment status enum (`Pending`, `Completed`, `Failed`, `Refunded`) |
| `Features/Tracks/Queries/GetAllTracksQuery.cs` | Example of a working Query record |
| `Features/Tracks/Handlers/GetAllTracksQueryHandler.cs` | Example of a working Handler |

---

## 📋 Assignment Tasks

---

### Task 1: `GetTrackByIdQuery` → Get a Single Track by ID

| | |
|---|---|
| **📁 Query File** | `Features/Tracks/Queries/GetTrackByIdQuery.cs` (**already exists** — used by Orchestrators) |
| **📁 Handler File** | Create: `Features/Tracks/Handlers/GetTrackByIdQueryHandler.cs` |
| **📁 Controller** | Wire: `TrackController.GetById(int id)` |
| **🎯 Business Goal** | Admin needs to view the full details of a specific training track. |
| **📥 Query Input** | `int Id` |
| **📤 Handler Returns** | `TrackDto` |
| **🔗 Swagger Endpoint** | `GET /api/Track/{id}` |

**What to do:**
1. The Query record **already exists** — open it to see the input/response types
2. Create the Handler: implement `IRequestHandler<GetTrackByIdQuery, TrackDto>`
3. Inside `Handle()`: use `_trackRepository.GetByIdAsync(request.Id)`, map with `.ToDto()`, return the result
4. In the Controller: use `await _mediator.Send(new GetTrackByIdQuery(id))` and return with `Ok(...)` or `NotFound()`

---

### Task 2: `GetAllInternsQuery` → List All Interns

| | |
|---|---|
| **📁 Query File** | Create: `Features/Interns/Queries/GetAllInternsQuery.cs` |
| **📁 Handler File** | Create: `Features/Interns/Handlers/GetAllInternsQueryHandler.cs` |
| **📁 Controller** | Wire: `InternController.GetAll()` |
| **🎯 Business Goal** | Admin needs to see a summary list of all registered interns with their assigned track names. |
| **📥 Query Input** | None (parameterless) |
| **📤 Handler Returns** | `IEnumerable<InternDto>` |
| **🔗 Swagger Endpoint** | `GET /api/Intern` |

**What to do:**
1. Create the Query: `public record GetAllInternsQuery : IRequest<IEnumerable<InternDto>>;`
2. Create the Handler: implement `IRequestHandler<GetAllInternsQuery, IEnumerable<InternDto>>`
3. Inside `Handle()`: use `_internRepository.GetTable()`, `.Include(i => i.Track)`, `.ToListAsync(cancellationToken)`, map with `.Select(i => i.ToDto())`
4. In the Controller: use `await _mediator.Send(new GetAllInternsQuery())` and return with `Ok(...)`

---

### Task 3: `GetInternByIdQuery` → Get a Single Intern by ID

| | |
|---|---|
| **📁 Query File** | Create: `Features/Interns/Queries/GetInternByIdQuery.cs` |
| **📁 Handler File** | Create: `Features/Interns/Handlers/GetInternByIdQueryHandler.cs` |
| **📁 Controller** | Wire: `InternController.GetById(int id)` |
| **🎯 Business Goal** | Admin needs to view full details of a specific intern (name, email, birth year, track info). |
| **📥 Query Input** | `int Id` |
| **📤 Handler Returns** | `InternDto?` (nullable — null if not found) |
| **🔗 Swagger Endpoint** | `GET /api/Intern/{id}` |

**What to do:**
1. Create the Query: `public record GetInternByIdQuery(int Id) : IRequest<InternDto?>;`
2. Create the Handler: implement `IRequestHandler<GetInternByIdQuery, InternDto?>`
3. Inside `Handle()`: use `_internRepository.GetTable()`, `.Include(i => i.Track)`, `.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken)`, map with `.ToDto()`
4. In the Controller: use `await _mediator.Send(new GetInternByIdQuery(id))` and return with `Ok(...)` or `NotFound()`

---

### Task 4: `GetActiveTracksQuery` → List Only Active Tracks

| | |
|---|---|
| **📁 Query File** | Create: `Features/Tracks/Queries/GetActiveTracksQuery.cs` |
| **📁 Handler File** | Create: `Features/Tracks/Handlers/GetActiveTracksQueryHandler.cs` |
| **📁 Controller** | Wire: `TrackController.GetActiveTracks()` |
| **🎯 Business Goal** | When enrolling an intern, the system should only show tracks that are currently active. |
| **📥 Query Input** | None (parameterless) |
| **📤 Handler Returns** | `IEnumerable<TrackDto>` |
| **🔗 Swagger Endpoint** | `GET /api/Track/active` |

**What to do:**
1. Create the Query: `public record GetActiveTracksQuery : IRequest<IEnumerable<TrackDto>>;`
2. Create the Handler: implement `IRequestHandler<GetActiveTracksQuery, IEnumerable<TrackDto>>`
3. Inside `Handle()`: use `_trackRepository.GetTable()`, `.Where(t => t.IsActive)`, `.Include(t => t.Enrollments)`, `.ToListAsync(cancellationToken)`, map with `.Select(t => t.ToDto())`
4. In the Controller: use `await _mediator.Send(new GetActiveTracksQuery())` and return with `Ok(...)`

---

### Task 5: `GetEnrollmentsByInternQuery` → Get All Enrollments for a Specific Intern

| | |
|---|---|
| **📁 Query File** | Create: `Features/Enrollments/Queries/GetEnrollmentsByInternQuery.cs` |
| **📁 Handler File** | Create: `Features/Enrollments/Handlers/GetEnrollmentsByInternQueryHandler.cs` |
| **📁 Controller** | Wire: `EnrollmentController.GetByIntern(int internId)` |
| **🎯 Business Goal** | Admin needs to view the enrollment history of a particular intern. |
| **📥 Query Input** | `int InternId` |
| **📤 Handler Returns** | `IEnumerable<EnrollmentDto>` |
| **🔗 Swagger Endpoint** | `GET /api/Enrollment/intern/{internId}` |

**What to do:**
1. Create the Query: `public record GetEnrollmentsByInternQuery(int InternId) : IRequest<IEnumerable<EnrollmentDto>>;`
2. Create the Handler: implement `IRequestHandler<GetEnrollmentsByInternQuery, IEnumerable<EnrollmentDto>>`
3. Inside `Handle()`: use `_enrollmentRepository.GetTable()`, `.Include(e => e.Track).Include(e => e.Intern)`, `.Where(e => e.InternId == request.InternId)`, `.ToListAsync(cancellationToken)`, map with `.Select(e => e.ToDto())`
4. In the Controller: use `await _mediator.Send(new GetEnrollmentsByInternQuery(internId))` and return with `Ok(...)`

---

### Task 6: `GetPaymentByIdQuery` → Get a Single Payment by ID

| | |
|---|---|
| **📁 Query File** | Create: `Features/Payments/Queries/GetPaymentByIdQuery.cs` |
| **📁 Handler File** | Create: `Features/Payments/Handlers/GetPaymentByIdQueryHandler.cs` |
| **📁 Controller** | Wire: `PaymentController.GetById(int id)` |
| **🎯 Business Goal** | Finance team needs to look up a specific payment record to verify transaction details. |
| **📥 Query Input** | `int Id` |
| **📤 Handler Returns** | `PaymentDto?` (nullable — null if not found) |
| **🔗 Swagger Endpoint** | `GET /api/Payment/{id}` |

**What to do:**
1. Create the Query: `public record GetPaymentByIdQuery(int Id) : IRequest<PaymentDto?>;`
2. Create the Handler: implement `IRequestHandler<GetPaymentByIdQuery, PaymentDto?>`
3. Inside `Handle()`: use `_paymentRepository.GetByIdAsync(request.Id)`, map with `.ToDto()`
4. In the Controller: use `await _mediator.Send(new GetPaymentByIdQuery(id))` and return with `Ok(...)` or `NotFound()`

---

### Task 7: `GetPendingPaymentsQuery` → List All Pending Payments

| | |
|---|---|
| **📁 Query File** | Create: `Features/Payments/Queries/GetPendingPaymentsQuery.cs` |
| **📁 Handler File** | Create: `Features/Payments/Handlers/GetPendingPaymentsQueryHandler.cs` |
| **📁 Controller** | Wire: `PaymentController.GetPending()` |
| **🎯 Business Goal** | Finance team needs a dashboard view of all payments that haven't been completed yet. |
| **📥 Query Input** | None (parameterless) |
| **📤 Handler Returns** | `IEnumerable<PaymentDto>` |
| **🔗 Swagger Endpoint** | `GET /api/Payment/pending` |

**What to do:**
1. Create the Query: `public record GetPendingPaymentsQuery : IRequest<IEnumerable<PaymentDto>>;`
2. Create the Handler: implement `IRequestHandler<GetPendingPaymentsQuery, IEnumerable<PaymentDto>>`
3. Inside `Handle()`: use `_paymentRepository.GetTable()`, `.Where(p => p.Status == PaymentStatus.Pending)`, `.Include(p => p.Enrollment)`, `.ToListAsync(cancellationToken)`, map with `.Select(p => p.ToDto())`
   - You'll need: `using LMS___Mini_Version.Domain.Enums;`
4. In the Controller: use `await _mediator.Send(new GetPendingPaymentsQuery())` and return with `Ok(...)`

---

## ✅ How to Verify Your Work

1. **Run the application**: `dotnet run` from the project root
2. **Open Swagger**: Navigate to `https://localhost:{port}/swagger`
3. **Test each endpoint**:
   - ❌ **Before your fix**: The endpoint returns **500 Internal Server Error** (NotImplementedException)
   - ✅ **After your fix**: The endpoint returns **200 OK** with the correct JSON data

## 🚫 Rules

- **DO NOT** use `ConfigureAwait(false)` — it's the default in modern .NET
- Handlers return **DTOs** (e.g., `TrackDto`, `InternDto`) — NOT ViewModels
- Use `async/await` for all DB calls
- Pass `cancellationToken` to all async EF Core methods
- Look at existing working examples (e.g., `GetAllTracksQueryHandler`) for reference

Good luck! 🚀
