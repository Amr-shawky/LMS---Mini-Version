using LMS___Mini_Version.features.InternFeature.CheckInternEmailExists.Query;
using LMS___Mini_Version.features.InternFeature.CreateIntern.Commands;
using LMS___Mini_Version.features.InternFeature.RegisterInternOrch.Orchestrator;
using LMS___Mini_Version.features.shared;
using LMS___Mini_Version.features.TrackFeature.GetTrackById.Query;
using MediatR;

namespace LMS___Mini_Version.features.InternFeature.RegisterInternOrch.Handler
{
    public class RegisterInternOrchestratorHandler
        : IRequestHandler<RegisterInternOrchestrator, RequestResponse<int>>
    {
        private readonly IMediator mediator;

        public RegisterInternOrchestratorHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<RequestResponse<int>> Handle(
            RegisterInternOrchestrator request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.FullName))
            {
                return RequestResponse<int>.Fail(
                    400,
                    new Dictionary<string, string[]>
                    {
                        ["FullName"] = new[] { "Full name is required." }
                    });
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                return RequestResponse<int>.Fail(
                    400,
                    new Dictionary<string, string[]>
                    {
                        ["Email"] = new[] { "Email is required." }
                    });
            }

            if (request.BirthYear <= 0 ||
                request.BirthYear > DateTime.UtcNow.Year)
            {
                return RequestResponse<int>.Fail(
                    400,
                    new Dictionary<string, string[]>
                    {
                        ["BirthYear"] = new[] { "Invalid birth year." }
                    });
            }
            var trackResult = await mediator.Send(
                new GetTrackByIdQuery(request.TrackId),
                cancellationToken);
            if (!trackResult.Success)
            {
                return RequestResponse<int>.Fail(
                    trackResult.StatusCode,
                    trackResult.Errors
                );
            }

            if (trackResult.Data == null)
            {
                return RequestResponse<int>.Fail(
                    404,
                    new Dictionary<string, string[]>
                    {
                        ["TrackId"] = new[] { "Track not found." }
                    });
            }
            if (!trackResult.Data.IsActive)
            {
                return RequestResponse<int>.Fail(
                    400,
                    new Dictionary<string, string[]>
                    {
                        ["TrackId"] = new[] { "Track is inactive." }
                    });
            }

            var emailResult = await mediator.Send(
                new CheckInternEmailExistsQuery(request.Email),
                cancellationToken);
            if (!emailResult.Success)
            {
                return RequestResponse<int>.Fail(
                    emailResult.StatusCode,
                    emailResult.Errors
                );
            }
            if (emailResult.Data)
            {
                return RequestResponse<int>.Fail(
                    409,
                    new Dictionary<string, string[]>
                    {
                        ["Email"] = new[] { "Email already exists." }
                    });
            }

            var createResult = await mediator.Send(
                new CreateInternCommand(
                    request.FullName,
                    request.Email,
                    request.BirthYear,
                    request.TrackId),
                cancellationToken);

            return createResult;
        }
    }
}