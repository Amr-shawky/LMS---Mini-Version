# 🎯 CQRS Practice Assignment — MediatR Query/Handler Migration

> **Welcome, Intern!** Your mission is to migrate 7 Read operations from empty handler shells to fully working MediatR Query Handlers. Each handler currently throws `NotImplementedException`. Your job is to write the database query logic inside each handler, then verify it works via Swagger.

---

## 🏗️ Architecture Overview

```
Controller  →  IMediator.Send(Query)  →  Handler  →  Repository  →  Database
     ↑                                       ↑
  Already wired                        YOUR WORK HERE
```

**The Controller endpoints and Query records are already set up.** You only need to implement the `Handle()` method inside each Handler class.

---

## 📋 Assignment Tasks

### Task 1: `GetTrackByIdQuery` → Get a Single Track by ID

| | |
|---|---|
| **📁 Handler File** | `Features/Tracks/Handlers/GetTrackByIdQueryHandler.cs` |
| **🎯 Business Goal** | Admin needs to view the full details of a specific training track (name, fees, capacity, enrollment count). |
| **📥 Input** | `int Id` (from the query record) |
| **📤 Expected Output** | `TrackDetailViewModel?` (or `null` if not found) |
| **🔗 Swagger Endpoint** | `GET /api/Track/{id}` |

**Steps:**
1. Open `GetTrackByIdQueryHandler.cs`
2. Inside the `Handle()` method, use `_trackRepository.GetByIdAsync(request.Id)` to fetch the track
3. Map the entity: `track?.ToDto().ToDetailViewModel()`
4. Return the result (return `null` if not found)
5. Run the app → Test via Swagger `GET /api/Track/1`

---

### Task 2: `GetAllInternsQuery` → List All Interns

| | |
|---|---|
| **📁 Handler File** | `Features/Interns/Handlers/GetAllInternsQueryHandler.cs` |
| **🎯 Business Goal** | Admin needs to see a summary list of all registered interns with their assigned track names. |
| **📥 Input** | None (parameterless query) |
| **📤 Expected Output** | `IEnumerable<InternSummaryViewModel>` |
| **🔗 Swagger Endpoint** | `GET /api/Intern` |

**Steps:**
1. Open `GetAllInternsQueryHandler.cs`
2. Use `_internRepository.GetTable()` to get `IQueryable<Intern>`
3. Chain `.Include(i => i.Track)` to eager-load the Track navigation
4. Chain `.ToListAsync(cancellationToken)` to execute the query
5. Map: `interns.Select(i => i.ToDto().ToSummaryViewModel())`
6. Run the app → Test via Swagger `GET /api/Intern`

---

### Task 3: `GetInternByIdQuery` → Get a Single Intern by ID

| | |
|---|---|
| **📁 Handler File** | `Features/Interns/Handlers/GetInternByIdQueryHandler.cs` |
| **🎯 Business Goal** | Admin needs to view full details of a specific intern (name, email, birth year, track info). |
| **📥 Input** | `int Id` |
| **📤 Expected Output** | `InternDetailViewModel?` (or `null` if not found) |
| **🔗 Swagger Endpoint** | `GET /api/Intern/{id}` |

**Steps:**
1. Open `GetInternByIdQueryHandler.cs`
2. Use `_internRepository.GetTable().Include(i => i.Track)`
3. Chain `.FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken)`
4. Map: `intern?.ToDto().ToDetailViewModel()`
5. Run the app → Test via Swagger `GET /api/Intern/1`

---

### Task 4: `GetActiveTracksQuery` → List Only Active Tracks

| | |
|---|---|
| **📁 Handler File** | `Features/Tracks/Handlers/GetActiveTracksQueryHandler.cs` |
| **🎯 Business Goal** | When enrolling an intern, the system should only show tracks that are currently active (accepting new enrollments). |
| **📥 Input** | None (parameterless query) |
| **📤 Expected Output** | `IEnumerable<TrackSummaryViewModel>` |
| **🔗 Swagger Endpoint** | `GET /api/Track/active` |

**Steps:**
1. Open `GetActiveTracksQueryHandler.cs`
2. Use `_trackRepository.GetTable()`
3. Chain `.Where(t => t.IsActive)` to filter only active tracks
4. Chain `.Include(t => t.Enrollments)` then `.ToListAsync(cancellationToken)`
5. Map: `tracks.Select(t => t.ToDto().ToSummaryViewModel())`
6. Run the app → Test via Swagger `GET /api/Track/active`

