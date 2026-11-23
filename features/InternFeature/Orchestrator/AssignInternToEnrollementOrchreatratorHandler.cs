using LMS___Mini_Version.features.EnrollementFeature.Commands;
using LMS___Mini_Version.features.InternFeature.Commands;
using LMS___Mini_Version.features.InternFeature.DTOs;
using LMS___Mini_Version.features.shared;
using MediatR;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LMS___Mini_Version.features.InternFeature.Orchestrator
{
    public class AssignInternToEnrollementOrchreatratorHandler : IRequestHandler<AssignInternToEnrollementOrchreatrator, RequestResponse<CreateInternDTO>>
    {
        private readonly IMediator mediator;

        public AssignInternToEnrollementOrchreatratorHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async  Task<RequestResponse<CreateInternDTO>> Handle(AssignInternToEnrollementOrchreatrator request, CancellationToken cancellationToken)
        {

            var res = await mediator.Send(new AssignInternIntoTrackCommand(request.fullName, request.Email, request.birthYear, request.trackId));
            if(!res.Success || res.Data is null)
            {
                return RequestResponse<CreateInternDTO>.Fail(400, res.Errors);
            }
            int internId = res.Data.Id;
            var res2 = await mediator.Send(new CreateEnrollementCommand(internId, request.trackId, Domain.Enums.EnrollmentStatus.Pending, DateTime.Now));
            if(!res2.Success)
            {
                return RequestResponse<CreateInternDTO>.Fail(400, res2.Errors);
            }
            return RequestResponse<CreateInternDTO>.OK(res.Data, 200);
        }
    }
}
