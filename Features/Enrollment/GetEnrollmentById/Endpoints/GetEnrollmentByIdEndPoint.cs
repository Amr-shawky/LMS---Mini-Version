using LMS___Mini_Version.Features.Enrollment.GetEnrollmentById.Queries;
using MediatR;

namespace LMS___Mini_Version.Features.Enrollment.GetEnrollmentById.Endpoints
{
    public static class GetEnrollmentByIdEndPoint
    {
        public static void GetEnrollmentById(this WebApplication app)
        {
            app.MapGet("api/Enrollment/v2/{id}", async (int id, IMediator _mediator) =>
            {
                var enrollment = await _mediator.Send(new GetEnrollmentByIdQuery(id));
                if (enrollment == null)
                {
                    return Results.NotFound();
                }
                return Results.Ok(enrollment);
            }).WithTags("Enrollments");
        }
    }
}
