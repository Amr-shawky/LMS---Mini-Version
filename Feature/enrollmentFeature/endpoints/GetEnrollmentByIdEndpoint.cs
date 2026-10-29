using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.enrollmentFeature.Queries;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Enrollment;
using MediatR;

namespace LMS___Mini_Version.Feature.enrollmentFeature.endpoints
{
    public static class GetEnrollmentByIdEndpoint
    {
        public static void MapGetEnrollmentByIdEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/Enrollment/v2/{id}", async (IMediator mediator, int id) =>
            {
                var response = await mediator.Send(new GetEnrollmentByIdQuery(id));
                if (!response.Success || response.Data == null)
                {
                    return Results.BadRequest(EndpointResponse<EnrollmentViewModel>.Fail(response.Message, response.StatusCode, response.Errors));
                }
                var viewModel = response.Data.ToViewModel();
                return Results.Ok(EndpointResponse<EnrollmentViewModel>.Ok(viewModel));
            }).WithTags("Enrollment V2")
            .WithSummary("Get enrollment by ID (v2)");
        }
    }
}