---

### Task 5: `GetEnrollmentsByInternQuery` → Get All Enrollments for a Specific Intern

| | |
|---|---|
| **📁 Handler File** | `Features/Enrollments/Handlers/GetEnrollmentsByInternQueryHandler.cs` |
| **🎯 Business Goal** | Admin needs to view the enrollment history of a particular intern (what tracks they enrolled in, enrollment dates, statuses). |
| **📥 Input** | `int InternId` |
| **📤 Expected Output** | `IEnumerable<EnrollmentViewModel>` |
| **🔗 Swagger Endpoint** | `GET /api/Enrollment/intern/{internId}` |

**Steps:**
1. Open `GetEnrollmentsByInternQueryHandler.cs`
2. Use `_enrollmentRepository.GetTable()`
3. Chain `.Include(e => e.Track).Include(e => e.Intern)` for navigation properties
4. Chain `.Where(e => e.InternId == request.InternId)`
5. Chain `.ToListAsync(cancellationToken)`
6. Map: `enrollments.Select(e => e.ToDto().ToViewModel())`
7. Run the app → Test via Swagger `GET /api/Enrollment/intern/1`

---

### Task 6: `GetPaymentByIdQuery` → Get a Single Payment by ID

| | |
|---|---|
| **📁 Handler File** | `Features/Payments/Handlers/GetPaymentByIdQueryHandler.cs` |
| **🎯 Business Goal** | Finance team needs to look up a specific payment record by its ID to verify transaction details. |
| **📥 Input** | `int Id` |
| **📤 Expected Output** | `PaymentViewModel?` (or `null` if not found) |
| **🔗 Swagger Endpoint** | `GET /api/Payment/{id}` |

**Steps:**
1. Open `GetPaymentByIdQueryHandler.cs`
2. Use `_paymentRepository.GetByIdAsync(request.Id)` to find the payment
3. Map: `payment?.ToDto().ToViewModel()`
4. Return the result (return `null` if not found)
5. Run the app → Test via Swagger `GET /api/Payment/1`

---

### Task 7: `GetPendingPaymentsQuery` → List All Pending Payments

| | |
|---|---|
| **📁 Handler File** | `Features/Payments/Handlers/GetPendingPaymentsQueryHandler.cs` |
| **🎯 Business Goal** | Finance team needs a dashboard view of all payments that haven't been completed yet, so they can follow up. |
| **📥 Input** | None (parameterless query) |
| **📤 Expected Output** | `IEnumerable<PaymentViewModel>` |
| **🔗 Swagger Endpoint** | `GET /api/Payment/pending` |

**Steps:**
1. Open `GetPendingPaymentsQueryHandler.cs`
2. Use `_paymentRepository.GetTable()`
3. Chain `.Where(p => p.Status == PaymentStatus.Pending)` — you'll need `using LMS___Mini_Version.Domain.Enums;`
4. Chain `.Include(p => p.Enrollment)` then `.ToListAsync(cancellationToken)`
5. Map: `payments.Select(p => p.ToDto().ToViewModel())`
6. Run the app → Test via Swagger `GET /api/Payment/pending`

---

## ✅ How to Verify Your Work

1. **Run the application**: `dotnet run` from the project root
2. **Open Swagger**: Navigate to `https://localhost:{port}/swagger`
3. **Test each endpoint**: 
   - ❌ **Before your fix**: The endpoint returns **500 Internal Server Error** (NotImplementedException)
   - ✅ **After your fix**: The endpoint returns **200 OK** with the correct JSON data

## 📚 Key Files to Reference

| File | Purpose |
|---|---|
| `Domain/Entities/` | Entity classes (Track, Intern, Enrollment, Payment) |
| `Domain/Repositories/IGeneralRepository.cs` | Repository interface (`GetTable()`, `GetByIdAsync()`) |
| `Mapping/MappingExtensions.cs` | All mapping methods (`.ToDto()`, `.ToViewModel()`, etc.) |
| `ViewModels/` | Output shapes returned to the client |
| `Domain/Enums/PaymentStatus.cs` | Payment status enum (Pending, Completed, Failed, Refunded) |

## 🚫 Rules

- **DO NOT** modify the Query records — they are already correct
- **DO NOT** modify the Controllers — they are already wired to MediatR
- **ONLY** implement the `Handle()` method inside the Handler files
- Use `async/await` and `ConfigureAwait(false)` for all DB calls
- Pass `cancellationToken` to all async EF Core methods

Good luck! 🚀
