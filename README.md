# LMS Mini — Enrollment Refactoring & Messaging

## What was refactored

The Enrollment module was rebuilt as a set of Vertical Slice / feature-first handlers
inside `Feature/enrollmentFeature/`. Each operation is split into one of three roles:

- **Query handler** — reads data through a single `IGeneralRepository<TEntity>`, returns a DTO.
- **Command handler** — writes data through a single `IGeneralRepository<TEntity>`.
- **Orchestrator handler** — coordinates multiple queries and commands through `IMediator`;
  never touches a repository directly.

Five Minimal API v2 endpoints were added under `api/Enrollment/v2`:

| Method | Route | Handler |
|--------|-------|---------|
| GET | `/api/Enrollment/v2` | GetAllEnrollmentsQuery (paginated) |
| GET | `/api/Enrollment/v2/{id}` | GetEnrollmentByIdQuery |
| POST | `/api/Enrollment/v2` | EnrollInternOrchestrator → returns enrollment ID |
| POST | `/api/Enrollment/v2/{id}/cancel` | cancelEnrollmentOrchestrator |
| POST | `/api/Enrollment/v2/{id}/transfer/{newTrackId}` | TransferEnrollmentOrchestrator |

The legacy `EnrollmentController` and `GetEnrollmentsByIntern` endpoint were kept intact (R4).

`SaveInclude` in `GeneralRepository<T>` was implemented: it attaches a detached entity to the
EF change tracker, then marks only the named property columns as `Modified`. This produces a
targeted SQL `UPDATE` that writes only those columns instead of every column (R11).

A `RefundHistory` entity was added (R23): every successful refund records one row with
PaymentId, EnrollmentId, PreviousStatus, NewStatus, and RefundedAt.

## Messaging and Outbox setup

**MassTransit 8.3.6** is used with **RabbitMQ** on virtual host `Elevate`.

RabbitMQ must be running locally with the management plugin enabled, and virtual host `Elevate`
must exist with permissions granted to the connecting user. Example setup commands:

```bash
rabbitmqctl add_vhost Elevate
rabbitmqctl set_permissions -p Elevate <your-user> ".*" ".*" ".*"
```

Credentials are NOT committed to Git. Supply them with user-secrets:

```bash
dotnet user-secrets init
dotnet user-secrets set "RabbitMQ:Username" "your-rabbitmq-username"
dotnet user-secrets set "RabbitMQ:Password" "your-rabbitmq-password"
```

Apply the migration before running the app:

```bash
dotnet ef database update
```

The **EF Core Transactional Bus Outbox** (R22) is configured with `AddEntityFrameworkOutbox<AppDbContext>`.
When `Publish` is called inside `UnitOfWork.ExecuteAsync`, MassTransit writes the serialized
message to the `OutboxMessage` table in the same SQL transaction. If the DB transaction rolls
back, the message row is also gone — no spurious event is ever sent. After a successful commit,
MassTransit's background delivery service picks up the row and forwards it to RabbitMQ.
This means a RabbitMQ outage does not require rolling back a committed transfer; the row stays
in the outbox until the broker is reachable.

**Fanout pattern (R18):** Two separate receive endpoints consume the same `EnrollmentTransferred`
event independently:
- `lms-enrollment-refund-queue` → `RefundConsumer`
- `lms-enrollment-notification-queue` → `NotificationConsumer`

## Retry strategy

`RefundConsumer` has an interval retry configured on its receive endpoint:

```
e.UseMessageRetry(r => r.Interval(retryCount, TimeSpan.FromSeconds(intervalSeconds)));
```

Defaults (from `appsettings.json`): 3 retries, 5-second interval.
Values are configurable via `Messaging:Retry:RetryCount` and `Messaging:Retry:IntervalSeconds`.

To test retry behavior, set `Messaging:Testing:RefundConsumerFailures` to a positive integer.
The consumer will throw an injected exception for the first N deliveries, exercising the retry
path. **Default is 0** — no failures are injected in a normal run.

## Why the savepoint is where it is (R25)

Inside `RefundConsumer.Consume`, the UOF transaction contains two writes in sequence:
1. `refundPaymentCommand` — marks the Payment as Refunded.
2. `CreateRefundHistoryCommand` — records the RefundHistory row.

The savepoint is placed **after step 1, before step 2**:

```
AddSavePointAsync("after-refund")   // savepoint here
CreateRefundHistoryCommand           // step 2 -- may throw
```

If step 2 throws:
- We call `RollbackToSavePointAsync("after-refund")` to undo any partial history write.
- We then **rethrow** the exception — we do NOT swallow it.
- The rethrow causes the UOF outer `catch` to roll back the entire transaction (including the
  refund from step 1).
- MassTransit sees the exception, applies the configured retry, and redelivers the message.

This guarantees that a payment is never left in `Refunded` state without a corresponding
`RefundHistory` row (R24), and that MassTransit retries when history creation fails (R25).