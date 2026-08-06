# 🎯 CQRS Practice Assignment V2 — Answer Key

> **Instructor Note:** This file contains the complete solutions for all 10 tasks in the CQRS V2 Assignment. You can provide this to interns after the assignment, or use it as a grading rubric.

---

## 📋 Section A: Queries (Tasks 1–3)

### Task 1: `GetTrackByIdQuery`

**1. Handler** (`Features/Tracks/Handlers/GetTrackByIdQueryHandler.cs`)
```csharp
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Tracks.Queries;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.Features.Tracks.Handlers
{
    public class GetTrackByIdQueryHandler 
        : IRequestHandler<GetTrackByIdQuery, TrackDto?>
    {
        private readonly IGeneralRepository<Track> _trackRepository;

        public GetTrackByIdQueryHandler(IGeneralRepository<Track> trackRepository)
        {
            _trackRepository = trackRepository;
        }

        public async Task<TrackDto?> Handle(
            GetTrackByIdQuery request, CancellationToken cancellationToken)
        {
            var track = await _trackRepository.GetByIdAsync(request.Id);
            return track?.ToDto();
        }
    }
}
```

**2. Controller Wiring** (`TrackController.cs`)
```csharp
[HttpGet("{id}")]
public async Task<ActionResult> GetById(int id)
{
    var track = await _mediator.Send(new GetTrackByIdQuery(id));
    if (track == null) return NotFound();
    return Ok(track);
}
```

---

### Task 2: `GetAllInternsQuery`

**1. Query Record** (`Features/Interns/Queries/GetAllInternsQuery.cs`)
```csharp
using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Queries
{
    public record GetAllInternsQuery : IRequest<IEnumerable<InternDto>>;
}
```

**2. Handler** (`Features/Interns/Handlers/GetAllInternsQueryHandler.cs`)
```csharp
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Interns.Queries;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Interns.Handlers
{
    public class GetAllInternsQueryHandler 
        : IRequestHandler<GetAllInternsQuery, IEnumerable<InternDto>>
    {
        private readonly IGeneralRepository<Intern> _internRepository;

        public GetAllInternsQueryHandler(IGeneralRepository<Intern> internRepository)
        {
            _internRepository = internRepository;
        }

        public async Task<IEnumerable<InternDto>> Handle(
            GetAllInternsQuery request, CancellationToken cancellationToken)
        {
            var interns = await _internRepository.GetTable()
                .Include(i => i.Track)
                .ToListAsync(cancellationToken);
                
            return interns.Select(i => i.ToDto());
        }
    }
}
```

**3. Controller Wiring** (`InternController.cs`)
```csharp
[HttpGet]
public async Task<ActionResult> GetAll()
{
    var result = await _mediator.Send(new GetAllInternsQuery());
    return Ok(result);
}
```

---

### Task 3: `GetInternByIdQuery`

**1. Query Record** (`Features/Interns/Queries/GetInternByIdQuery.cs`)
```csharp
using LMS___Mini_Version.DTOs;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Queries
{
    public record GetInternByIdQuery(int Id) : IRequest<InternDto?>;
}
```

**2. Handler** (`Features/Interns/Handlers/GetInternByIdQueryHandler.cs`)
```csharp
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Interns.Queries;
using LMS___Mini_Version.Mapping;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LMS___Mini_Version.Features.Interns.Handlers
{
    public class GetInternByIdQueryHandler 
        : IRequestHandler<GetInternByIdQuery, InternDto?>
    {
        private readonly IGeneralRepository<Intern> _internRepository;

        public GetInternByIdQueryHandler(IGeneralRepository<Intern> internRepository)
        {
            _internRepository = internRepository;
        }

        public async Task<InternDto?> Handle(
            GetInternByIdQuery request, CancellationToken cancellationToken)
        {
            var intern = await _internRepository.GetTable()
                .Include(i => i.Track)
                .FirstOrDefaultAsync(i => i.Id == request.Id, cancellationToken);
                
            return intern?.ToDto();
        }
    }
}
```

**3. Controller Wiring** (`InternController.cs`)
```csharp
[HttpGet("{id}")]
public async Task<ActionResult> GetById(int id)
{
    var intern = await _mediator.Send(new GetInternByIdQuery(id));
    if (intern == null) return NotFound();
    return Ok(intern);
}
```

---

## 📋 Section B: Standalone Commands (Tasks 4–7)

### Task 4: `CreateTrackCommand`

