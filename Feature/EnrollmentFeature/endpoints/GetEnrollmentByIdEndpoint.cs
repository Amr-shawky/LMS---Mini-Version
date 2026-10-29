using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Enrollment;
using MediatR;

namespace LMS___Mini_Version.Feature.EnrollmentFeature.endpoints
{
    public static class GetEnrollmentByIdEndpoint
    {
        public static void MapGetEnrollmentByIdEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/Enrollment/v2/{id:int}", async (int id, IMediator _mediator) =>
            {
                var response = await _mediator.Send(new GetEnrollmentByIdQuery(id));


                if (!response.Success || response.Data == null)
                {
                    return Results.Json(
                        EndpointResponse<EnrollmentViewModel>.Fail(
                            response.Message, response.StatusCode, response.Errors),
                        statusCode: response.StatusCode);
                }

                return Results.Ok(EndpointResponse<EnrollmentViewModel>.Ok(response.Data.ToViewModel()));
            }).WithTags("Enrollment V2")
            .WithSummary("Get enrollment by ID (v2)");
        }
    }
}
