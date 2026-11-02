using LMS___Mini_Version.Features.Enrollment.TransferEnrollment.Orchestrator;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollment.TransferEnrollment.Endpoint
{
    public static class TransferEnrollmentEndpoint
    {
        public static void TransferEnrollment(this WebApplication app)
        {
            app.MapPut("api/Enrollment/v2/{enrollmentId}/transfer/{newTrackId}", async (int enrollmentId, int newTrackId, IMediator _mediator) =>
            {
                var result = await _mediator.Send(new TransferEnrollmentOrchestrator(enrollmentId, newTrackId));
                if (result == false)
                {
                    return Results.BadRequest();
                }
                return Results.Ok();
            }).WithTags("Enrollments");
        }
    }
}