**1. Command Record**
```csharp
using LMS___Mini_Version.ViewModels.Track;
using MediatR;

namespace LMS___Mini_Version.Features.Tracks.Commands
{
    public record CreateTrackCommand(
        string Name,
        decimal Fees,
        bool IsActive,
        int MaxCapacity
    ) : IRequest<TrackSummaryViewModel>;
}
```

**2. Handler**
```csharp
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Tracks.Commands;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Track;
using MediatR;

namespace LMS___Mini_Version.Features.Tracks.Handlers
{
    public class CreateTrackCommandHandler 
        : IRequestHandler<CreateTrackCommand, TrackSummaryViewModel>
    {
        private readonly IGeneralRepository<Track> _trackRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateTrackCommandHandler(
            IGeneralRepository<Track> trackRepository,
            IUnitOfWork unitOfWork)
        {
            _trackRepository = trackRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TrackSummaryViewModel> Handle(
            CreateTrackCommand request, CancellationToken cancellationToken)
        {
            var entity = new Track
            {
                Name = request.Name,
                Fees = request.Fees,
                IsActive = request.IsActive,
                MaxCapacity = request.MaxCapacity
            };

            _trackRepository.Add(entity);
            await _unitOfWork.CompleteAsync(); // Atomic commit!

            return entity.ToDto().ToSummaryViewModel();
        }
    }
}
```

**3. Controller Wiring**
```csharp
[HttpPost]
public async Task<ActionResult<TrackSummaryViewModel>> Create(CreateTrackViewModel vm)
{
    var result = await _mediator.Send(new CreateTrackCommand(
        vm.Name, vm.Fees, vm.IsActive, vm.MaxCapacity
    ));

    return Ok(result);
}
```

---

### Task 5: `DeleteTrackCommand`

**1. Command Record**
```csharp
using MediatR;
namespace LMS___Mini_Version.Features.Tracks.Commands
{
    public record DeleteTrackCommand(int Id) : IRequest<bool>;
}
```

**2. Handler**
```csharp
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Tracks.Commands;
using MediatR;

namespace LMS___Mini_Version.Features.Tracks.Handlers
{
    public class DeleteTrackCommandHandler : IRequestHandler<DeleteTrackCommand, bool>
    {
        private readonly IGeneralRepository<Track> _trackRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTrackCommandHandler(IGeneralRepository<Track> trackRepository, IUnitOfWork unitOfWork)
        {
            _trackRepository = trackRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteTrackCommand request, CancellationToken cancellationToken)
        {
            var track = await _trackRepository.GetByIdAsync(request.Id);
            if (track == null) return false;

            _trackRepository.Delete(track);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
```

**3. Controller Wiring**
```csharp
[HttpDelete("{id}")]
public async Task<ActionResult> Delete(int id)
{
    var deleted = await _mediator.Send(new DeleteTrackCommand(id));
    if (!deleted) return NotFound();
    return NoContent();
}
```

---

### Task 6: `CreateInternCommand`

**1. Command Record**
```csharp
using LMS___Mini_Version.ViewModels.Intern;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Commands
{
    public record CreateInternCommand(
        string FullName,
        string Email,
        int BirthYear,
        string Status,
        int TrackId
    ) : IRequest<InternSummaryViewModel>;
}
```

**2. Handler**
```csharp
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Interns.Commands;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Intern;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Handlers
{
    public class CreateInternCommandHandler : IRequestHandler<CreateInternCommand, InternSummaryViewModel>
    {
        private readonly IGeneralRepository<Intern> _internRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateInternCommandHandler(IGeneralRepository<Intern> internRepository, IUnitOfWork unitOfWork)
        {
            _internRepository = internRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<InternSummaryViewModel> Handle(CreateInternCommand request, CancellationToken cancellationToken)
        {
            var entity = new Intern
            {
                FullName = request.FullName,
                Email = request.Email,
                BirthYear = request.BirthYear,
                Status = request.Status,
                TrackId = request.TrackId
            };

            _internRepository.Add(entity);
            await _unitOfWork.CompleteAsync();

            return entity.ToDto().ToSummaryViewModel();
        }
    }
}
```

**3. Controller Wiring**
```csharp
[HttpPost]
public async Task<ActionResult<InternSummaryViewModel>> Create(CreateInternViewModel vm)
{
    var result = await _mediator.Send(new CreateInternCommand(
        vm.FullName, vm.Email, vm.BirthYear, vm.Status, vm.TrackId
    ));
    return Ok(result);
}
```

