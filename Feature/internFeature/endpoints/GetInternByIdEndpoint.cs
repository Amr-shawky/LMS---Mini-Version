using exam_system.Features.Shared;
using LMS___Mini_Version.Feature.internFeature.Queries;
using LMS___Mini_Version.Mapping;
using LMS___Mini_Version.ViewModels.Intern;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace LMS___Mini_Version.Feature.internFeature.endpoints
{
    public static class GetInternByIdEndpoint
    {
        public static void MapGetInternByIdEndpoint(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/Intern/v2/{id:int}", async (int id, IMediator mediator) =>
            {
                var response = await mediator.Send(new GetInternByIdQuery(id));
                if (!response.Success || response.Data == null)
                {
                    return Results.NotFound(EndpointResponse<InternDetailViewModel>.Fail(response.Message, response.StatusCode, response.Errors));
                }

                return Results.Ok(EndpointResponse<InternDetailViewModel>.Ok(response.Data.ToDetailViewModel()));
            }).WithTags("Intern V2")
            .WithSummary("Get intern by ID (v2)");
        }
    }
}
