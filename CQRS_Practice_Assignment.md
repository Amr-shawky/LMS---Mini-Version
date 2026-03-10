# 🎯 CQRS Practice Assignment V2 — Full CQRS Spectrum

> **Welcome, Intern!** This is your ultimate CQRS test. You will implement the complete CQRS pattern (Queries, Commands, and Orchestrators). The controllers currently throw `NotImplementedException`, waiting for your code!

---

## 🏗️ The 3 Layers of CQRS in this Architecture

1. **Queries (Reads)**: Retrieve data directly using `GetTable()` or `GetByIdAsync()`. Return DTOs.
2. **Commands (Writes - Standalone)**: Simple actions (Create/Update/Delete) that modify one entity, then immediately call `IUnitOfWork.CompleteAsync()`.
3. **Orchestrators (Complex Workflows)**: Coordinate multiple atomic *Queries* and *Stage Commands* across boundaries, and finally call ONE `IUnitOfWork.CompleteAsync()` to make the whole operation atomic.

---

## 📋 Section A: Queries (Tasks 1–3)

| Task | Query | Input | Returns | Goal |
|---|---|---|---|---|
| **1** | `GetTrackByIdQuery` | `int Id` | `TrackDto` | Admin views details of a training track. |
| **2** | `GetAllInternsQuery` | none | `IEnumerable<InternDto>` | List all registered interns with track names. |
| **3** | `GetInternByIdQuery` | `int Id` | `InternDto?` | View full details of a specific intern. |

**How to implement a Query:**
1. Create the **Query record** in `Features/{Feature}/Queries/` (e.g., `public record GetAllInternsQuery() : IRequest<IEnumerable<InternDto>>;`).
   *(Note: `GetTrackByIdQuery.cs` already exists because Orchestrators use it, but you must create its Handler).*
2. Create the **Handler class** in `Features/{Feature}/Handlers/` implementing `IRequestHandler<TQuery, TResponse>`.
3. Inside `Handle()`: Inject your repository, query the DB (e.g., `_internRepository.GetTable().Include(...)`), map using `.ToDto()`, and return the result.
4. **Wire the Controller**: Go to the corresponding controller endpoint and `return await _mediator.Send(new YourQuery(...));`.

---

## 📋 Section B: Standalone Commands (Tasks 4–7)

| Task | Command | Input | Returns | Goal |
|---|---|---|---|---|
| **4** | `CreateTrackCommand` | `Name, Fees, IsActive, MaxCapacity` | `TrackSummaryViewModel` | Create a new track. |
| **5** | `DeleteTrackCommand` | `int Id` | `bool` | Delete an existing track. |
| **6** | `CreateInternCommand` | `FullName, Email, BirthYear, Status, TrackId` | `InternSummaryViewModel` | Register a new Intern. |
| **7** | `DeleteInternCommand` | `int Id` | `bool` | Delete an Intern. |

**How to implement a Standalone Command:**
1. Create the **Command record** in `Features/{Feature}/Commands/`.
2. Create the **Handler class** in `Features/{Feature}/Handlers/`.
3. Inside `Handle()`:
   * Inject `IGeneralRepository<{Entity}>` and `IUnitOfWork`.
   * For Create: Instantiate the entity, call `_repo.Add(entity)`.
   * For Delete: Fetch by ID. If null return false. Else `_repo.Delete(entity)`.
   * **CRITICAL:** Call `await _unitOfWork.CompleteAsync();` to save to DB.
   * Return the expected result (Mapped entity or true/false).
4. **Wire the Controller**: Go to the POST/DELETE endpoint and use `_mediator.Send(new YourCommand(...));`.

---

## 📋 Section C: Orchestrators (Tasks 8–10)

> ⚡ **What is an Orchestrator?**
> An Orchestrator is the modern equivalent of the "Action Coordinator" Mediator. It ties together multiple atomic steps from different features into one unified workflow and executes a **single Atomic Commit** at the end. An Orchestrator Handler should **ONLY** inject `IMediator` (to run steps) and `IUnitOfWork` (to commit). **Do NOT inject repositories.**

*(Note: The Orchestrator Request records are already created for you in `Features/Enrollments/Orchestrators/`)*.

### Task 8: `EnrollInternOrchestratorHandler` (The Enrollment Workflow)
**Goal:** Registers an Intern to a Track, applies Fees, makes a Payment, and saves everything together natively.
**File to Create:** `Features/Enrollments/Orchestrators/EnrollInternOrchestratorHandler.cs`
**Implements:** `IRequestHandler<EnrollInternOrchestratorRequest, EnrollmentResultDto>`