---

### Task 7: `DeleteInternCommand`

**1. Command Record**
```csharp
using MediatR;
namespace LMS___Mini_Version.Features.Interns.Commands
{
    public record DeleteInternCommand(int Id) : IRequest<bool>;
}
```

**2. Handler**
```csharp
using LMS___Mini_Version.Domain.Entities;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Interns.Commands;
using MediatR;

namespace LMS___Mini_Version.Features.Interns.Handlers
{
    public class DeleteInternCommandHandler : IRequestHandler<DeleteInternCommand, bool>
    {
        private readonly IGeneralRepository<Intern> _internRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteInternCommandHandler(IGeneralRepository<Intern> internRepository, IUnitOfWork unitOfWork)
        {
            _internRepository = internRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteInternCommand request, CancellationToken cancellationToken)
        {
            var intern = await _internRepository.GetByIdAsync(request.Id);
            if (intern == null) return false;

            _internRepository.Delete(intern);
            await _unitOfWork.CompleteAsync();
            return true;
        }
    }
}
```

**3. Controller Wiring**
```csharp
[HttpDelete("{id}")]
public async Task<ActionResult> Delete(int id)
{
    var deleted = await _mediator.Send(new DeleteInternCommand(id));
    if (!deleted) return NotFound();
    return NoContent();
}
```

---

## 📋 Section C: Orchestrators (Tasks 8–10)

### Task 8: `EnrollInternOrchestratorHandler`

**1. Handler Implementation**
```csharp
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Features.Common;
using LMS___Mini_Version.Features.Enrollments.Commands;
using LMS___Mini_Version.Features.Interns.Queries;
using LMS___Mini_Version.Features.Payments.Commands;
using LMS___Mini_Version.Features.Tracks.Queries;
using LMS___Mini_Version.Mapping;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Orchestrators
{
    public class EnrollInternOrchestratorHandler
        : IRequestHandler<EnrollInternOrchestratorRequest, EnrollmentResultDto>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public EnrollInternOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<EnrollmentResultDto> Handle(
            EnrollInternOrchestratorRequest request, CancellationToken cancellationToken)
        {
            // 1. Validate Intern
            var internExists = await _mediator.Send(new ValidateInternExistsQuery(request.InternId), cancellationToken);
            if (!internExists) return EnrollmentResultDto.Fail($"Intern with ID {request.InternId} not found.");

            // 2. Validate Track
            var track = await _mediator.Send(new GetTrackByIdQuery(request.TrackId), cancellationToken);
            if (track == null) return EnrollmentResultDto.Fail($"Track with ID {request.TrackId} not found.");
            if (!track.IsActive) return EnrollmentResultDto.Fail($"Track '{track.Name}' is not active.");

            // 3. Check Capacity
            var hasCapacity = await _mediator.Send(new CheckTrackCapacityQuery(request.TrackId), cancellationToken);
            if (!hasCapacity) return EnrollmentResultDto.Fail($"Track '{track.Name}' has reached maximum capacity.");

            // 4. Stage Enrollment
            var enrollment = await _mediator.Send(new StageEnrollmentCommand(request.InternId, request.TrackId), cancellationToken);

            // 5. Commit Enrollment atomic
            await _unitOfWork.CompleteAsync();

            // 6. Stage & Commit Payment
            PaymentDto? payment = null;
            if (track.Fees > 0)
            {
                var paymentEntity = await _mediator.Send(
                    new StagePaymentCommand(enrollment.Id, track.Fees, PaymentMethod.Cash), cancellationToken);
                await _unitOfWork.CompleteAsync();
                payment = paymentEntity.ToDto();
            }

            return EnrollmentResultDto.Succeed(enrollment.ToDto(), payment);
        }
    }
}
```

**2. Controller Wiring**
```csharp
[HttpPost]
public async Task<ActionResult<EnrollmentViewModel>> Enroll(EnrollInternViewModel vm)
{
    var result = await _mediator.Send(new EnrollInternOrchestratorRequest(vm.InternId, vm.TrackId));
    if (!result.IsSuccess) return BadRequest(new { error = result.ErrorMessage });
    return Ok(result.Enrollment!.ToViewModel());
}
```

---

### Task 9: `CancelEnrollmentOrchestratorHandler`

