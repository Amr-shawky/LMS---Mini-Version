using LMS___Mini_Version.Features.Enrollment.CancelEnrollment.Command;
using LMS___Mini_Version.Features.Enrollment.CancelEnrollment.Orchestrator;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollment.CancelEnrollment.Endpoint
{
    public static class CancelEnrollmentEndpoint
    {
        public static void CancelEnrollment(this WebApplication app)
        {
            app.MapPut("api/Enrollment/v2/{id}/cancel", async (int enrollmentId, IMediator _mediator) =>
            {
               var result = await _mediator.Send(new CancelEnrollmentOrchestrator(enrollmentId));
                if(result == false)
                {
                    return Results.BadRequest();
                }
                return Results.Ok();
            }).WithTags("Enrollments");
        }
    }
}
