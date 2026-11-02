using LMS___Mini_Version.Features.Enrollment.AddEnrollment.Orchestrator;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollment.AddEnrollment.Endpoint
{
    public static class AddEnrollmentEndpoint
    {
        public static void AddEnrollment(this WebApplication app)
        {
            app.MapPost("api/Enrollment/{internId}/{trackId}", async (int internId, int trackId, IMediator _mediator) =>
            {
               var result = await _mediator.Send(new AddEnrollmentOrchestrator(internId, trackId));
                return Results.Ok(result);
            }).WithTags("Enrollments");
        }
    }
}
