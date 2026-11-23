using LMS___Mini_Version.features.InternFeature.CreateIntern.Commands;
using LMS___Mini_Version.features.InternFeature.CreateIntern.Orchestrators;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.Queries;
using MediatR;
namespace LMS___Mini_Version.features.InternFeature.CreateIntern.handler
{
    public class CreateInternOrchestratorHandler : IRequestHandler<CreateInternOrchestrator, RequestResponse<int>>
    {
        private readonly IMediator _mediator;
        public CreateInternOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<RequestResponse<int>> Handle(CreateInternOrchestrator request, CancellationToken cancellationToken)
        {
            var errors = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(request.FullName))
            {
                errors.Add("FullName", new[] { "Full name is required" });
            }
            if (string.IsNullOrWhiteSpace(request.Email))
            {
                errors.Add("Email", new[] { "Email is required" });
            }
            if (request.BirthYear > DateTime.UtcNow.Year || request.BirthYear < 1900)
            {
                errors.Add("BirthYear", new[] { "Invalid birth year" });
            }
            if (request.TrackId <= 0)
            {
                errors.Add("TrackId", new[] { "Track Id must be greater than zero" });
            }
            if (errors.Count > 0)
            {
                return RequestResponse<int>.Fail(400, errors);
            }
            var trackResponse = await _mediator.Send(new GetTrackByIdQuery(request.TrackId), cancellationToken);
            if (!trackResponse.Success)
            {
                return RequestResponse<int>.Fail(trackResponse.StatusCode,trackResponse.Errors);
            }
            if (trackResponse.Data is null)
            {
                return RequestResponse<int>.Fail(404, new Dictionary<string, string[]> { { "Track", new[] { "Track data not found" } } });
            }

            if (!trackResponse.Data.IsActive)
            {
                return RequestResponse<int>.Fail(409, new Dictionary<string, string[]> { { "Track", new[] { "Track is inactive" } } });
            }
            var response = await _mediator.Send(new CreateInternCommand(request.FullName, request.Email, request.BirthYear, request.TrackId), cancellationToken);
            return response;
        }
    }
}
