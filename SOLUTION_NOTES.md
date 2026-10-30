# LMS Mini — Mentor Reference Solution

> **Mentor-only reference.** This branch implements the assignment from `assignment/enrollment-refactoring-starter`. Do not merge it into the students' starter branch.

## Implemented

- Five Enrollment Minimal API operations: list (paged), get by ID, create, cancel and transfer. Existing `GetEnrollmentsByIntern` remains available.
- Vertical Slice organization: `Feature/EnrollmentSlices/{Read,Enroll,Cancel,Transfer,Refund}`.
- CQRS and MediatR orchestrators, DTO query responses and EndpointResponse HTTP wrappers.
- Direct `IGeneralRepository<TEntity>` injection in each query/command handler; no repository resolution from UnitOfWork.
- Implemented selective `SaveInclude`, including pre-tracked entity handling; used in Enrollment transfer/status and refund.
- Existing `UnitOfWork` unchanged; refund consumer demonstrates `AddSavePointAsync` and `RollbackToSavePointAsync`. A refund and its mandatory history are atomic.
- Transfer checks enrollment state, target track activity and available capacity; refund eligibility checks the **original Payment** (Completed).
- Two independent RabbitMQ queues for the `EnrollmentTransferred` fanout event: refund and notification consumers.
- MassTransit **8.3.6** (compatible with the existing EF Core 8 packages), retry policies, SQL Server Entity Framework **Bus Outbox**.
- `RefundHistory : baseEntity`, a unique PaymentId index and EF migration for RefundHistory + MassTransit outbox tables.
- Removed legacy EnrollmentController; its five operations are now served by Minimal APIs.

## Setup

1. Use SQL Server and **a new database** (the previous `EnsureCreated` databases are not compatible with automatic migration history). Runtime connection string comes from `ConnectionStrings__DefaultConnection` or appsettings.
2. Apply EF migrations (or allow `DbInitializer.Seed` to call `Database.Migrate()` on application startup).
3. Set up RabbitMQ and create the **`Elevate`** virtual host; grant your RabbitMQ user permissions there.
4. Provide credentials via environment variables or user secrets; never commit passwords:
   - `RabbitMq__Host` (optional, defaults to `localhost`)
   - `RabbitMq__Username` (required)
   - `RabbitMq__Password` (required)
5. Run `dotnet restore`, `dotnet build`, then `dotnet run`.

Example RabbitMQ administration commands (run with RabbitMQ CLI and substitute your actual username):

```sh
rabbitmqctl add_vhost Elevate
rabbitmqctl set_permissions -p Elevate YOUR_RABBIT_USER ".*" ".*" ".*"
```

## Main flow

```text
POST /api/Enrollment/v2/{id}/transfer/{newTrackId}
  -> TransferEnrollmentOrchestratorHandler (IMediator + IUnitOfWork)
  -> updateEnrollmentTrackCommandHandler (one Generic Repository + IPublishEndpoint)
  -> SaveInclude(TrackId) + publish EnrollmentTransferred (Bus Outbox)
  -> SQL transaction commits Enrollment update and OutboxMessage
  -> MassTransit delivery agent publishes to RabbitMQ (vhost Elevate)
     -> refund queue -> RefundEnrollmentTransferredConsumer
        -> refundPaymentCommand -> SQL savepoint
        -> CreateRefundHistoryCommand -> commit, or rollback and retry on error
     -> notification queue -> TransferNotificationConsumer
```

Cancellation performs its eligible refund synchronously within its own UOF transaction and records RefundHistory. Ineligible (non-Completed) original payments are not refunded.

## Manual verification

Verified on a disposable local SQL Server database:

- `dotnet build`: succeeded.
- EF Core migration `AddRefundHistoryAndBusOutbox`: applied successfully.
- GET list / GET by ID / GET by intern: responded with DTO wrappers.
- POST enrollment: returned the new Enrollment ID.
- POST cancel of a completed-payment enrollment: Payment became `Refunded` and one RefundHistory was written.
- POST transfer: TrackId changed, while OutboxMessage and OutboxState were committed.

**Not verified end-to-end in this environment:** broker delivery to both RabbitMQ consumers, live consumer retries and their resulting refund after transfer. RabbitMQ was not running during verification. Start a broker with `Elevate`, then verify queue bindings, independent deliveries and retry behavior before using this branch as a fully integration-tested reference.

## Notes

- The SQL Server outbox persists the message before asynchronous broker delivery. Do not use in-memory outbox as a substitute.
- Savepoints do not turn a mandatory failed RefundHistory into a successful refund: on failure the consumer throws and outer UOF rolls back.
- The old UOF implementation intentionally remains unchanged as requested.
- Because the repository is **public**, any solution branch pushed here is visible to students. Consider moving the mentor answer to a private repository if you need to keep it confidential until grading is finished.