**1. Handler Implementation**
```csharp
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Common;
using LMS___Mini_Version.Features.Enrollments.Commands;
using LMS___Mini_Version.Features.Enrollments.Queries;
using LMS___Mini_Version.Features.Payments.Commands;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Orchestrators
{
    public class CancelEnrollmentOrchestratorHandler
        : IRequestHandler<CancelEnrollmentOrchestratorRequest, CommandResult>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public CancelEnrollmentOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(
            CancelEnrollmentOrchestratorRequest request, CancellationToken cancellationToken)
        {
            var enrollment = await _mediator.Send(new GetEnrollmentByIdQuery(request.EnrollmentId), cancellationToken);
            if (enrollment == null) return CommandResult.Fail($"Enrollment not found.");
            if (enrollment.Status == EnrollmentStatus.Cancelled.ToString()) return CommandResult.Fail("Enrollment is already cancelled.");

            var statusUpdated = await _mediator.Send(new StageUpdateEnrollmentStatusCommand(request.EnrollmentId, EnrollmentStatus.Cancelled), cancellationToken);
            if (!statusUpdated) return CommandResult.Fail("Failed to update status.");

            await _mediator.Send(new StageRefundPaymentCommand(request.EnrollmentId), cancellationToken);

            await _unitOfWork.CompleteAsync(); // Atomic Commit

            return CommandResult.Succeed("Enrollment cancelled and payment refunded successfully.");
        }
    }
}
```

**2. Controller Wiring**
```csharp
[HttpPost("{id}/cancel")]
public async Task<ActionResult> Cancel(int id)
{
    var result = await _mediator.Send(new CancelEnrollmentOrchestratorRequest(id));
    if (!result.IsSuccess) return BadRequest(new { error = result.Message });
    return Ok(new { message = result.Message });
}
```

---

### Task 10: `TransferEnrollmentOrchestratorHandler`

**1. Handler Implementation**
```csharp
using LMS___Mini_Version.Domain.Enums;
using LMS___Mini_Version.Domain.Repositories;
using LMS___Mini_Version.Features.Common;
using LMS___Mini_Version.Features.Enrollments.Commands;
using LMS___Mini_Version.Features.Enrollments.Queries;
using LMS___Mini_Version.Features.Payments.Commands;
using LMS___Mini_Version.Features.Tracks.Queries;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollments.Orchestrators
{
    public class TransferEnrollmentOrchestratorHandler
        : IRequestHandler<TransferEnrollmentOrchestratorRequest, CommandResult>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public TransferEnrollmentOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Handle(
            TransferEnrollmentOrchestratorRequest request, CancellationToken cancellationToken)
        {
            var enrollment = await _mediator.Send(new GetEnrollmentByIdQuery(request.EnrollmentId), cancellationToken);
            if (enrollment == null) return CommandResult.Fail($"Enrollment not found.");
            if (enrollment.Status == EnrollmentStatus.Cancelled.ToString()) return CommandResult.Fail("Cannot transfer a cancelled enrollment.");

            var newTrack = await _mediator.Send(new GetTrackByIdQuery(request.NewTrackId), cancellationToken);
            if (newTrack == null) return CommandResult.Fail($"Target Track not found.");
            if (!newTrack.IsActive) return CommandResult.Fail($"Target Track '{newTrack.Name}' is not currently active.");

            var hasCapacity = await _mediator.Send(new CheckTrackCapacityQuery(request.NewTrackId), cancellationToken);
            if (!hasCapacity) return CommandResult.Fail($"Target Track '{newTrack.Name}' has reached its maximum capacity.");

            var trackUpdated = await _mediator.Send(new StageUpdateEnrollmentTrackCommand(request.EnrollmentId, request.NewTrackId), cancellationToken);
            if (!trackUpdated) return CommandResult.Fail("Failed to update the enrollment's track.");

            if (newTrack.Fees > 0)
            {
                await _mediator.Send(new StageUpdatePaymentAmountCommand(request.EnrollmentId, newTrack.Fees), cancellationToken);
            }

            await _unitOfWork.CompleteAsync(); // Atomic Commit

            return CommandResult.Succeed($"Enrollment transferred to '{newTrack.Name}' successfully. New fees: {newTrack.Fees:C}.");
        }
    }
}
```

**2. Controller Wiring**
```csharp
[HttpPost("{id}/transfer/{newTrackId}")]
public async Task<ActionResult> Transfer(int id, int newTrackId)
{
    var result = await _mediator.Send(new TransferEnrollmentOrchestratorRequest(id, newTrackId));
    if (!result.IsSuccess) return BadRequest(new { error = result.Message });
    return Ok(new { message = result.Message });
}
```
