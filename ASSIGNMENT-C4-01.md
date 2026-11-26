# Cycle 4 — Assignment 01: Business-Driven CQRS Practice

**Student:** Abdallah Samir · **Branch:** `c4/Abdallah-Samir` (from `starter-prep`) · **Completed tasks:** 01, 02, 03, 04, 05, 06, 07, 08

## Endpoints

| Task | Endpoint | Request | Folder |
|---|---|---|---|
| 01 | `POST /api/Intern` | Command | `features/InternFeature/AddIntern` |
| 02 | `GET /api/Intern` | Query | `features/InternFeature/GetAllInterns` |
| 03 | `GET /api/Enrollment/intern/{internId}` | Query | `features/EnrollmentFeature/GetInternEnrollments` |
| 04 | `POST /api/Enrollment/{id}/complete` | Command | `features/EnrollmentFeature/CompleteEnrollment` |
| 05 | `DELETE /api/Intern/{id}` | Command | `features/InternFeature/DeleteIntern` |
| 06 | `GET /api/Track/{id}/availability` | Query | `features/TrackFeature/GetTrackAvailability` |
| 07 | `GET /api/Enrollment/track/{trackId}` | Query | `features/EnrollmentFeature/GetTrackEnrollments` |
| 08 | `POST /api/Intern/{internId}/approve-and-register` | Command | `features/InternFeature/ApproveAndRegisterIntern` |

Task 08 body: `{ "paymentMethod": "Cash" | "CreditCard" | "BankTransfer" | "Online" }`

## Responses

All endpoints return the existing `EndpointResponse` envelope.
`200/201` success · `400` invalid input · `404` missing resource · `409` rejected business rule.
Each error names the field and the reason, e.g. `{"Email": ["An intern with the email ... is already registered."]}`.
Enums are serialized as names (`"Active"`, `"Completed"`) via `JsonStringEnumConverter`.

## Design decisions

- **Business rules live in handlers**; controllers only map the request to a command/query and the result to HTTP.
- **Reads project in SQL** (`Select` into DTOs through navigation properties) — no `Include`, no entities returned.
  Task 07 uses the optional `Payment` navigation, which EF translates to a `LEFT JOIN`, so missing payments come back as `null`.
- **Seat rule in one place:** `Domain/Rules/SeatReservationRules` (Pending/Active reserve a seat; available = `max(0, capacity − reserved)`),
  used by both Task 06 and Task 08 so they can never disagree.
- **Task 03/07 read the track from the enrollment** (`Enrollment.TrackId` / `e.Track.Name`), not from the intern's current track.
- **Task 04 changes only `Enrollment.Status`.** The payment status is projected (not tracked), and `Repository.Update()` is
  not called because it would mark the whole graph as modified.
- **Task 05 checks enrollments explicitly** and returns a clear 409 instead of relying on the FK restriction to throw.
- **Task 08 is atomic:** intern status change + new enrollment + its payment are staged in the change tracker and committed by a
  single `IUnitOfWork.SaveChangesAsync()`, which EF Core runs in one transaction. A `DbUpdateException` returns 409 with nothing saved.
- Intern statuses are constants in `Domain/Constants/InternStatuses` (the column is a string).
- `CancellationToken` is passed to every EF async call; `IUnitOfWork.SaveChangesAsync()` has no token parameter, so it was left unchanged.

## Verification (run against a fresh LocalDB copy of the seed data)

- [x] Create intern in an active track → 201 with id; it appears in the list with its track name.
- [x] Same email with different casing → 409. Inactive track → 409. Unknown track → 404. Invalid input → 400.
- [x] History: intern with no enrollments → `[]`; intern with an older cancelled enrollment in another track shows that track's name, newest first; unknown intern → 404.
- [x] Complete Active + Completed payment → 200 (intern and payment columns identical before and after). Already completed / Cancelled / Pending / payment Failed / no payment → 409. Unknown → 404.
- [x] Delete intern with enrollments → 409, enrollments and payments unchanged; intern without enrollments → 200.
- [x] Availability for track 1: reserved 2 → 1 after completing an enrollment → 2 after a new registration. Capacity below reserved count → `AvailableSeats = 0`.
- [x] Approve: inactive track, already accepted intern, full track, duplicate open enrollment → 409, with no changes saved.
- [x] Successful approval creates matching Intern (Accepted), Enrollment (Active) and Payment (Completed, amount = track fees).
- [x] Forced database failure on the payment insert → 409; intern still Pending, no enrollment or payment saved. Retry afterwards succeeds.

## Known limitation

Two approvals racing for the very last seat could both pass the capacity check. Fixing that needs a serializable transaction
or a database-level guard, which is beyond the existing `IUnitOfWork` abstraction and this assignment's scope.
