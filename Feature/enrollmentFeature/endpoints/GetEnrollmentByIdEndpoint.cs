using exam_system.Features.Shared;
using LMS___Mini_Version.DTOs;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.endpoints
{
    public static class GetEnrollmentByIdEndpoint
    {
        public static void MapGetEnrollmentByIdEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/Enrollment/v2/{id:int}", async (int id, IMediator mediator) =>
            {
                var response = await mediator.Send(new GetEnrollmentByIdQuery(id));

                if (!response.Success)
                    return Results.Json(EndpointResponse<EnrollmentDto>.FromResult(response), statusCode: response.StatusCode);

                return Results.Ok(EndpointResponse<EnrollmentDto>.Ok(response.Data!, response.Message));
            })
            .WithTags("Enrollment V2")
            .WithName("GetEnrollmentById");
        }
    }
}