**Handler Steps (Detailed):**
1. **Validate Intern exists**: `await _mediator.Send(new ValidateInternExistsQuery(request.InternId))`
   * If `false`, return `EnrollmentResultDto.Fail("Intern not found.")`
2. **Validate Track**: `await _mediator.Send(new GetTrackByIdQuery(request.TrackId))`
   * If `null` or `!track.IsActive`, return Fail.
3. **Check Capacity**: `await _mediator.Send(new CheckTrackCapacityQuery(request.TrackId))`
   * If `false`, return Fail.
4. **Stage Enrollment**: `await _mediator.Send(new StageEnrollmentCommand(request.InternId, request.TrackId))`
   * This creates the Enrollment in EF Core Change Tracker (NOT saved yet).
5. **Atomic Commit**: `await _unitOfWork.CompleteAsync()`
   * This saves the Enrollment to the DB and generates its real ID.
6. **Apply Payment (if Track has Fees > 0)**:
   * Use `await _mediator.Send(new StagePaymentCommand(enrollment.Id, track.Fees, PaymentMethod.Cash))`
   * Use `await _unitOfWork.CompleteAsync()` to save the payment.
7. **Return Success**: Return the `EnrollmentResultDto.Succeed(enrollment.ToDto(), payment?.ToDto())`
8. **Wire the Controller**: Fix `EnrollmentController.Enroll`.

### Task 9: `CancelEnrollmentOrchestratorHandler` (Cancellation Workflow)
**Goal:** Cancels an Enrollment and automatically refunds the Payment simultaneously.
**File to Create:** `Features/Enrollments/Orchestrators/CancelEnrollmentOrchestratorHandler.cs`
**Implements:** `IRequestHandler<CancelEnrollmentOrchestratorRequest, CommandResult>`

**Handler Steps (Detailed):**
1. **Fetch Enrollment**: `await _mediator.Send(new GetEnrollmentByIdQuery(request.EnrollmentId))`
   * If `null`, return `CommandResult.Fail("Not found.")`
2. **Validate Status**: Ensure the enrollment is not already cancelled (check `enrollment.Status`).
3. **Stage Status to Cancelled**: `await _mediator.Send(new StageUpdateEnrollmentStatusCommand(request.EnrollmentId, EnrollmentStatus.Cancelled))`
4. **Stage Refund**: `await _mediator.Send(new StageRefundPaymentCommand(request.EnrollmentId))`
5. **Atomic Commit**: `await _unitOfWork.CompleteAsync()`
   * Both the cancellation and refund are saved together! If one fails, both fail.
6. **Return Success**: Return `CommandResult.Succeed("Cancelled successfully.")`
7. **Wire the Controller**: Fix `EnrollmentController.Cancel`.

### Task 10: `TransferEnrollmentOrchestratorHandler` (Transfer Workflow)
**Goal:** Changes the Intern's Track and recalculates/updates the Payment amount.
**File to Create:** `Features/Enrollments/Orchestrators/TransferEnrollmentOrchestratorHandler.cs`
**Implements:** `IRequestHandler<TransferEnrollmentOrchestratorRequest, CommandResult>`

**Handler Steps (Detailed):**
1. **Fetch Enrollment**: `await _mediator.Send(new GetEnrollmentByIdQuery(request.EnrollmentId))`
   * If `null`, fail. If already cancelled, fail.
2. **Validate Target Track**: `await _mediator.Send(new GetTrackByIdQuery(request.NewTrackId))`
   * If target track is null or inactive, fail.
3. **Check Capacity**: `await _mediator.Send(new CheckTrackCapacityQuery(request.NewTrackId))`
   * If `false`, fail.
4. **Stage Track Transfer**: `await _mediator.Send(new StageUpdateEnrollmentTrackCommand(request.EnrollmentId, request.NewTrackId))`
5. **Stage Payment Adjustment**: If the new track has Fees > 0, adjust the payment:
   * `await _mediator.Send(new StageUpdatePaymentAmountCommand(request.EnrollmentId, newTrack.Fees))`
6. **Atomic Commit**: `await _unitOfWork.CompleteAsync()`
   * Transfer and payment adjustment happen together.
7. **Return Success**: Return `CommandResult.Succeed("Transferred successfully.")`
8. **Wire the Controller**: Fix `EnrollmentController.Transfer`.

---

## 🚫 Final Reminders

- **DO NOT** use `.ConfigureAwait(false)` anywhere in your code.
- Always pass `cancellationToken` down the chain.
- Use `dotnet build` frequently to catch syntax errors as you recreate the parts!
- Verify your endpoints using Swagger!

Good luck, you've got this! 🚀
