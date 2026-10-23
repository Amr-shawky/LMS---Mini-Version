# LMS Mini — Backend Refactoring & Messaging Assignment

- **Format:** Individual assignment
- **Assignment date:** Saturday, **October 24, 2026**
- **Time limit:** **10 calendar days** (not two weeks)
- **Submission deadline:** **Tuesday, November 3, 2026, at 11:59 PM — Cairo, Egypt local time**
- **Stack:** ASP.NET Core (.NET 8), EF Core, SQL Server, MediatR/CQRS, Minimal APIs, RabbitMQ, MassTransit **v8**
- **Repository:** https://github.com/Amr-shawky/LMS---Mini-Version
- **Required starting branch:** [`assignment/enrollment-refactoring-starter`](https://github.com/Amr-shawky/LMS---Mini-Version/tree/assignment/enrollment-refactoring-starter)

### Important — next project eligibility and team placement

**Your performance on this assignment is an important factor, but not the only factor, in assessing your eligibility to join the next project and determining which team you will be assigned to.** Your completed work, implementation quality, understanding of the covered topics, and submission by the stated deadline will be considered. Completing the assignment does not automatically guarantee a particular team or a place on the next project.

## 1. Goal

Refactor the existing **Enrollment module** into a clear **Vertical Slice / feature-first structure**, while preserving its business behavior and integrating reliable asynchronous messaging. This is a **refactoring assignment on an existing project**, not a new application.

Your implementation must demonstrate **Minimal API, CQRS + MediatR, the existing modified Unit of Work (UOF), Generic Repository, SaveInclude, RabbitMQ + MassTransit, retry policies, and an EF Core Transactional Bus Outbox**.

Organize the implementation around business features, not just around folders named `Commands` or `Handlers`. You may choose a sensible feature-first or hybrid organization that retains the existing Domain and Infrastructure boundaries. **Do not create abstractions simply to increase the number of layers.**

## 2. Start from the correct branch

1. Clone the repository and check out **`assignment/enrollment-refactoring-starter`**.
2. Create **your own working branch** from that starter, for example `assignment/enrollment-<your-name>`.
3. Perform all implementation work on your branch and open a **Pull Request targeting `assignment/enrollment-refactoring-starter`**, not `main` or `masstransit`.
4. Do not copy completed implementations from the repository's answer/reference branches.

The starter deliberately removes five Enrollment Minimal API endpoint implementations and three handlers:

- `TransferEnrollmentOrchestratorHandler`
- `updateEnrollmentTrackCommandHandler`
- `refundPaymentCommandHandler`

The project also deliberately leaves **`GeneralRepository<T>.SaveInclude(...)`** unimplemented. The existing UOF implementation and response wrappers are provided. The remaining **GetEnrollmentsByIntern** endpoint is outside the five required endpoints: **keep it working**, even if you relocate its files during refactoring.

> **Starter note:** An existing Track update also calls `SaveInclude`; it will not function until you implement that shared method. Restoring `SaveInclude` is therefore essential even though further Track refactoring is optional.

## 3. Required Minimal API endpoints — exactly five in scope

Rebuild these five operations using **Minimal APIs** (you may keep the existing route shapes):

| Operation | Existing route shape | Main responsibility |
|---|---|---|
| List enrollments | `GET /api/Enrollment/v2` | Paginated query returning DTOs |
| Get enrollment | `GET /api/Enrollment/v2/{id}` | Single-record DTO query |
| Enroll intern | `POST /api/Enrollment/v2` | Validate and create Enrollment; create Payment where applicable |
| Cancel enrollment | `POST /api/Enrollment/v2/{id}/cancel` | Update status and handle eligible refunds synchronously |
| Transfer enrollment | `POST /api/Enrollment/v2/{id}/transfer/{newTrackId}` | Validate, update Enrollment, and enqueue an asynchronous transfer event |

**No delete endpoint is required.** Preserve existing relevant validation: enrollment/intern/track existence, cancellation state, track active state, duplicate/same-track checks, capacity checks, and applicable payment rules. Keep existing unrelated endpoints functional.

### Business rules for Transfer and Refund

- The **Transfer** request must validate the existing enrollment, destination track, and capacity before completing the change.
- Refund eligibility must be determined from the **original Enrollment's Payment**, **not** from the destination track's fee. A refundable payment is one with `PaymentStatus.Completed`. Do not attempt to refund a missing, pending, failed, or already refunded payment.
- Transferring an enrollment must commit the enrollment update and the **outbox event record in the same SQL transaction**. The HTTP response confirms the **transfer**, not completion of the asynchronous refund.
- The **Refund Consumer** must process the eligible payment later. No new `RefundPending` status is required for this assignment.
- **Cancel Enrollment** may execute its eligible refund **synchronously inside its own UOF transaction**; it does **not** need RabbitMQ. Both paths should use consistent refund eligibility rules and record refund history when a refund actually happens.

## 4. CQRS, MediatR, response contracts, and dependency rules

Apply the following contracts to the required features:

| Situation | Required MediatR result |
|---|---|
| Query returning one item | `IRequest<RequestResponse<SomeDto>>` |
| Query returning multiple items | `IRequest<RequestResponse<PaginatedResult<SomeDto>>>` |
| Command returning created ID | `IRequest<RequestResponse<int>>` |
| Other write commands | `IRequest<RequestResponse<bool>>`, `IRequest<RequestResponse<int>>`, or `IRequest<RequestResponse>`, as appropriate |
| Orchestrator | An `IRequest<RequestResponse<...>>` with the result type that fits its business workflow |

- **CreateEnrollment must return the newly created Enrollment ID** (`RequestResponse<int>`), not a full Enrollment DTO.
- Query handlers return **DTOs**, not EF Core entities. List queries must support pagination and return `PaginatedResult<TDto>`.
- The **Minimal API layer** returns `EndpointResponse` (typed when appropriate), never a raw `RequestResponse` or EF entity. For example:

```csharp
return Results.Ok(
    EndpointResponse<PaginatedResult<EnrollmentViewModel>>.Ok(paginatedResult));
```

- For create/post operations, return an `EndpointResponse` such as `EndpointResponse<int>.Created(enrollmentId)`; **do not return a full Enrollment DTO**.
- A **Query/Command Handler accesses its entity data via one directly injected `IGeneralRepository<TEntity>`**. **Do not retrieve repositories through `IUnitOfWork`**. Other legitimate dependencies (such as `IUnitOfWork`, `IPublishEndpoint`, or a logger) may be injected where required; this does not authorize injecting additional repositories for the same handler.
- An **Orchestrator coordinates multiple steps through `IMediator`**; it must not query data through an injected repository or DbContext. It **may inject `IUnitOfWork`, caching, and logging** where they serve the workflow.
- **At least one orchestrated workflow** must remain clearly identifiable. Do not move all business workflow steps directly into endpoint delegates.

## 5. Modified UOF and SaveInclude

### Existing UOF — use, don't rewrite

Use the **existing** `IUnitOfWork` / `UnitOfWork` implementation and its:

- `ExecuteAsync(...)` transaction boundary and depth tracking;
- `AddSavePointAsync(name)`;
- `RollbackToSavePointAsync(name)`.

**Do not redesign or fix the UOF as part of this assignment.** Nested `ExecuteAsync` calls are allowed where sensible but not required solely to make the depth counter increment. Keep the chosen transaction scope appropriate to each business operation.

### SaveInclude — implement the provided signature

Implement the method already declared by the existing Generic Repository:

```csharp
public void SaveInclude(T entity, params string[] Includedproperties)
```

Requirements:

- Use the **selective update** approach discussed in the session: mark only the specified scalar properties as modified rather than updating the whole entity.
- Keep the method compatible with the current EF Core tracking context and repository contract.
- Use it in **at least one required update operation**, preferably the **Enrollment Track ID update** as part of Transfer Enrollment.
- Complete the actual persistence step through the existing repository/UOF flow where appropriate.
- **One-step update** means no unnecessary load-and-replace flow merely to update selected fields; it does **not** mean SQL Server must emit exactly one statement for the entire multi-step transaction.

**Recommended / optional:** refactor other updates, including `UpdateTrack`, to use this method consistently. The starter's existing `UpdateTrack` already depends on `SaveInclude` and should become functional once your implementation is provided.

## 6. RabbitMQ + MassTransit v8 (mandatory)

Integrate MassTransit **version 8** into the same LMS application (a separate microservice is **not** required).

- RabbitMQ **virtual host must be exactly `Elevate`**, not `/`.
- Create/configure that virtual host and grant permissions to the connecting RabbitMQ user.
- Publish an `EnrollmentTransferred` message/event as part of the successful transfer workflow.
- Configure a **Fanout** publish scenario delivering the same event to **two independent receive queues and consumers**:
  - **Refund consumer**: uses `IMediator` to invoke the applicable refund Command and persists its result.
  - **Notification consumer**: observes/logs the transfer with enough identifying information; no external email/SMS provider is required.
- The two consumers must not compete for messages on the same queue: each should receive the event independently.
- The API must **not wait for a consumer response** to complete the Transfer request.
- Do not put local passwords or secret connection strings into Git.

**Retry (mandatory):** configure and demonstrate at least one MassTransit retry strategy suitable for a transient consumer failure: **Immediate**, **Interval**, or **Exponential/interval-based backoff**. The retry must be attached to the appropriate consumer/receive endpoint. A configurable temporary exception or a similarly controlled manual scenario is sufficient to verify retries; do not introduce permanent failures into the submitted solution.

**Optional bonus:** demonstrate a **Direct exchange + routing key** scenario separate from the mandatory Fanout flow.

## 7. EF Core Transactional Outbox (mandatory)

Use the **MassTransit Entity Framework Core Transactional Bus Outbox** (not only an in-memory outbox):

- Register the SQL Server EF Core outbox for the application's `AppDbContext`, including the relevant outbox tables/entities and a migration.
- Configure the **Bus Outbox** so publishing from the application scope is persisted with the Enrollment update **in the same transaction**.
- After commit, MassTransit's delivery service should forward the stored event to RabbitMQ independently.
- A failed database transaction must **not** publish a successful transfer event.
- A temporary RabbitMQ outage must **not** require rolling back an already committed transfer solely because the broker is unavailable; the stored outbox event should remain eligible for later delivery.

The **Consumer Outbox** and extra duplicate-delivery protections are optional; they do not replace the required Bus Outbox. Be mindful that broker delivery/retries can result in repeated messages, and avoid repeated refund changes where practical.

## 8. RefundHistory, Consumer UOF, and Savepoint

Create a **`RefundHistory` entity inheriting from the existing `baseEntity`**. Suggested fields are `PaymentId`, `EnrollmentId`, `PreviousStatus`, `NewStatus`, and `CreatedAt`; add appropriate EF Core mapping/relationship and a migration. You may add fields if justified by the business case.

For each **successful eligible refund**:

1. Change the original payment's status to `Refunded` through an appropriate Command.
2. Persist a corresponding `RefundHistory` record through a Command.
3. Treat both writes as **one atomic business outcome**: if history creation fails, the refund must **not** be committed as successful.

The **Refund Consumer** may inject `IMediator`, `IUnitOfWork`, and logging. **It must not access the repository directly**: dispatch Command(s) with `IMediator` and use UOF to manage the transaction.

### Savepoint task

Use **at least one real savepoint** in an appropriate multi-step transaction and explain its purpose briefly in your PR summary. **Recommended location:** inside the Refund Consumer's UOF workflow, after the refund Command and before an additional write such as creating `RefundHistory`.

If mandatory RefundHistory persistence fails, simply rolling back to a savepoint and swallowing the error is **not acceptable**. The consumer transaction must ultimately roll back, and the failure should propagate for the configured MassTransit retry. You may place the savepoint elsewhere if that fits your design and the session's approach better; explain why.

## 9. Testing and verification

**You must run the project and manually test the implemented behavior before submitting. Automated unit/integration tests are not required.** For refund scenarios, prepare a test Enrollment whose Payment status is `Completed` (new enrollments may initially create `Pending` payments, which must not be refunded). Verify at minimum:

- The five required endpoints are reachable and return the agreed response wrappers.
- Single-item and paginated reads return correct DTO data.
- Create Enrollment returns the created ID and respects its validations.
- Cancel and Transfer enforce the relevant business rules.
- A valid Transfer updates the Enrollment and results in one event being delivered to **both** Fanout consumers.
- Eligible refunds update Payment and write RefundHistory; non-eligible refunds do not incorrectly change Payment.
- A controlled transient consumer failure triggers the selected retry policy.
- Outbox records are persisted transactionally and delivered after commit; a rolled-back transfer does not produce a successful event.
- SaveInclude updates the intended column(s) without a full-entity overwrite.
- Any remaining existing route, including `GetEnrollmentsByIntern`, still works.

### Optional bonus: SQL Server Profiler

Open **SQL Server Profiler**, perform a real read or selective update, and include the **actual SQL command captured during execution** in your PR description or README. Do **not** substitute a guessed SQL statement, a LINQ expression, or only `ToQueryString()`. No execution plan or screenshots are required. Briefly mention which API action generated the SQL.

## 10. Mandatory vs. recommended/bonus

| Mandatory | Recommended / bonus |
|---|---|
| Five Enrollment Minimal API operations | Remove the legacy Enrollment Controller if replacement is correct and behavior remains intact |
| Enrollment feature-first / Vertical Slicing refactor | Apply SaveInclude to further updates beyond the required one |
| CQRS, MediatR, correct DTO and response contracts | Add sensible logging or caching where helpful |
| One Generic Repository injected directly per Query/Command Handler | Nested UOF calls if they genuinely fit your transaction flow |
| Use existing modified UOF and at least one meaningful savepoint | Direct exchange + routing keys |
| Implement and use SaveInclude at least once | Consumer Outbox / additional idempotency protections |
| RabbitMQ `Elevate` vhost and MassTransit v8 | Actual SQL captured with SQL Server Profiler |
| One Fanout event, two queues, two Consumers | Remove duplication/improve other modules only when safe |
| At least one configured and demonstrated retry policy | Additional automated tests |
| Transactional **EF Core Bus Outbox** | |
| RefundHistory inheriting `baseEntity`, atomic refund/history | |
| Manual verification and PR submission | |

## 11. Submission instructions

1. Push your implementation to **your own branch**.
2. Open a **Pull Request targeting `assignment/enrollment-refactoring-starter`**.
3. Send the mentor **only the Pull Request URL** as your submission link.
4. Include a **short implementation summary in your README** (or the PR description): what was refactored, messaging/outbox setup, the chosen retry strategy, and why your savepoint is in its chosen location. No long report, screenshots, or test document is required.
5. Confirm that the project builds and that you performed the manual checks above before submission.

### Already implemented these topics in a previous project?

If you have **already applied the assignment requirements in a previous project**, contact **me directly as your mentor** before the deadline. Share the previous project repository or PR link and point out where you implemented the relevant concepts. I will personally review the work, verify that you actually implemented and understand the requirements, and decide whether to **approve that previous work as your assignment submission**. **Previous work is not automatically accepted without mentor review and approval.**

**Final deadline: Tuesday, November 3, 2026, 11:59 PM (Cairo, Egypt local time).** This is **10 calendar days from October 24, 2026**, not two weeks. Focus on correct behavior, clear responsibilities, and a working integrated flow rather than unnecessary abstractions.

---

### Useful documentation

- MassTransit Outbox: https://masstransit.io/documentation/configuration/middleware/outbox
- MassTransit RabbitMQ: https://masstransit.io/documentation/configuration/transports/rabbitmq
- Microsoft EF Core Transactions / Savepoints: https://learn.microsoft.com/en-us/ef/core/saving/transactions
