using LMS___Mini_Version.Common;
using LMS___Mini_Version.Features.Enrollment.AddEnrollment.Command;
using LMS___Mini_Version.Features.Enrollment.GetEnrollmentById.Queries;
using LMS___Mini_Version.Features.Intern.GetInternById.Query;
using LMS___Mini_Version.Features.Track.GetTrackById.Query;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollment.AddEnrollment.Orchestrator
{
    public record AddEnrollmentOrchestrator(int internId, int trackId) : IRequest<int>;
    public class AddEnrollmentOrchestratorHandler : IRequestHandler<AddEnrollmentOrchestrator, int>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public AddEnrollmentOrchestratorHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(AddEnrollmentOrchestrator request, CancellationToken cancellationToken)
        {
            await _unitOfWork.ExecuteAsync(async () =>
             {
                 var track = await _mediator.Send(new GetTrackByIdQuery(request.trackId));
                 if (track == null) { return; }
                 var intern = await _mediator.Send(new GetInternByIdQuery(request.internId));
                 if (intern == null) { return; }
                await _mediator.Send(new AddEnrollmentCommand(request.internId, request.trackId));
                
             });

            return 1;
        }
    }
}
