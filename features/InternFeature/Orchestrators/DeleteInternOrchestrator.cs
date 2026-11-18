using LMS___Mini_Version.features.EnrollmentFeature.DTOs;
using LMS___Mini_Version.features.InternFeature.Commands;
using LMS___Mini_Version.features.InternFeature.Queries;
using LMS___Mini_Version.features.shared;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.Orchestrators
{
    public record DeleteInternOrchestrator(int internId): IRequest<RequestResponse>
    {
    }

    public class DeleteInternOrchestratorHandler : IRequestHandler<DeleteInternOrchestrator, RequestResponse>
    {
        private readonly IMediator _mediator;

        public DeleteInternOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<RequestResponse> Handle(DeleteInternOrchestrator request, CancellationToken cancellationToken)
        {
            if(request.internId <= 0)
                return RequestResponse.Fail(ErrorCode.InvalidInternId);

            var internExists = await _mediator.Send(new CheckInternExistsQuery(request.internId), cancellationToken);

            if(!internExists.Data)
                return RequestResponse.Fail(ErrorCode.InternNotFound, 404);

            var enrollmentsExist = await _mediator.Send(new CheckInternEnrollmentsQuery(request.internId));

            if(enrollmentsExist.Data)
                return RequestResponse.Fail(ErrorCode.InternHasEnrollments);

            return await _mediator.Send(new DeleteInternCommand(request.internId));




        }
    }
}
